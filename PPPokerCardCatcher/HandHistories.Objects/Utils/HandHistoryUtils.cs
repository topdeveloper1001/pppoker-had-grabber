//-----------------------------------------------------------------------
// <copyright file="HandHistoryUtils.cs" company="Ace Poker Solutions">
// Copyright © 2018 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using HandHistories.Objects.Actions;
using HandHistories.Objects.Cards;
using HandHistories.Objects.Hand;
using HandHistories.Objects.Players;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HandHistories.Parser.Utils
{
    public class HandHistoryUtils
    {
        public static void AddShowActions(HandHistory handHistory)
        {
            // add show
            var playersCouldSeeShowdown = (from handAction in handHistory.HandActions
                                           group handAction by handAction.PlayerName into grouped
                                           let playerFolded = grouped.Any(x => x.IsFold)
                                           where !playerFolded
                                           select grouped.Key).ToArray();

            if (playersCouldSeeShowdown.Length > 1)
            {
                foreach (var playerCouldSeeShowdown in playersCouldSeeShowdown)
                {
                    var showdownAction = new HandAction(playerCouldSeeShowdown, HandActionType.SHOW, 0, Street.Showdown);
                    handHistory.HandActions.Add(showdownAction);
                }
            }
        }

        public static void AddWinningActions(HandHistory handHistory)
        {
            foreach (var winner in handHistory.Players.Where(p => p.Win > 0))
            {
                var winAction = new WinningsAction(winner.PlayerName, HandActionType.WINS, winner.Win, 0);
                handHistory.HandActions.Add(winAction);
            }
        }

        public static void CalculateBets(HandHistory handHistory)
        {
            var betsByPlayer = handHistory.HandActions
                .GroupBy(p => p.PlayerName)
                .Select(x => new { PlayerName = x.Key, Bet = x.Where(a => a.Amount < 0).Sum(a => a.Amount) })
                .ToDictionary(x => x.PlayerName, x => Math.Abs(x.Bet));

            foreach (var player in handHistory.Players)
            {
                if (betsByPlayer.ContainsKey(player.PlayerName))
                {
                    player.Bet = betsByPlayer[player.PlayerName];
                }
            }
        }

        public static void CalculateTotalPot(HandHistory handHistory)
        {
            handHistory.TotalPot = Math.Abs(handHistory.HandActions.Where(x => !x.IsWinningsAction).Sum(x => x.Amount));
        }

        public static void RemoveSittingOutPlayers(HandHistory handHistory)
        {
            foreach (var player in handHistory.Players.ToArray())
            {
                if (handHistory.HandActions.Any(x => x.PlayerName == player.PlayerName))
                {
                    continue;
                }

                handHistory.Players.Remove(player);

                if (handHistory.Hero == player)
                {
                    handHistory.Hero = null;
                }
            }
        }

        public static void UpdateAllInActions(HandHistory handHistory)
        {
            AllInActionHelper.IdentifyAllInActions(handHistory.Players, handHistory.HandActions);
            handHistory.HandActions = AllInActionHelper.UpdateAllInActions(handHistory.HandActions);
        }

        /// <summary>
        /// Adds uncalled bets to the hand history and adjust winnings
        /// </summary>
        /// <param name="handHistory">Hand history to update</param>
        /// <param name="winsIncludeUncalledBet">Determines whenvers winnings include uncalled bet</param>
        public static void CalculateUncalledBets(HandHistory handHistory, bool winsIncludeUncalledBet)
        {
            if (handHistory == null || handHistory.HandActions.Count == 0)
            {
                return;
            }

            var playersPutInPot = new Dictionary<string, decimal>();

            foreach (var action in handHistory.HandActions
                .Where(x => x.IsGameAction && x.Street != Street.Showdown && x.Street != Street.Summary))
            {
                if (!playersPutInPot.ContainsKey(action.PlayerName))
                {
                    playersPutInPot.Add(action.PlayerName, 0);
                }

                playersPutInPot[action.PlayerName] += Math.Abs(action.Amount);
            }

            var playerPutMaxInPot = new KeyValuePair<string, decimal>();
            var playerPutSecondMaxInPot = new KeyValuePair<string, decimal>();

            foreach (KeyValuePair<string, decimal> playerPutInPot in playersPutInPot)
            {
                if (playerPutInPot.Value > playerPutMaxInPot.Value)
                {
                    playerPutSecondMaxInPot = playerPutMaxInPot;
                    playerPutMaxInPot = playerPutInPot;
                }
                else
                {
                    if (playerPutInPot.Value > playerPutSecondMaxInPot.Value)
                    {
                        playerPutSecondMaxInPot = playerPutInPot;
                    }
                }
            }

            var diffBetweenPots = playerPutMaxInPot.Value - playerPutSecondMaxInPot.Value;

            if (diffBetweenPots <= 0)
            {
                return;
            }

            var totalWinnings = handHistory.WinningActions.Sum(x => x.Amount);
            var totalPot = playersPutInPot.Sum(x => x.Value) - diffBetweenPots;

            // we don't know if winning commands includes uncalled bet or not, but if winning are greater than all players put into the pot 
            // then we know what uncalled bet is in winnings
            if (totalWinnings > totalPot || winsIncludeUncalledBet)
            {
                var winAction = handHistory.HandActions
                    .FirstOrDefault(x => x.HandActionType == HandActionType.WINS && x.PlayerName == playerPutMaxInPot.Key);

                if (winAction != null)
                {
                    winAction.Amount -= diffBetweenPots;

                    if (handHistory.Players[playerPutMaxInPot.Key].Win > 0)
                    {
                        handHistory.Players[playerPutMaxInPot.Key].Win -= diffBetweenPots;
                    }

                    if (winAction.Amount == 0)
                    {
                        handHistory.HandActions.Remove(winAction);
                    }
                }
            }

            var lastRaiseAction = handHistory.HandActions.LastOrDefault(x => x.PlayerName == playerPutMaxInPot.Key && x.IsGameAction);

            if (lastRaiseAction == null)
            {
                throw new InvalidOperationException("Last raise has not been found.");
            }

            // folded players can't get uncalled bet
            if (lastRaiseAction.IsFold)
            {
                return;
            }

            var uncalledBet = new HandAction(playerPutMaxInPot.Key, HandActionType.UNCALLED_BET, diffBetweenPots, lastRaiseAction.Street);

            var lastStreeAction = handHistory.HandActions.Street(lastRaiseAction.Street).LastOrDefault();
            var indexOfLastStreetAction = handHistory.HandActions.IndexOf(lastStreeAction);

            handHistory.HandActions.Insert(indexOfLastStreetAction + 1, uncalledBet);
        }

        public static void SortHandActions(HandHistory handHistory)
        {
            var dealer = handHistory.DealerButtonPosition;

            var orderedPlayers = handHistory
                .Players
                .Skip(dealer)
                .Concat(handHistory.Players.TakeWhile(x => x.SeatNumber <= dealer))
                .ToArray();

            var orderedPlayersDictionary = OrderedPlayersToDict(orderedPlayers);

            var orderedHandActions = new List<HandAction>();

            if (orderedPlayersDictionary.Count == 2)
            {
                var preflopOrderedPlayersDictionary = OrderedPlayersToDict(orderedPlayers.Reverse());
                orderedHandActions.AddRange(OrderStreetHandActions(handHistory.HandActions, preflopOrderedPlayersDictionary, x => x.Street == Street.Preflop));
            }
            else
            {
                var blindsCount = handHistory.PreFlop
                    .Count(x => x.HandActionType == HandActionType.SMALL_BLIND || x.HandActionType == HandActionType.BIG_BLIND || x.HandActionType == HandActionType.STRADDLE);

                var preflopOrderedPlayers = orderedPlayers.Skip(blindsCount).Concat(orderedPlayers.Take(blindsCount)).ToArray();
                var preflopOrderedPlayersDictionary = OrderedPlayersToDict(preflopOrderedPlayers);

                orderedHandActions.AddRange(OrderStreetHandActions(handHistory.HandActions, orderedPlayersDictionary, x => x.Street == Street.Preflop && x.IsBlinds));
                orderedHandActions.AddRange(OrderStreetHandActions(handHistory.HandActions, preflopOrderedPlayersDictionary, x => x.Street == Street.Preflop && !x.IsBlinds));
            }

            orderedHandActions.AddRange(OrderStreetHandActions(handHistory.HandActions, orderedPlayersDictionary, x => x.Street == Street.Flop));
            orderedHandActions.AddRange(OrderStreetHandActions(handHistory.HandActions, orderedPlayersDictionary, x => x.Street == Street.Turn));
            orderedHandActions.AddRange(OrderStreetHandActions(handHistory.HandActions, orderedPlayersDictionary, x => x.Street == Street.River));
            orderedHandActions.AddRange(OrderStreetHandActions(handHistory.HandActions, orderedPlayersDictionary, x => x.Street == Street.Showdown));
            orderedHandActions.AddRange(OrderStreetHandActions(handHistory.HandActions, orderedPlayersDictionary, x => x.Street == Street.Summary));

            handHistory.HandActions = orderedHandActions;
        }

        public static void CalculateRake(HandHistory history)
        {
            history.Rake = PotCalculator.CalculateRake(history);
        }

        private static Dictionary<string, int> OrderedPlayersToDict(IEnumerable<Player> orderedPlayers)
        {
            var orderedPlayersDictionary = orderedPlayers
               .Select((x, index) => new { Index = index, x.PlayerName })
               .GroupBy(x => x.PlayerName)
               .ToDictionary(x => x.Key, x => x.FirstOrDefault().Index);

            return orderedPlayersDictionary;
        }

        private static List<HandAction> OrderStreetHandActions(List<HandAction> handActions, Dictionary<string, int> orderedPlayersDictionary, Func<HandAction, bool> filter)
        {
            var streetActions = handActions.Where(filter).ToList();

            if (streetActions.Count == 0)
            {
                return streetActions;
            }

            var orderedHandActions = new List<HandAction>();

            var isInvalid = false;

            var groupedStreetActions = streetActions
                .GroupBy(x => x.PlayerName)
                .Select(x => new { PlayerName = x.Key, PlayerActions = x.ToList() })
                .OrderBy(x =>
                {
                    if (!orderedPlayersDictionary.ContainsKey(x.PlayerName))
                    {
                        isInvalid = true;
                        return int.MaxValue;
                    }

                    return orderedPlayersDictionary[x.PlayerName];
                })
                .ToList();

            if (isInvalid)
            {
                return streetActions;
            }

            while (orderedHandActions.Count != streetActions.Count)
            {
                foreach (var groupedAction in groupedStreetActions.ToArray())
                {
                    var playerAction = groupedAction.PlayerActions.FirstOrDefault();

                    if (playerAction != null)
                    {
                        orderedHandActions.Add(playerAction);
                        groupedAction.PlayerActions.Remove(playerAction);
                    }
                    else
                    {
                        groupedStreetActions.Remove(groupedAction);
                    }
                }
            }

            return orderedHandActions;
        }
    }
}