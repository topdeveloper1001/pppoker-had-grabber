//-----------------------------------------------------------------------
// <copyright file="AssertionUtils.cs" company="Ace Poker Solutions">
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
using NUnit.Framework;
using System.Linq;

namespace PPPokerCardCatcher.Tests
{
    internal sealed class AssertionUtils
    {
        public static void AssertHandHistory(HandHistory actual, HandHistory expected)
        {
            Assert.Multiple(() =>
            {
                Assert.That(actual.DateOfHandUtc, Is.EqualTo(expected.DateOfHandUtc), "DateOfHandUtc must be equal");
                Assert.That(actual.HandId, Is.EqualTo(expected.HandId), "HandId must be equal");
                Assert.That(actual.DealerButtonPosition, Is.EqualTo(expected.DealerButtonPosition), "DealerButtonPosition must be equal");
                Assert.That(actual.TableName, Is.EqualTo(expected.TableName), "TableName must be equal");
                Assert.That(actual.GameDescription.PokerFormat, Is.EqualTo(expected.GameDescription.PokerFormat), "GameDescription.PokerFormat must be equal");
                Assert.That(actual.GameDescription.Site, Is.EqualTo(expected.GameDescription.Site), "GameDescription.Site must be equal");
                Assert.That(actual.GameDescription.GameType, Is.EqualTo(expected.GameDescription.GameType), "GameDescription.GameType must be equal");
                Assert.That(actual.GameDescription.Limit, Is.EqualTo(expected.GameDescription.Limit), "GameDescription.Limit must be equal");
                Assert.That(actual.GameDescription.SeatType, Is.EqualTo(expected.GameDescription.SeatType), "GameDescription.SeatType must be equal");
                Assert.That(actual.TotalPot, Is.EqualTo(expected.TotalPot), "TotalPot must be equal");
                Assert.That(actual.Rake, Is.EqualTo(expected.Rake), "Rake must be equal");
                Assert.That(actual.CommunityCards, Is.EqualTo(expected.CommunityCards), "CommunityCards must be equal");
                Assert.That(actual.HeroName, Is.EqualTo(expected.HeroName), "HeroName must be equal");

                Assert.That(actual.GameDescription.GameType, Is.EqualTo(expected.GameDescription.GameType), "GameDescription.GameType must be equal");
                Assert.That(actual.GameDescription.PokerFormat, Is.EqualTo(expected.GameDescription.PokerFormat), "GameDescription.PokerFormat must be equal");
                Assert.That(actual.GameDescription.Site, Is.EqualTo(expected.GameDescription.Site), "GameDescription.Site must be equal");
                Assert.That(actual.GameDescription.IsStraddle, Is.EqualTo(expected.GameDescription.IsStraddle), "GameDescription.IsStraddle must be equal");
                Assert.That(actual.GameDescription.SeatType.MaxPlayers, Is.EqualTo(expected.GameDescription.SeatType.MaxPlayers), "GameDescription.SeatType.MaxPlayers must be equal");
                Assert.That(actual.GameDescription.Limit.Ante, Is.EqualTo(expected.GameDescription.Limit.Ante), "GameDescription.Limit.Ante must be equal");
                Assert.That(actual.GameDescription.Limit.BigBlind, Is.EqualTo(expected.GameDescription.Limit.BigBlind), "GameDescription.Limit.BigBlind must be equal");
                Assert.That(actual.GameDescription.Limit.Currency, Is.EqualTo(expected.GameDescription.Limit.Currency), "GameDescription.Limit.Currency must be equal");
                Assert.That(actual.GameDescription.Limit.IsAnteTable, Is.EqualTo(expected.GameDescription.Limit.IsAnteTable), "GameDescription.Limit.IsAnteTable must be equal");
                Assert.That(actual.GameDescription.Limit.SmallBlind, Is.EqualTo(expected.GameDescription.Limit.SmallBlind), "GameDescription.Limit.SmallBlind must be equal");
                CollectionAssert.AreEquivalent(actual.GameDescription.TableType, expected.GameDescription.TableType, "GameDescription.TableType must be equivalent");
                CollectionAssert.AreEquivalent(actual.GameDescription.TableTypeDescriptors, expected.GameDescription.TableTypeDescriptors, "GameDescription.TableTypeDescriptors must be equivalent");
                Assert.That(actual.GameDescription.IsTournament, Is.EqualTo(expected.GameDescription.IsTournament), "GameDescription.IsTournament must be equal");
                Assert.That(actual.GameDescription.Identifier, Is.EqualTo(expected.GameDescription.Identifier), "GameDescription.Identifier must be equal");

                if (actual.GameDescription.IsTournament)
                {                    
                    Assert.That(actual.GameDescription.Tournament.TournamentId, Is.EqualTo(expected.GameDescription.Tournament.TournamentId), "GameDescription.Tournament.TournamentId must be equal");
                    Assert.That(actual.GameDescription.Tournament.TournamentInGameId, Is.EqualTo(expected.GameDescription.Tournament.TournamentInGameId), "GameDescription.Tournament.TournamentInGameId must be equal");
                    Assert.That(actual.GameDescription.Tournament.TournamentName, Is.EqualTo(expected.GameDescription.Tournament.TournamentName), "GameDescription.Tournament.TournamentName must be equal");
                    Assert.That(actual.GameDescription.Tournament.BuyIn, Is.EqualTo(expected.GameDescription.Tournament.BuyIn), "GameDescription.Tournament.BuyIn must be equal");
                    Assert.That(actual.GameDescription.Tournament.Bounty, Is.EqualTo(expected.GameDescription.Tournament.Bounty), "GameDescription.Tournament.Bounty must be equal");
                    Assert.That(actual.GameDescription.Tournament.Rebuy, Is.EqualTo(expected.GameDescription.Tournament.Rebuy), "GameDescription.Tournament.Rebuy must be equal");
                    Assert.That(actual.GameDescription.Tournament.Addon, Is.EqualTo(expected.GameDescription.Tournament.Addon), "GameDescription.Tournament.Addon must be equal");
                    Assert.That(actual.GameDescription.Tournament.Winning, Is.EqualTo(expected.GameDescription.Tournament.Winning), "GameDescription.Tournament.Winning must be equal");
                    Assert.That(actual.GameDescription.Tournament.FinishPosition, Is.EqualTo(expected.GameDescription.Tournament.FinishPosition), "GameDescription.Tournament.FinishPosition must be equal");
                    Assert.That(actual.GameDescription.Tournament.TotalPlayers, Is.EqualTo(expected.GameDescription.Tournament.TotalPlayers), "GameDescription.Tournament.TotalPlayers must be equal");
                    Assert.That(actual.GameDescription.Tournament.StartDate, Is.EqualTo(expected.GameDescription.Tournament.StartDate), "GameDescription.Tournament.StartDate must be equal");
                    Assert.That(actual.GameDescription.Tournament.Speed, Is.EqualTo(expected.GameDescription.Tournament.Speed), "GameDescription.Tournament.Speed must be equal");
                }

                Assert.That(actual.Players.Count, Is.EqualTo(expected.Players.Count), "Players.Count must be equal");

                actual.Players.SortList();
                expected.Players.SortList();

                for (var i = 0; i < actual.Players.Count; i++)
                {
                    Assert.That(actual.Players[i].PlayerName, Is.EqualTo(expected.Players[i].PlayerName), $"Player.PlayerName must be equal [{expected.Players[i].PlayerName}]");
                    Assert.That(actual.Players[i].PlayerNick, Is.EqualTo(expected.Players[i].PlayerNick), $"Player.PlayerNick must be equal [{expected.Players[i].PlayerNick}]");
                    Assert.That(actual.Players[i].Bet, Is.EqualTo(expected.Players[i].Bet), $"Player.Bet must be equal [{expected.Players[i].PlayerName}]");
                    Assert.That(actual.Players[i].Cards, Is.EqualTo(expected.Players[i].Cards), $"Player.Cards must be equal [{expected.Players[i].PlayerName}]");
                    Assert.That(actual.Players[i].SeatNumber, Is.EqualTo(expected.Players[i].SeatNumber), $"Player.SeatNumber must be equal [{expected.Players[i].PlayerName}]");
                    Assert.That(actual.Players[i].StartingStack, Is.EqualTo(expected.Players[i].StartingStack), $"Player.StartingStack must be equal [{expected.Players[i].PlayerName}]");
                    Assert.That(actual.Players[i].Win, Is.EqualTo(expected.Players[i].Win), $"Player.Win must be equal []");
                }

                Assert.That(actual.HandActions, Is.EqualTo(expected.HandActions), "HandActions.Count must be equal");

                var actualActions = actual.HandActions.OrderBy(x => x.ActionNumber).ToArray();
                var expectedActions = expected.HandActions.OrderBy(x => x.ActionNumber).ToArray();

                for (var i = 0; i < actualActions.Length; i++)
                {
                    Assert.That(actualActions[i].ActionNumber, Is.EqualTo(expectedActions[i].ActionNumber), "HandActions.ActionNumber must be equal");
                    Assert.That(actualActions[i].Amount, Is.EqualTo(expectedActions[i].Amount), "HandActions.Amount must be equal");
                    Assert.That(actualActions[i].HandActionType, Is.EqualTo(expectedActions[i].HandActionType), "HandActions.HandActionType must be equal");
                    Assert.That(actualActions[i].PlayerName, Is.EqualTo(expectedActions[i].PlayerName), "HandActions.PlayerName must be equal");
                    Assert.That(actualActions[i].Street, Is.EqualTo(expectedActions[i].Street), "HandActions.Street must be equal");
                }
            });
        }
    }
}