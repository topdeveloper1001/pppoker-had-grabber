//-----------------------------------------------------------------------
// <copyright file="TcpPacketImporter.cs" company="Ace Poker Solutions">
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
using PacketDotNet;
using PPPokerCardCatcher.Common.Extensions;
using PPPokerCardCatcher.Common.Linq;
using PPPokerCardCatcher.Common.Log;
using PPPokerCardCatcher.Common.Resources;
using PPPokerCardCatcher.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace PPPokerCardCatcher.Importers.TcpBased
{
    internal abstract class TcpPacketImporter : BaseImporter, ITcpPacketImporter
    {
        #region ITcpPacketImporter implementation

        public abstract void AddPacket(CapturedPacket capturedPacket);

        public abstract bool Match(TcpPacket tcpPacket, IpPacket ipPacket);

        #endregion

        protected const int StopTimeout = 5000;
        protected const int NoDataDelay = 100;
        protected const int DelayToProcessHands = 2000;

        protected IDHImporterService dhImporterService;

        protected bool IsAdvancedLogEnabled { get; set; }

#if DEBUG
        protected abstract string HandHistoryFilePrefix
        {
            get;
        }
#endif

        /// <summary>
        /// Exports captured hand history to the supported DB
        /// </summary>
        protected virtual void ExportHandHistory(HandHistoryData handHistoryData, ConcurrentDictionary<long, List<HandHistoryData>> handHistoriesToProcess)
        {
            LogProvider.Log.Info(this, $"Hand #{handHistoryData.HandHistory.HandId} is being prepared [{handHistoryData.Uuid}].");

            var handId = handHistoryData.HandHistory.HandId;

            if (!handHistoriesToProcess.TryGetValue(handId, out List<HandHistoryData> handHistoriesData))
            {
                handHistoriesData = new List<HandHistoryData>();
                handHistoriesToProcess.AddOrUpdate(handId, handHistoriesData, (key, prev) => handHistoriesData);

                Task.Run(() =>
                {
                    try
                    {
                        Task.Delay(DelayToProcessHands).Wait(cancellationTokenSource.Token);
                        ExportHandHistory(handHistoriesData.ToList());
                    }
                    catch (OperationCanceledException)
                    {
                        return;
                    }
                    finally
                    {
                        handHistoriesToProcess.TryRemove(handId, out List<HandHistoryData> removedData);
                    }
                });
            }

            handHistoriesData.Add(handHistoryData);
        }

        /// <summary>
        /// Exports captured hand history to the supported DB
        /// </summary>
        protected virtual void ExportHandHistory(List<HandHistoryData> handHistories)
        {
            InitializeDHImporterService();

            // merge hands
            var playerWithHoleCards = handHistories
                .SelectMany(x => x.HandHistory.Players)
                .Where(x => x.hasHoleCards)
                .DistinctBy(x => x.SeatNumber)
                .ToDictionary(x => x.SeatNumber, x => x.HoleCards);

            foreach (var handHistoryData in handHistories)
            {
                handHistoryData.HandHistory.Players.ForEach(player =>
                {
                    if (!player.hasHoleCards && playerWithHoleCards.ContainsKey(player.SeatNumber))
                    {
                        player.HoleCards = playerWithHoleCards[player.SeatNumber];
                    }
                });

                handHistoryData.HandHistory.Players = GetPlayerList(handHistoryData.HandHistory);

                var handHistoryText = SerializationHelper.SerializeObject(handHistoryData.HandHistory);

#if DEBUG                                
                if (!Directory.Exists("Hands"))
                {
                    Directory.CreateDirectory("Hands");
                }

                File.WriteAllText($"Hands\\{HandHistoryFilePrefix}_hand_exported_{handHistoryData.Uuid}_{handHistoryData.HandHistory.HandId}.xml", handHistoryText);
#endif

                try
                {
                    var handHistoryDto = new HandHistoryDto
                    {
                        PokerSite = EnumPokerSites.PPPoker,
                        WindowHandle = handHistoryData.WindowHandle.ToInt32(),
                        HandText = handHistoryText
                    };

                    dhImporterService.ImportHandHistory(handHistoryDto);
                    LogProvider.Log.Info($"Hand #{handHistoryData.HandHistory.HandId} has been sent. [{handHistoryData.HandHistory.TableName}]");
                }
                catch (EndpointNotFoundException)
                {
                    LogProvider.Log.Error(this, $"DriveHUD isn't running. Hand #{handHistoryData.HandHistory.HandId} hasn't been sent. [{handHistoryData.HandHistory.TableName}]");
                    dhImporterService = null;
                }
                catch (Exception e)
                {
                    if (IsAdvancedLogEnabled)
                    {
                        LogProvider.Log.Error(this, $"Hand #{handHistoryData.HandHistory.HandId} hasn't been sent. [{handHistoryData.HandHistory.TableName}]", e);
                    }
                    else
                    {
                        LogProvider.Log.Error(this, $"Hand #{handHistoryData.HandHistory.HandId} hasn't been sent. [{handHistoryData.HandHistory.TableName}]");
                    }

                    dhImporterService = null;
                }
            }
        }

        protected virtual void InitializeDHImporterService()
        {
            if (dhImporterService != null)
            {
                return;
            }

            var endpointAddress = CommonResourceManager.Instance.GetResourceString("SystemSettings_ImporterPipeAddress");
            var pipeFactory = new ChannelFactory<IDHImporterService>(new NetNamedPipeBinding(), new EndpointAddress(endpointAddress));
            dhImporterService = pipeFactory.CreateChannel();
        }

        protected abstract PlayerList GetPlayerList(HandHistory handHistory);

        protected class HandHistoryData
        {
            public long Uuid { get; set; }

            public HandHistory HandHistory { get; set; }

            public IntPtr WindowHandle { get; set; }
        }
    }
}