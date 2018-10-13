using System;
using System.Collections.Generic;
using HandHistories.Objects.Actions;
using HandHistories.Objects.Cards;
using HandHistories.Objects.GameDescription;
using HandHistories.Objects.Players;
using System.Xml.Serialization;

namespace HandHistories.Objects.Hand
{
    [Serializable]
    public class HandHistorySummary
    {
        public HandHistorySummary()
        {           
            GameDescription = new GameDescriptor();            
        }

        [XmlElement]
        public DateTime DateOfHandUtc { get; set; }

        [XmlElement]
        public long HandId { get; set; }

        [XmlElement]
        public int DealerButtonPosition { get; set; }

        [XmlElement]
        public string TableName { get; set; }

        [XmlElement]
        public GameDescriptor GameDescription { get; set; }

        [XmlElement]
        public bool Cancelled { get; set; }

        [XmlElement]
        public int NumPlayersSeated { get; set; }

        [XmlIgnore]
        public string FullHandHistoryText { get; set; }

        [XmlElement]
        public decimal? TotalPot { get; set; }

        [XmlElement]
        public decimal? Rake { get; set; }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            HandHistorySummary hand = obj as HandHistorySummary;
            if (hand == null) return false;

            return ToString().Equals(hand.ToString());
        }
           
        public override string ToString()
        {
            return string.Format("[{0}] {1}", HandId, GameDescription.ToString());
        }
    }
}
