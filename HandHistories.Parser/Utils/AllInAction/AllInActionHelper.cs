//-----------------------------------------------------------------------
// <copyright file="AllInActionHelper.cs" company="Ace Poker Solutions">
// Copyright © 2015 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using DriveHUD.Common.Log;
using HandHistories.Objects.Actions;
using HandHistories.Objects.Cards;
using HandHistories.Objects.Players;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HandHistories.Parser.Utils
{
    internal static class AllInActionHelper
    {
        /// <summary>
        /// Some sites (like IPoker) don't specifically identify All-In calls/raises. In these cases we need to parse the actions 
        /// and reclassify certain actions as all-in
        /// </summary>
        internal static List<HandAction> IdentifyAllInActions(PlayerList playerList, List<HandAction> handActions)
        {
            Dictionary<string, decimal> playerStackRemaining = new Dictionary<string, decimal>();

            foreach (Player player in playerList)
            {
                if (!playerStackRemaining.ContainsKey(player.PlayerName))
                {
                    playerStackRemaining.Add(player.PlayerName, player.StartingStack);
                }
                else
                {
                    LogProvider.Log.Warn($"Identifying all in actions: Player [{player.PlayerName}] has been added already.");
                }
            }

            List<HandAction> identifiedActions = new List<HandAction>(handActions.Count);

            foreach (HandAction action in handActions)
            {
                // Negative amounts represent putting money into the pot - ignore actions which aren't negative
                if (action.Amount >= 0)
                {
                    identifiedActions.Add(action);
                    continue;
                }

                // Skip actions which have already been identified
                if (action is AllInAction)
                {
                    identifiedActions.Add(action);
                    continue;
                }

                // Update the remaining stack with our action's amount
                playerStackRemaining[action.PlayerName] += action.Amount;

                if (playerStackRemaining[action.PlayerName] == 0)
                {
                    if (action.IsBlinds)
                    {
                        action.IsAllIn = true;
                        identifiedActions.Add(action);
                        continue;
                    }

                    // This was a bet/raise/call for our remaining chips - we are all in
                    var allInAction = new AllInAction(action.PlayerName,
                        action.Amount,
                        action.Street,
                        action.HandActionType == HandActionType.RAISE || action.HandActionType == HandActionType.BET,
                        action.HandActionType);

                    identifiedActions.Add(allInAction);
                }
                else
                {
                    identifiedActions.Add(action);
                }
            }

            return identifiedActions;
        }

        /// <summary>
        /// Some sites (like IPoker) dont specify if allins are CALL/BET/RAISE so we we fix that after parsing actions.
        /// We do that by assigning HandAction.HandActionType with HandActionType.ALL_IN
        /// </summary>
        internal static List<HandAction> UpdateAllInActions(List<HandAction> handActions)
        {
            List<HandAction> identifiedActions = new List<HandAction>(handActions.Count);

            foreach (HandAction action in handActions)
            {
                if (action is AllInAction)
                {
                    var actionType = GetAllInActionType(action.PlayerName, action.Amount, action.Street, identifiedActions);

                    identifiedActions.Add(new AllInAction(action.PlayerName,
                        action.Amount,
                        action.Street,
                        actionType == HandActionType.RAISE || actionType == HandActionType.BET,
                        actionType));
                }
                else
                {
                    identifiedActions.Add(action);
                }
            }

            return identifiedActions;
        }

        /// <summary>
        /// Gets the ActionType for an unadjusted action amount
        /// </summary>
        /// <param name="playerName"></param>
        /// <param name="amount"></param>
        /// <param name="street"></param>
        /// <param name="actions"></param>
        /// <returns></returns>
        internal static HandActionType GetAllInActionType(string playerName, decimal amount, Street street, List<HandAction> actions)
        {
            try
            {
                var streetActions = actions.Street(street);

                if (street != Street.Preflop && 
                    streetActions.FirstOrDefault(p => p.HandActionType == HandActionType.BET || 
                        (p is AllInAction allInAction) && allInAction.SourceActionType == HandActionType.BET) == null)
                {
                    return HandActionType.BET;
                }

                var putInPot = new Dictionary<string, decimal>();

                foreach (var action in streetActions)
                {
                    if (!putInPot.ContainsKey(action.PlayerName))
                    {
                        putInPot.Add(action.PlayerName, action.Amount);
                    }
                    else
                    {
                        putInPot[action.PlayerName] += action.Amount;
                    }
                }

                var contributed = Math.Abs(amount);

                if (putInPot.ContainsKey(playerName))
                {
                    contributed += Math.Abs(putInPot[playerName]);
                }

                if (contributed <= Math.Abs(putInPot.Min(p => p.Value)))
                {
                    return HandActionType.CALL;
                }
                else
                {
                    return HandActionType.RAISE;
                }
            }
            catch
            {                
                return HandActionType.BET;
            }
        }
    }
}