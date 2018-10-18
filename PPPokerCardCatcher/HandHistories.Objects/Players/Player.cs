using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using HandHistories.Objects.Cards;
using System.Xml.Serialization;

namespace HandHistories.Objects.Players
{
    [Serializable]
    public class Player
    {
        [DataMember]
        [XmlIgnore]
        public int PlayerId { get; set; }

        [XmlAttribute]
        public string PlayerName { get; set; }

        [XmlAttribute]
        public string PlayerNick { get; set; }

        [XmlAttribute]
        public decimal StartingStack { get; set; }

        [XmlAttribute]
        public decimal Bet { get; set; }

        [XmlAttribute]
        public decimal Win { get; set; }

        [XmlAttribute]
        public int SeatNumber { get; set; }

        /// <summary>
        /// Hole cards will be null when there are no cards,
        /// use "hasHoleCards" to find out if there are any cards
        /// </summary>
        [XmlIgnore]
        public HoleCards HoleCards { get; set; }

        [XmlAttribute]
        public string Cards
        {
            get { return HoleCards?.ToString() ?? string.Empty; }
            set
            {
                HoleCards = HoleCards.FromCards(PlayerName, value);
            }
        }

        [XmlAttribute]
        public bool IsSittingOut { get; set; }

        public Player()
        {
        }

        public Player(string playerName,
                      decimal startingStack,
                      int seatNumber)
        {
            PlayerName = playerName;
            StartingStack = startingStack;
            SeatNumber = seatNumber;
            HoleCards = null;
        }

        [XmlIgnore]
        public bool hasHoleCards
        {
            get
            {
                return HoleCards != null && HoleCards.Count > 0;
            }
        }

        [XmlIgnore]
        public bool IsLost
        {
            get
            {
                return StartingStack == Bet && Win == 0;
            }
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return obj.ToString().Equals(ToString());
        }

        public override string ToString()
        {
            string s = string.Format("Seat {0}: {1} {2} [{3}] with '{4}'", SeatNumber, PlayerName, PlayerNick, StartingStack.ToString("N2"), (hasHoleCards ? HoleCards.ToString() : ""));

            if (IsSittingOut)
            {
                s = "[Sitting Out] " + s;
            }

            return s;
        }
    }
}
