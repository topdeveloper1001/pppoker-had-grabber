//-----------------------------------------------------------------------
// <copyright file="TournamentDescriptor.cs" company="Ace Poker Solutions">
// Copyright © 2018 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using PPPokerCardCatcher.Entities;
using System;
using System.Xml.Serialization;

namespace HandHistories.Objects.GameDescription
{
    [Serializable]
    public class TournamentDescriptor
    {
        [XmlElement]
        public string TournamentId { get; set; }

        [XmlElement]
        public string TournamentInGameId { get; set; }

        [XmlElement]
        public string TournamentName { get; set; }

        [XmlElement]
        public string Summary { get; set; }

        [XmlElement]
        public Buyin BuyIn { get; set; }

        [XmlElement]
        public decimal Bounty { get; set; }

        [XmlElement]
        public decimal Rebuy { get; set; }

        [XmlElement]
        public decimal Addon { get; set; }

        [XmlElement]
        public decimal Winning { get; set; }

        [XmlElement]
        public decimal TotalPrize { get; set; }

        [XmlElement]
        public int FinishPosition { get; set; }

        [XmlElement]
        public int TotalPlayers { get; set; }

        [XmlElement]
        public DateTime StartDate { get; set; }

        [XmlElement]
        public TournamentSpeed Speed { get; set; }

        [XmlElement]
        public int StartingStack { get; set; }

        [XmlElement]
        public bool IsSummary
        {
            get
            {
                return !string.IsNullOrEmpty(Summary);
            }
        }

        [XmlElement]
        public TournamentsTags? TournamentsTags { get; set; }
    }
}