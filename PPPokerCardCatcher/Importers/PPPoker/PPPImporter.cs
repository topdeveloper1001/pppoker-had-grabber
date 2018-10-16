//-----------------------------------------------------------------------
// <copyright file="PPPImporter.cs" company="Ace Poker Solutions">
// Copyright © 2018 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using HandHistories.Objects.Hand;
using HandHistories.Objects.Players;
using Microsoft.Practices.ServiceLocation;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PacketDotNet;
using PPPokerCardCatcher.Common.Extensions;
using PPPokerCardCatcher.Common.Log;
using PPPokerCardCatcher.Common.Resources;
using PPPokerCardCatcher.Entities;
using PPPokerCardCatcher.Importers.AndroidBase;
using PPPokerCardCatcher.Importers.PPPoker.Model;
using PPPokerCardCatcher.Importers.TcpBased;
using PPPokerCardCatcher.Licensing;
using PPPokerCardCatcher.Settings;
using ProtoBuf;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PPPokerCardCatcher.Importers.PPPoker
{
    internal class PPPImporter : TcpPacketImporter, IPPPImporter
    {
        private readonly ILicenseService licenseService;

        private readonly BlockingCollection<CapturedPacket> packetBuffer = new BlockingCollection<CapturedPacket>();

        protected IProtectedLogger protectedLogger;

        public override string ImporterName => "PPPImporter";

#if DEBUG
        protected override string HandHistoryFilePrefix => "ppp";
#endif        

        private Dictionary<PackageType, MethodInfo> LogPackageMapping { get; } = new Dictionary<PackageType, MethodInfo>();

        public PPPImporter()
        {
            licenseService = ServiceLocator.Current.GetInstance<ILicenseService>();

            PackageTypeEnumerator.ForEach((PackageType enumValue, Type packageObjectType) =>
            {
                Action<PPPokerPackage> action = LogPackage<HeartBeatREQ>;
                MethodInfo method = action.Method.GetGenericMethodDefinition();
                MethodInfo generic = method.MakeGenericMethod(packageObjectType);

                LogPackageMapping[enumValue] = generic;
            });
        }

        #region Implementation of ITcpImporter

        public override bool Match(TcpPacket tcpPacket, IpPacket ipPacket)
        {
            return tcpPacket.SourcePort == PPPConstants.Port || tcpPacket.DestinationPort == PPPConstants.Port;
        }

        public override void AddPacket(CapturedPacket capturedPacket)
        {
            packetBuffer.Add(capturedPacket);
        }

        #endregion

        /// <summary>
        /// Main importer task, executes in the background thread
        /// </summary>
        protected override void DoImport()
        {
            try
            {
                InitializeLogger();
                InitializeSettings();
                ProcessBuffer();
            }
            catch (Exception e)
            {
                LogProvider.Log.Error(this, $"Failed importing.", e);
            }

            packetBuffer.Clear();

            protectedLogger.StopLogging();

            RaiseProcessStopped();
        }

        /// <summary>
        /// Initializes logger with protection
        /// </summary>
        protected virtual void InitializeLogger()
        {
            var logger = new ProtectedLoggerConfiguration
            {
                DateFormat = "yyy-MM-dd",
                DateTimeFormat = "HH:mm:ss",
                LogCleanupTemplate = "ppp-games-*-*-*.log",
                LogDirectory = "Logs",
                LogTemplate = "ppp-games-{0}.log",
                MessagesInBuffer = 10
            };

            protectedLogger = ServiceLocator.Current.GetInstance<IProtectedLogger>();
            protectedLogger.Initialize(logger);
            protectedLogger.CleanLogs();
            protectedLogger.StartLogging();
        }

        /// <summary>
        /// Reads settings and initializes importer variables
        /// </summary>
        protected void InitializeSettings()
        {
            var settings = ServiceLocator.Current.GetInstance<ISettingsService>().GetSettings();

            if (settings != null)
            {
                IsAdvancedLogEnabled = settings.IsAdvancedLoggingEnabled;
            }
        }

        /// <summary>
        /// Processes packets in the buffer
        /// </summary>
        protected void ProcessBuffer()
        {
            var tableWindowProvider = ServiceLocator.Current.GetInstance<ITableWindowProvider>();
            var packetManager = ServiceLocator.Current.GetInstance<IPacketManager<PPPokerPackage>>();
            var handBuilder = ServiceLocator.Current.GetInstance<IPPPHandBuilder>();

            var connectionsService = ServiceLocator.Current.GetInstance<INetworkConnectionsService>();
            var detectedTables = new HashSet<IntPtr>();
            var handHistoriesToProcess = new ConcurrentDictionary<long, List<HandHistoryData>>();

            while (!cancellationTokenSource.IsCancellationRequested)
            {
                try
                {
                    if (!packetBuffer.TryTake(out CapturedPacket capturedPacket))
                    {
                        Task.Delay(NoDataDelay).Wait();
                        continue;
                    }

                    LogPacket(capturedPacket, ".log");

                    if (IsAdvancedLogEnabled)
                    {
                        LogPacket(capturedPacket, ".log");
                    }

                    if (!packetManager.TryParse(capturedPacket, out IList<PPPokerPackage> packages))
                    {
                        continue;
                    }

                    foreach (var package in packages)
                    {
                        if (IsAdvancedLogEnabled)
                        {
                            LogPackage(package);
                        }

                        if (!IsAllowedPackage(package))
                        {
                            continue;
                        }

                        var process = connectionsService.GetProcess(capturedPacket);
                        var windowHandle = tableWindowProvider.GetTableWindowHandle(process);

                        if (package.PackageType != PackageType.SelUserInfoRSP && !detectedTables.Contains(windowHandle))
                        {
                            detectedTables.Add(windowHandle);

                            if (package.PackageType == PackageType.EnterRoomRSP || handBuilder.IsRoomSnapShotAvailable(package))
                            {
                                LogProvider.Log.Info(this, $"User entered room. [{package.ClientPort}]");
                                SendShowHUDRequest("Notifications_HudLayout_PreLoadingText_Init", windowHandle);
                            }
                            else
                            {                                
                                SendShowHUDRequest("Notifications_HudLayout_PreLoadingText_CanNotBeCapturedText", windowHandle);
                            }
                        }

                        if (handBuilder.TryBuild(package, out HandHistory handHistory))
                        {
                            if (IsAdvancedLogEnabled)
                            {
                                LogProvider.Log.Info(this, $"Hand #{handHistory.HandId} [{package.ClientPort}]");
                            }

                            var handHistoryData = new HandHistoryData
                            {
                                Uuid = windowHandle.ToInt32(),
                                HandHistory = handHistory,
                                WindowHandle = windowHandle
                            };

                            if (!licenseService.IsMatch(handHistory))
                            {
                                LogProvider.Log.Info(this, $"License doesn't support cash hand {handHistory.HandId}. [BB={handHistory.GameDescription.Limit.BigBlind}]");
                                SendShowHUDRequest("Notifications_HudLayout_PreLoadingText_NoLicense", windowHandle);
                                continue;
                            }

                            ExportHandHistory(handHistoryData, handHistoriesToProcess);
                        }

                        if (package.PackageType == PackageType.LeaveRoomRSP || package.PackageType == PackageType.TableGameOverRSP)
                        {
                            SendCloseHUDRequest(windowHandle);
                            detectedTables.Remove(windowHandle);
                        }
                    }
                }
                catch (Exception e)
                {
                    LogProvider.Log.Error(this, $"Could not process captured packet.", e);
                }
            }

            SendCloseHUDRequest(detectedTables.ToArray());
        }

        /// <summary>
        /// Parses package body into the specified type <see cref="{T}"/>, then performs <paramref name="onSuccess"/> if parsing succeed, 
        /// or <paramref name="onFail"/> if parsing failed
        /// </summary>
        /// <typeparam name="T">Type of the package body</typeparam>
        /// <param name="package">Package to parse</param>
        /// <param name="onSuccess">Action to perform if parsing succeed</param>
        /// <param name="onFail">Action to perform if parsing failed</param>
        protected virtual void ParsePackage<T>(PPPokerPackage package, Action<T> onSuccess, Action onFail)
        {
            if (SerializationHelper.TryDeserialize(package.Body, out T packageBody))
            {
                onSuccess?.Invoke(packageBody);
                return;
            }

            LogProvider.Log.Warn(this, $"Failed to deserialize {typeof(T)} package");

            onFail?.Invoke();
        }

        /// <summary>
        /// Checks whenever the specified package has to be processed
        /// </summary>
        /// <param name="package">Package to check</param>
        /// <returns></returns>
        protected static bool IsAllowedPackage(PPPokerPackage package)
        {
            switch (package.PackageType)
            {
                case PackageType.GetUserMarksREQ:
                case PackageType.GetUserMarksRSP:
                case PackageType.SelUserInfoRSP:
                case PackageType.EnterRoomRSP:
                case PackageType.SitDownRSP:
                case PackageType.SitDownBRC:
                case PackageType.StandUpBRC:
                case PackageType.BlindStatusBRC:
                case PackageType.DealerInfoRSP:
                case PackageType.RoundStartBRC:
                case PackageType.RoundOverBRC:
                case PackageType.ActionBRC:
                case PackageType.HandCardRSP:
                case PackageType.ShowHandRSP:
                case PackageType.WinnerRSP:
                case PackageType.ShowMyCardBRC:
                case PackageType.UserSngOverRSP:
                case PackageType.TableGameOverRSP:
                case PackageType.LeaveRoomRSP:
                    return true;

                default:
                    return false;
            }
        }

        #region Debug logging

        protected virtual void LogPackage(PPPokerPackage package)
        {
            if (LogPackageMapping.ContainsKey(package.PackageType))
            {
                LogPackageMapping[package.PackageType].Invoke(this, new object[] { package });
            }
        }

        protected virtual void LogPackage<T>(PPPokerPackage package)
        {
            try
            {
                if (package.PackageType == PackageType.HeartBeatREQ || package.PackageType == PackageType.HeartBeatRSP)
                {
                    return;
                }

#if DEBUG
                if (!SerializationHelper.TryDeserialize(package.Body, out T packageContent))
                {
                    LogProvider.Log.Warn(this, $"Failed to deserialize {typeof(T)} package");
                }

                var packageJson = new PackageJson<T>
                {
                    PackageType = package.PackageType,
                    Direction = package.Direction,
                    ClientPort = package.ClientPort,
                    Content = packageContent,
                    Time = package.Timestamp
                };

                var json = JsonConvert.SerializeObject(packageJson, Formatting.Indented, new StringEnumConverter());

                protectedLogger.Log(json);
#else
                using (var ms = new MemoryStream())
                {
                    Serializer.Serialize(ms, package);
                    var packageBytes = ms.ToArray();

                    var logText = Encoding.UTF8.GetString(packageBytes);
                    protectedLogger.Log(logText);
                }
#endif

            }
            catch (Exception e)
            {
                LogProvider.Log.Error(this, "Failed to log package", e);
            }
        }

        private void LogPacket(CapturedPacket capturedPacket, string ext)
        {
            var packageFileName = Path.Combine("Logs", capturedPacket.ToString().Replace(":", ".").Replace("->", "-")) + ext;

            var pppManager = new PPPokerPacketManager();

            var sb = new StringBuilder();
            sb.AppendLine("-----------begin----------------");
            sb.AppendLine($"Date: {capturedPacket.CreatedTimeStamp}");
            sb.AppendLine($"Date Now: {DateTime.Now}");
            sb.AppendLine($"Date Now (ticks): {DateTime.Now.Ticks}");
            sb.AppendLine($"SequenceNumber: {capturedPacket.SequenceNumber}");
            sb.AppendLine($"Packet Full Length: {(pppManager.IsStartingPacket(capturedPacket.Bytes) ? pppManager.ReadPacketLength(capturedPacket.Bytes) : 0)}");
            sb.AppendLine($"Packet Current Length: {capturedPacket.Bytes.Length}");
            sb.AppendLine($"Body:");
            sb.AppendLine("---------body begin-------------");
            sb.AppendLine(BitConverter.ToString(capturedPacket.Bytes).Replace("-", " "));
            sb.AppendLine("----------body end--------------");
            sb.AppendLine("--------------ascii-------------");
            sb.AppendLine(Encoding.ASCII.GetString(capturedPacket.Bytes.Skip(4).ToArray()));
            sb.AppendLine("------------ascii end-----------");
            sb.AppendLine("------------end-----------------");

            File.AppendAllText(packageFileName, sb.ToString());
        }

        private class PackageJson<T>
        {
            public PackageType PackageType { get; set; }

            public PackageDirection Direction { get; set; }

            public int ClientPort { get; set; }

            public DateTime Time { get; set; }

            public T Content { get; set; }
        }

        #endregion

        private readonly Dictionary<int, int> autoCenterSeats = new Dictionary<int, int>
        {
            { 2, 1 },
            { 3, 1 },
            { 4, 1 },
            { 5, 1 },
            { 6, 1 },
            { 7, 1 },
            { 8, 1 },
            { 9, 1 }
        };

        protected override PlayerList GetPlayerList(HandHistory handHistory)
        {
            var playerList = handHistory.Players;

            var maxPlayers = handHistory.GameDescription.SeatType.MaxPlayers;

            var heroSeat = handHistory.Hero != null ? handHistory.Hero.SeatNumber : 0;

            if (heroSeat != 0 && autoCenterSeats.ContainsKey(maxPlayers))
            {
                var prefferedSeat = autoCenterSeats[maxPlayers];

                var shift = (prefferedSeat - heroSeat) % maxPlayers;

                foreach (var player in playerList)
                {
                    player.SeatNumber = ShiftPlayerSeat(player.SeatNumber, shift, maxPlayers);
                }
            }

            return playerList;
        }

        protected virtual void SendShowHUDRequest(string loadingTextKey, IntPtr windowHandle)
        {
            if (windowHandle == IntPtr.Zero)
            {
                return;
            }

            var loadingText = CommonResourceManager.Instance.GetResourceString(loadingTextKey);

            InitializeDHImporterService();

            if (dhImporterService == null)
            {
                return;
            }

            try
            {
                dhImporterService.ShowHUD(EnumPokerSites.PPPoker, windowHandle.ToInt32(), loadingText);
            }
            catch (EndpointNotFoundException)
            {
                LogProvider.Log.Error(this, $"DriveHUD isn't running. Request to show HUD hasn't been sent [{windowHandle.ToInt32()}].");
                dhImporterService = null;
            }
            catch (Exception e)
            {
                if (IsAdvancedLogEnabled)
                {
                    LogProvider.Log.Error(this, $"Failed to send request to show HUD [{windowHandle.ToInt32()}].", e);
                }

                dhImporterService = null;
            }
        }

        protected virtual void SendCloseHUDRequest(IntPtr windowHandle)
        {
            // add delay
            Task.Run(() =>
            {
                Task.Delay(DelayToProcessHands * 2).Wait();
                SendCloseHUDRequest(new[] { windowHandle });
            });
        }

        protected virtual void SendCloseHUDRequest(IntPtr[] windowHandles)
        {
            InitializeDHImporterService();

            foreach (var windowHandle in windowHandles)
            {
                try
                {
                    dhImporterService.CloseHUD(windowHandle.ToInt32());
                }
                catch (EndpointNotFoundException)
                {
                    LogProvider.Log.Error(this, $"DriveHUD isn't running. Request to close HUD hasn't been sent [{windowHandle.ToInt32()}].");
                    dhImporterService = null;
                }
                catch (Exception e)
                {
                    if (IsAdvancedLogEnabled)
                    {
                        LogProvider.Log.Error(this, $"Failed to send request to close HUD [{windowHandle.ToInt32()}].", e);
                    }

                    dhImporterService = null;
                }
            }
        }

        private static int ShiftPlayerSeat(int seat, int shift, int tableType)
        {
            return (seat + shift + tableType - 1) % tableType + 1;
        }
    }
}