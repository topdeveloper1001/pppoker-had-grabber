﻿//-----------------------------------------------------------------------
// <copyright file="PPPHandBuilder.cs" company="Ace Poker Solutions">
// Copyright © 2018 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

//using DriveHUD.Common.Exceptions;
using DriveHUD.Common.Extensions;
using DriveHUD.Common.Linq;
using DriveHUD.Common.Log;
//using DriveHUD.Common.Resources;
using DriveHUD.Entities;
using DriveHUD.Importers.PPPoker.Model;
using HandHistories.Objects.Actions;
using HandHistories.Objects.Cards;
using HandHistories.Objects.GameDescription;
using HandHistories.Objects.Hand;
using HandHistories.Objects.Players;
using HandHistories.Parser.Utils;
//using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace DriveHUD.Importers.PPPoker
{
    class PPPHandBuilder : IPPPHandBuilder
    {
        private static readonly Dictionary<ActionType, HandActionType> BlindActionMapping = new Dictionary<ActionType, HandActionType>
        {
            { ActionType.Ante, HandActionType.ANTE },
            { ActionType.ForceBigBlind, HandActionType.POSTS },
            { ActionType.SmallBlind, HandActionType.SMALL_BLIND },
            { ActionType.BigBlind, HandActionType.BIG_BLIND },
            { ActionType.Straddle, HandActionType.STRADDLE },
        };

        private static readonly Dictionary<ActionType, HandActionType> ActionMapping = new Dictionary<ActionType, HandActionType>
        {
            { ActionType.Fold, HandActionType.FOLD },
            { ActionType.SystemFold, HandActionType.FOLD },
            { ActionType.Check, HandActionType.CHECK },
            { ActionType.SystemCheck, HandActionType.CHECK },
            { ActionType.Call, HandActionType.CALL },
            { ActionType.Bet, HandActionType.BET },
            { ActionType.Raise, HandActionType.RAISE },
            { ActionType.Pot, HandActionType.RAISE },
        };

        private static readonly HashSet<HandActionType> InvestingHandActionTypes = new HashSet<HandActionType>
        {
            HandActionType.CALL,
            HandActionType.BET,
            HandActionType.RAISE,
        };

        private static readonly HashSet<RoomType> TournamentRoomTypes = new HashSet<RoomType>
        {
            RoomType.SngRoom,
            RoomType.MttRoom,
        };

        private static readonly HashSet<PackageType> TerminatingPackageTypes = new HashSet<PackageType>
        {
            PackageType.TableGameOverRSP,
            PackageType.LeaveRoomRSP,
        };

        private readonly Dictionary<int, ClientRecord> clientRecords = new Dictionary<int, ClientRecord>();

        public bool TryBuild(PPPokerPackage package, out HandHistory handHistory)
        {
            handHistory = null;

            if (package == null)
            {
                return false;
            }

            if (!clientRecords.TryGetValue(package.ClientPort, out ClientRecord record))
            {
                record = clientRecords[package.ClientPort] = new ClientRecord { Port = package.ClientPort };
            }

            if (package.PackageType == PackageType.EnterRoomRSP)
            {
                ParsePackage<EnterRoomRSP>(package, m => ProcessEnterRoomRSP(m, record));
                return false;
            }
            // We use both GetUserMarksREQ and GetUserMarksRSP to get Uid and to be sure that no packets are lost and we successfully determined who is hero
            else if (package.PackageType == PackageType.GetUserMarksREQ)
            {
                ParsePackage<GetUserMarksREQ>(package, m => ProcessGetUserMarksREQ(m, record));
            }
            else if (package.PackageType == PackageType.GetUserMarksRSP)
            {
                ParsePackage<GetUserMarksRSP>(package, m => ProcessGetUserMarksRSP(m, record));
            }

            // Delay following actions until DealerInfoRSP message arrives
            // Otherwise these messages will not be processed becase ValidatePackages in BuildHand method will return false and package list will be cleared
            if (package.PackageType == PackageType.SelUserInfoRSP)
            {
                record.DelayedActions.Add(() => ParsePackage<SelUserInfoRSP>(package, m => ProcessSelUserInfoRSP(m, record)));
            }
            else if (package.PackageType == PackageType.SitDownRSP)
            {
                record.DelayedActions.Add(() => ParsePackage<SitDownRSP>(package, m => ProcessSitDownRSP(m, record)));
            }
            else if (package.PackageType == PackageType.BlindStatusBRC)
            {
                record.DelayedActions.Add(() => ParsePackage<BlindStatusBRC>(package, m => ProcessBlindStatusBRC(m, record)));
            }
            else if (package.PackageType == PackageType.SitDownBRC)
            {
                record.DelayedActions.Add(() => ParsePackage<SitDownBRC>(package, m => ProcessSitDownBRC(m, record)));
            }
            else if (package.PackageType == PackageType.StandUpBRC)
            {
                record.DelayedActions.Add(() => ParsePackage<StandUpBRC>(package, m => ProcessStandUpBRC(m, record)));
            }

            if (package.PackageType == PackageType.DealerInfoRSP || record.IsHandStarted && TerminatingPackageTypes.Contains(package.PackageType))
            {
                handHistory = BuildHand(record);

                if (record.DelayedActions.Count > 0)
                {
                    foreach (var action in record.DelayedActions)
                    {
                        action();
                    }
                    record.DelayedActions.Clear();
                }

                record.IsHandStarted = false;
            }

            record.Packages.Add(package);

            if (package.PackageType == PackageType.DealerInfoRSP)
            {
                record.IsHandStarted = true;
            }

            return handHistory != null;
        }

        private bool ValidatePackages(ClientRecord record)
        {
            var dealerInfoPackage = record.Packages.FirstOrDefault(x => x.PackageType == PackageType.DealerInfoRSP);

            if (dealerInfoPackage == null)
            {
                LogProvider.Log.Warn(CustomModulesNames.PPPCatcher, $"Hand cannot be built because DealerInfoRSP message is missing. [{record.Port}]");
                return false;
            }

            var isValid = false;

            ParsePackage<DealerInfoRSP>(dealerInfoPackage, x =>
            {
                isValid = record.Packages.Any(p => p.PackageType == PackageType.WinnerRSP);
                if (!isValid)
                {
                    LogProvider.Log.Warn(CustomModulesNames.PPPCatcher, $"Hand GameID = {x.GameID} cannot be built because WinnerRSP message is missing. [{record.Port}]");
                }
            });

            return isValid;
        }

        private HandHistory BuildHand(ClientRecord record)
        {
            HandHistory history = null;

            try
            {
                if (!ValidatePackages(record))
                {
                    return history;
                }

                var isGameStarted = false;

                foreach (var package in record.Packages)
                {
                    if (package.PackageType == PackageType.DealerInfoRSP)
                    {
                        history = new HandHistory
                        {
                            DateOfHandUtc = package.Timestamp.ToUniversalTime()
                        };

                        ParsePackage<DealerInfoRSP>(package, m => ProcessDealerInfoRSP(m, record, history));
                        isGameStarted = true;
                        continue;
                    }

                    if (!isGameStarted)
                    {
                        continue;
                    }

                    switch (package.PackageType)
                    {
                        case PackageType.RoundStartBRC:
                            ParsePackage<RoundStartBRC>(package, m => ProcessRoundStartBRC(m, record, history));
                            break;
                        case PackageType.RoundOverBRC:
                            ParsePackage<RoundOverBRC>(package, m => ProcessRoundOverBRC(m, record, history));
                            break;
                        case PackageType.ActionBRC:
                            ParsePackage<ActionBRC>(package, m => ProcessActionBRC(m, record, history));
                            break;
                        case PackageType.HandCardRSP:
                            ParsePackage<HandCardRSP>(package, m => ProcessHandCardRSP(m, record, history));
                            break;
                        case PackageType.ShowHandRSP:
                            ParsePackage<ShowHandRSP>(package, m => ProcessShowHandRSP(m, record, history));
                            break;
                        case PackageType.WinnerRSP:
                            ParsePackage<WinnerRSP>(package, m => ProcessWinnerRSP(m, record, history));
                            break;
                        case PackageType.ShowMyCardBRC:
                            ParsePackage<ShowMyCardBRC>(package, m => ProcessShowMyCardBRC(m, record, history));
                            break;
                        case PackageType.UserSngOverRSP:
                            ParsePackage<UserSngOverRSP>(package, m => ProcessUserSngOverRSP(m, record, history));
                            break;
                    }
                }

                AdjustHandHistory(history);

                return history;
            }
            catch (Exception e)
            {
                LogProvider.Log.Error(CustomModulesNames.PPPCatcher, $"Failed to build hand #{history?.HandId ?? 0} room #{history?.GameDescription.Identifier ?? 0} [{record.Port}]", e);
                return null;
            }
            finally
            {
                record.Packages.Clear();
            }
        }

        private void AdjustHandHistory(HandHistory history)
        {
            const decimal divider = 100m;

            if (history == null)
            {
                return;
            }

            HandHistoryUtils.UpdateAllInActions(history);
            HandHistoryUtils.CalculateBets(history);
            HandHistoryUtils.CalculateUncalledBets(history, false);
            HandHistoryUtils.CalculateTotalPot(history);
            HandHistoryUtils.RemoveSittingOutPlayers(history);

            if (history.GameDescription.IsTournament)
            {
                history.GameDescription.Tournament.BuyIn.PrizePoolValue /= divider;
                history.GameDescription.Tournament.BuyIn.KnockoutValue /= divider;
                history.GameDescription.Tournament.BuyIn.Rake /= divider;
                history.GameDescription.Tournament.Addon /= divider;
                history.GameDescription.Tournament.Rebuy /= divider;
                history.GameDescription.Tournament.Winning /= divider;
                history.GameDescription.Tournament.Bounty /= divider;
                history.GameDescription.Tournament.TotalPrize /= divider;
            }

            history.HandActions.ForEach(a => a.Amount = a.Amount / divider);

            history.GameDescription.Limit.SmallBlind /= divider;
            history.GameDescription.Limit.BigBlind /= divider;
            history.GameDescription.Limit.Ante /= divider;

            history.Players.ForEach(p =>
            {
                p.Bet /= divider;
                p.StartingStack /= divider;
                p.Win /= divider;
            });

            history.TotalPot /= divider;

            if (!history.GameDescription.IsTournament)
            {
                HandHistoryUtils.CalculateRake(history);
            }
        }

        private void ParsePackage<T>(PPPokerPackage package, Action<T> action)
        {
            if (!SerializationHelper.TryDeserialize(package.Body, out T packageBody))
            {
                //throw new DHInternalException(new NonLocalizableString($"Failed to deserialize package of {package.PackageType} type."));
                throw new Exception($"Failed to deserialize package of {package.PackageType} type.");
            }

            action?.Invoke(packageBody);
        }

        private bool TryParseHandID(string gameId, out long handId)
        {
            DateTime BaseTime = new DateTime(2018, 1, 1) + TimeSpan.FromHours(8); // pppoker.net time is 8 hours ahead of UTC timezone (China Standard Time), no daylight saving time
            const int Base = 100000000;
            const int PartCount = 4;

            handId = 0;

            if (string.IsNullOrWhiteSpace(gameId))
            {
                return false;
            }

            // datetime-session-hand-table
            string[] parts = gameId.Split('-');
            if (parts.Length != PartCount)
            {
                return false;
            }

            if (!DateTime.TryParseExact(parts[0], "yyMMddHHmmss", null, DateTimeStyles.None, out DateTime dt))
            {
                return false;
            }

            long result = Convert.ToInt64((dt - BaseTime).TotalSeconds);

            result *= Base;

            string sessionPlusTable = parts[1] + parts[3];
            result += unchecked((uint)sessionPlusTable.GetHashCode()) % Base;

            if (!int.TryParse(parts[2], out int handNumber))
            {
                return false;
            }

            handId = result + handNumber;

            return true;
        }

        private RoomPlayer UserBriefToRoomPlayer(UserBrief brief)
        {
            return new RoomPlayer {
                ID = brief.Uid,
                Name = brief.Name,
            };
        }

        private void ProcessSelUserInfoRSP(SelUserInfoRSP message, ClientRecord record)
        {
            record.HeroUid = message.Brief.Uid;
        }

        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static DateTime TimestampToDateTime(long timestamp)
        {
            return Epoch.AddSeconds(timestamp);
        }

        private static long DateTimeToTimestamp(DateTime dt)
        {
            return Convert.ToInt64((dt - Epoch).TotalSeconds);
        }

        private void ProcessEnterRoomRSP(EnterRoomRSP message, ClientRecord record)
        {
            if (message.RoomType == RoomType.PineRoom)
            {
                return;
            }

            bool isTournament = TournamentRoomTypes.Contains(message.RoomType);
            IRoomInfo roomInfo = isTournament ? (IRoomInfo)message.SngRoomInfo : (IRoomInfo)message.RoomInfo;

            record.RoomID = roomInfo.RoomID;
            record.TableID = message.TableStatus.Tid;
            record.IsOmaha = message.RoomType == RoomType.OmahaRoom;
            record.RoomName = roomInfo.RoomName.Length > 0 ? roomInfo.RoomName : roomInfo.TempID;
            record.Ante = roomInfo.Ante;
            record.BigBlind = roomInfo.Blind;
            record.MaxPlayers = roomInfo.SeatNum;

            record.IsTournament = isTournament;
            if (isTournament)
            {
                var utcNow = DateTime.UtcNow;

                long startTimestamp = message.RoomType == RoomType.MttRoom ? message.MttRoomInfo.MttStartTime : DateTimeToTimestamp(utcNow);
                record.TournamentID = $"{record.RoomID}-{startTimestamp}";
                record.TournamentName = $"{record.RoomName}";
                record.TournamentBuyIn = message.SngRoomInfo.BuyIn;
                record.TournamentReBuy = message.MttRoomInfo.ReBuyIn;
                record.TournamentAddOn = message.MttRoomInfo.AddOnBuyIn;
                record.TournamentBounty = message.MttRoomInfo.HunterReward;
                record.TournamentHasFixedRewards = message.SngRoomInfo.FixedReward;
                record.TournamentStartDate = message.RoomType == RoomType.MttRoom ? TimestampToDateTime(message.MttRoomInfo.MttStartTime) : utcNow;
            }

            foreach (var seat in message.TableStatus.Seat.Where(s => s.Player != null))
            {
                record.Players[seat.SeatID] = UserBriefToRoomPlayer(seat.Player);
            }
        }

        private void ProcessGetUserMarksREQ(GetUserMarksREQ message, ClientRecord record)
        {
            record.HeroUid = message.Uid;
        }

        private void ProcessGetUserMarksRSP(GetUserMarksRSP message, ClientRecord record)
        {
            record.HeroUid = message.Uid;
        }

        private void ProcessSitDownRSP(SitDownRSP message, ClientRecord record)
        {
            record.HeroUid = record.Players[message.SeatID].ID;
        }

        private void ProcessSitDownBRC(SitDownBRC message, ClientRecord record)
        {
            record.Players[message.SeatID] = UserBriefToRoomPlayer(message.Brief);
        }

        private void ProcessStandUpBRC(StandUpBRC message, ClientRecord record)
        {
            record.Players.Remove(message.SeatID);
        }

        private void ProcessDealerInfoRSP(DealerInfoRSP message, ClientRecord record, HandHistory history)
        {
            if (!TryParseHandID(message.GameID, out long handId))
            {
                //throw new DHInternalException(new NonLocalizableString($"Failed to parse hand id from '{noticeResetGame.GameId}'."));
                throw new Exception($"Failed to parse hand id from '{message.GameID}'.");
            }

            LogProvider.Log.Info($"Parsed GameID = {message.GameID} into HandID = {handId}");

            history.HandId = handId;

            TournamentDescriptor tournament = null;
            if (record.IsTournament)
            {
                const int RakeProportion = 10;

                long rake = record.TournamentHasFixedRewards ? 0 : record.TournamentBuyIn / RakeProportion;
                long prizePoolValue = record.TournamentBuyIn - rake - record.TournamentBounty;

                tournament = new TournamentDescriptor
                {
                    TournamentId = record.TournamentID,
                    TournamentName = record.TournamentName,
                    StartDate = record.TournamentStartDate,
                    BuyIn = Buyin.FromBuyinRake(prizePoolValue, rake, Currency.All, record.TournamentBounty > 0, record.TournamentBounty),
                    Rebuy = record.TournamentReBuy,
                    Addon = record.TournamentAddOn,
                    Speed = TournamentSpeed.Regular, // I didn't discover any reliable way to determine tournament speed from protocol messages
                };
            }

            history.GameDescription = new GameDescriptor(
                EnumPokerSites.PPPoker,
                record.IsOmaha ? GameType.PotLimitOmaha : GameType.NoLimitHoldem,
                Limit.FromSmallBlindBigBlind(record.SmallBlind, record.BigBlind, Currency.All, record.Ante > 0, record.Ante),
                TableType.FromTableTypeDescriptions(record.Ante > 0 ? TableTypeDescription.Ante : TableTypeDescription.Regular),
                SeatType.FromMaxPlayers(record.MaxPlayers),
                tournament
            );

            history.GameDescription.Identifier = record.RoomID;
            history.TableName = $"{record.RoomName}-{record.TableID}";

            foreach (var stackInfo in message.Stacks)
            {
                if (!record.Players.ContainsKey(stackInfo.SeatID))
                {
                    throw new Exception($"Can't find a player sitting on seat #{stackInfo.SeatID} for hand {handId}. Total known players: {record.Players.Count}. Total participating players: {message.Stacks.Length}.");
                }

                var roomPlayer = record.Players[stackInfo.SeatID];

                var player = new Player
                {
                    PlayerName = roomPlayer.ID.ToString(),
                    PlayerNick = roomPlayer.Name,
                    StartingStack = stackInfo.Chips,
                    SeatNumber = stackInfo.SeatID + 1,
                    IsSittingOut = false
                };

                history.Players.Add(player);
            }

            history.DealerButtonPosition = message.Dealer + 1;
        }

        private void ProcessBlindStatusBRC(BlindStatusBRC message, ClientRecord record)
        {
            record.BigBlind = message.Blind;
            record.Ante = message.Ante;
        }

        private void ProcessRoundStartBRC(RoundStartBRC message, ClientRecord record, HandHistory history)
        {
            if (message.Board != null)
            {
                foreach (int card_value in message.Board)
                {
                    history.CommunityCards.Add(ConvertToCard(card_value));
                }
            }
        }

        private void ProcessRoundOverBRC(RoundOverBRC message, ClientRecord record, HandHistory history)
        {
            record.Pots = message.Pool;
        }

        private void ProcessActionBRC(ActionBRC message, ClientRecord record, HandHistory history)
        {
            var playerName = GetPlayerName(history, message.SeatID + 1);

            if (BlindActionMapping.ContainsKey(message.ActionType))
            {
                history.HandActions.Add(new HandAction(
                    playerName,
                    BlindActionMapping[message.ActionType],
                    message.Chips,
                    Street.Preflop
                ));
            }
            else if (ActionMapping.ContainsKey(message.ActionType))
            {
                HandAction action;

                var handActionType = ActionMapping[message.ActionType];
                if (InvestingHandActionTypes.Contains(handActionType) && message.HandChips == 0)
                {
                    action = new AllInAction(
                        playerName,
                        message.Chips,
                        history.CommunityCards.Street,
                        false, // TODO: Find out when IsRaiseAllIn should be true
                        ActionMapping[message.ActionType]
                    );
                }
                else
                {
                    action = new HandAction(
                        playerName,
                        ActionMapping[message.ActionType],
                        message.Chips,
                        history.CommunityCards.Street
                    );
                }

                history.HandActions.Add(action);
            }
        }

        private void ProcessHandCardRSP(HandCardRSP message, ClientRecord record, HandHistory history)
        {
            var roomHero = record.Players.Values.FirstOrDefault(p => p.ID == record.HeroUid);
            if (roomHero == null)
            {
                LogProvider.Log.Warn($"Hero is not seated in hand {history.HandId}, room {record.RoomID}, user {record.HeroUid}");
                return;
            }

            var hero = history.Players.FirstOrDefault(p => p.PlayerName == roomHero.ID.ToString());
            if (hero == null)
            {
                LogProvider.Log.Warn($"Hero not found for hand {history.HandId}, room {record.RoomID}, user {record.HeroUid}");
                return;
            }

            history.Hero = hero;

            hero.HoleCards = HoleCards.FromCards(hero.PlayerName, GetCards(message));
        }

        private void AddShowCardsAction(HandHistory history, Player player, Card[] cards)
        {
            history.HandActions.Add(new HandAction(
                player.PlayerName,
                HandActionType.SHOW,
                0,
                Street.Showdown
            ));

            if (player.hasHoleCards)
            {
                return;
            }

            player.HoleCards = HoleCards.FromCards(player.PlayerName, cards);
        }

        private void ProcessShowHandRSP(ShowHandRSP message, ClientRecord record, HandHistory history)
        {
            foreach (var handInfo in message.Info)
            {
                AddShowCardsAction(history, GetPlayer(history, handInfo.SeatID + 1), GetCards(handInfo));
            }
        }

        private void ProcessWinnerRSP(WinnerRSP message, ClientRecord record, HandHistory history)
        {
            foreach (var winner in message.Winner)
            {
                var player = GetPlayer(history, winner.SeatID + 1);

                // Note that pots are raked therefore it's not correct to use following condition for ties: winning < pot
                // Following condition should be used instead: winning <= pot / 2. Works for raked and not raked pots.
                HandActionType handActionType = winner.Chips <= record.Pots[winner.PoolID] / 2 ? HandActionType.TIES : HandActionType.WINS;

                history.HandActions.Add(new WinningsAction(
                    player.PlayerName,
                    handActionType,
                    winner.Chips,
                    winner.PoolID
                ));

                player.Win += winner.Chips;
            }
        }

        private void ProcessShowMyCardBRC(ShowMyCardBRC message, ClientRecord record, HandHistory history)
        {
            AddShowCardsAction(history, GetPlayer(history, message.SeatID + 1), GetCards(message));
        }

        private void ProcessUserSngOverRSP(UserSngOverRSP message, ClientRecord record, HandHistory history)
        {
            history.GameDescription.Tournament.FinishPosition = message.Rank;
            history.GameDescription.Tournament.TotalPrize = message.Money;
        }

        #region Static helpers

        private static readonly ReadOnlyDictionary<int, int> SuitMapping = new ReadOnlyDictionary<int, int>(new Dictionary<int, int>
        {
            [0] = 1,
            [1] = 0,
            [2] = 2,
            [3] = 3,
        });

        private static Card ConvertToCard(int value)
        {
            const int RankCount = 13;
            // Ad = 0001 0000 1110
            // Ac = 0010 0000 1110
            // Ah = 0011 0000 1110
            // As = 0100 0000 1110

            int rank = (value & 0xF) - 2;
            int suit = ((value & 0xF00) >> 8) - 1;

            int card_number = SuitMapping[suit] * RankCount + rank;

            return Card.GetCardFromIntValue(card_number);
        }

        private static Card[] GetCards(IHoleCardInfo cardInfo)
        {
            var card_values = new List<int>();

            // We have to add hole cards by pairs, otherwise Player.HoleCards will throw an exception
            if (cardInfo.Card1 > 0 && cardInfo.Card2 > 0)
            {
                card_values.Add(cardInfo.Card1);
                card_values.Add(cardInfo.Card2);
            }

            if (cardInfo.Card3 > 0 && cardInfo.Card4 > 0)
            {
                card_values.Add(cardInfo.Card3);
                card_values.Add(cardInfo.Card4);
            }

            return card_values.Select(v => ConvertToCard(v)).ToArray();
        }

        private static string GetPlayerName(HandHistory handHistory, int seat)
        {
            var player = GetPlayer(handHistory, seat);
            return player.PlayerName;
        }

        private static Player GetPlayer(HandHistory handHistory, int seat)
        {
            var player = handHistory.Players.FirstOrDefault(x => x.SeatNumber == seat);

            if (player == null)
            {
                //throw new HandBuilderException(handHistory.HandId, $"Player with seat={seat} not found.");
                throw new Exception($"Player with seat={seat} not found.");
            }

            return player;
        }

        #endregion
    }
}
