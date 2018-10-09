//-----------------------------------------------------------------------
// <copyright file="TcpImporter.cs" company="Ace Poker Solutions">
// Copyright © 2018 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using DriveHUD.Common.Linq;
using DriveHUD.Common.Log;
using DriveHUD.Entities;
using Microsoft.Practices.ServiceLocation;
//using Model.Settings;
using PacketDotNet;
using SharpPcap;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace DriveHUD.Importers.AndroidBase
{
    internal class TcpImporter : BaseImporter, ITcpImporter
    {
        private const int StopTimeout = 5000;

        private const int NoDataDelay = 100;

        private readonly ManualResetEventSlim captureResetEvent = new ManualResetEventSlim();

        private readonly BlockingCollection<ITcpPacketImporter> importers = new BlockingCollection<ITcpPacketImporter>();

        private readonly ConcurrentStack<CaptureDevice> captureDevices = new ConcurrentStack<CaptureDevice>();

        protected override EnumPokerSites Site => EnumPokerSites.Unknown;

        public override string SiteString => "TcpImporter";

        private readonly bool isAdvancedLogEnabled = true;

        //private readonly IImporterService importerService;

        public TcpImporter()
        {
            //importerService = ServiceLocator.Current.GetInstance<IImporterService>(); ;
            var importer = new DriveHUD.Importers.PPPoker.PPPImporter();
            importers.Add(importer);
            importer.Start();
        }

        public void RegisterImporter<T>() where T : ITcpPacketImporter
        {
            //if (importerService == null)
            //{
            //    return;
            //}

            //var tcpPacketImporter = importerService.GetImporter<T>();

            //if (tcpPacketImporter != null)
            //{
            //    importers.Add(tcpPacketImporter);
            //}
        }

        protected override void DoImport()
        {
            try
            {
                InitializeSettings();
                StartNetworkDataCapture();

                while (!cancellationTokenSource.IsCancellationRequested)
                {
                    try
                    {
                        Task.Delay(StopTimeout).Wait(cancellationTokenSource.Token);
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                }

            }
            catch (Exception e)
            {
                LogProvider.Log.Error(this, $"Failed to capture network data. [{SiteString}]", e);
            }

            captureResetEvent.Wait(StopTimeout);
            captureDevices.Clear();

            RaiseProcessStopped();
        }

        /// <summary>
        /// Reads settings and initializes importer variables
        /// </summary>
        protected void InitializeSettings()
        {
            //var settings = ServiceLocator.Current.GetInstance<ISettingsService>().GetSettings();

            //if (settings != null)
            //{
            //    isAdvancedLogEnabled = settings.GeneralSettings.IsAdvancedLoggingEnabled;
            //}
        }

        /// <summary>
        /// Starts processes to capture network data for all available devices
        /// </summary>
        protected void StartNetworkDataCapture()
        {
            captureResetEvent.Set();

            var devices = CaptureDeviceList.Instance;

            foreach (var device in devices)
            {
                if (isAdvancedLogEnabled)
                {
                    LogProvider.Log.Info(this, $"Found device: {device.Name}, {device.Description}. [{SiteString}]");
                }

                try
                {
                    device.Open(DeviceMode.Normal);

                    var captureDevice = new CaptureDevice(device)
                    {
                        IsOpened = true
                    };

                    captureDevices.Push(captureDevice);
                }
                catch (Exception e)
                {
                    LogProvider.Log.Error(this, $"Failed to open device {device.Name}. [{SiteString}]", e);
                }
            }

            captureDevices.ForEach(captureDevice => Task.Run(() => CaptureData(captureDevice)));
        }

        /// <summary>
        /// Captures network data of the specified device
        /// </summary>
        /// <param name="device">Device to capture</param>
        protected void CaptureData(CaptureDevice captureDevice)
        {
            if (captureResetEvent.IsSet)
            {
                captureResetEvent.Reset();
            }

            while (true)
            {
                if (cancellationTokenSource.IsCancellationRequested)
                {
                    try
                    {
                        captureDevice.Device.Close();
                        captureDevice.IsOpened = false;
                    }
                    catch (Exception e)
                    {
                        LogProvider.Log.Error(this, $"Device {captureDevice.Device.Name} has not been closed. [{SiteString}]", e);
                    }

                    if (!captureResetEvent.IsSet && captureDevices.All(x => !x.IsOpened))
                    {
                        captureResetEvent.Set();
                    }

                    return;
                }

                try
                {
                    var nextPacket = captureDevice.Device.GetNextPacket();

                    if (nextPacket != null)
                    {
                        ParsePacket(nextPacket);
                    }
                    else
                    {
                        Task.Delay(NoDataDelay).Wait();
                    }
                }
                catch (Exception e)
                {
                    LogProvider.Log.Error(this, $"Data has not been captured from {captureDevice.Device.Name}. [{SiteString}]", e);
                }
            }
        }

        /// <summary>
        /// Parses captured data into the packet
        /// </summary>
        /// <param name="rawCapture">Captured data</param>
        protected void ParsePacket(RawCapture rawCapture)
        {
            var packet = Packet.ParsePacket(rawCapture.LinkLayerType, rawCapture.Data);

            var tcpPacket = packet.Extract<TcpPacket>();
            var ipPacket = packet.Extract<IpPacket>();

            if (tcpPacket == null || ipPacket == null)
            {
                return;
            }

            var matchedImporter = importers.FirstOrDefault(x => x.IsRunning && !x.IsDisabled() && x.Match(tcpPacket, ipPacket));

            if (matchedImporter == null)
            {
                return;
            }

            var payloadData = tcpPacket.PayloadData;

            if (payloadData == null || payloadData.Length == 0)
            {
                return;
            }

            var capturedPacket = new CapturedPacket
            {
                Bytes = tcpPacket.PayloadData,
                Source = new IPEndPoint(ipPacket.SourceAddress, tcpPacket.SourcePort),
                Destination = new IPEndPoint(ipPacket.DestinationAddress, tcpPacket.DestinationPort),
                CreatedTimeStamp = rawCapture.Timeval.Date,
                SequenceNumber = tcpPacket.SequenceNumber
            };

            matchedImporter.AddPacket(capturedPacket);
        }

        /// <summary>
        /// Determines whenever importer is disabled
        /// </summary>
        /// <returns>True if importer is disabled; otherwise - false</returns>
        public override bool IsDisabled()
        {
            return importers.All(x => x.IsDisabled());
        }
    }
}