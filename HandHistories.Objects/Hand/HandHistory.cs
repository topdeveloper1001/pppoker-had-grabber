//-----------------------------------------------------------------------
// <copyright file="HandHistory.cs" company="Ace Poker Solutions">
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
using HandHistories.Objects.GameDescription;
using HandHistories.Objects.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace HandHistories.Objects.Hand
{
    [Serializable]
    public class HandHistory : HandHistorySummary
    {
        public HandHistory(GameDescriptor gameDescription) : base()
        {
            HandActions = new List<HandAction>();
            Players = new PlayerList();

            CommunityCards = BoardCards.ForPreflop();
            GameDescription = gameDescription;
        }

        public HandHistory() : this(new GameDescriptor())
        {
        }

        [XmlArray(ElementName = "Actions")]
        public List<HandAction> HandActions { get; set; }

        [XmlIgnore]
        public IEnumerable<HandAction> PreFlop
        {
            get { return HandActions.Where(x => x.Street == Street.Preflop); }
        }

        [XmlIgnore]
        public IEnumerable<HandAction> Flop
        {
            get { return HandActions.Where(x => x.Street == Street.Flop); }
        }

        [XmlIgnore]
        public IEnumerable<HandAction> Turn
        {
            get { return HandActions.Where(x => x.Street == Street.Turn); }
        }

        [XmlIgnore]
        public IEnumerable<HandAction> River
        {
            get { return HandActions.Where(x => x.Street == Street.River); }
        }

        [XmlIgnore]
        public IEnumerable<HandAction> Showdown
        {
            get { return HandActions.Where(x => x.Street == Street.Showdown); }
        }

        [XmlIgnore]
        public IEnumerable<HandAction> Summary
        {
            get { return HandActions.Where(x => x.Street == Street.Summary); }
        }

        [XmlIgnore]
        public IEnumerable<HandAction> WinningActions
        {
            get { return HandActions.Where(x => x.IsWinningsAction); }
        }

        [XmlElement("CommunityCards")]
        public string CommunityCardsString
        {
            get { return CommunityCards?.ToString() ?? string.Empty; }
            set
            {
                CommunityCards = BoardCards.FromCards(value);
            }
        }

        [XmlIgnore]
        public BoardCards CommunityCards { get; set; }

        [XmlArray]
        [XmlArrayItem(ElementName = "Player")]
        public PlayerList Players { get; set; }

        private Player hero;

        [XmlIgnore]
        public Player Hero
        {
            get
            {
                return hero;
            }
            set
            {
                if (ReferenceEquals(hero, value))
                {
                    return;
                }

                hero = value;
                HeroName = hero?.PlayerName;
            }
        }

        public string HeroName { get; set; }

        [XmlIgnore]
        public Exception Exception { get; set; }

        [XmlIgnore]
        public bool HasError
        {
            get
            {
                return Exception != null;
            }
        }

        [XmlIgnore]
        public int NumPlayersActive
        {
            get { return Players.Count(p => p.IsSittingOut == false); }
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            HandHistory hand = obj as HandHistory;
            if (hand == null) return false;

            return ToString().Equals(hand.ToString());
        }

        public override string ToString()
        {
            return string.Format("[{0}] {1}", HandId, GameDescription.ToString());
        }       
    }
}