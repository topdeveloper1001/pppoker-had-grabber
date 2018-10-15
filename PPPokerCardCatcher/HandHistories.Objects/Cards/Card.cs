//-----------------------------------------------------------------------
// <copyright file="Card.cs" company="Ace Poker Solutions">
// Copyright © 2015 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace HandHistories.Objects.Cards
{
    // When Card is a struct it only allocates 1 byte on the stack instead of 4 Reference bytes and two strings on the heap
    // Combined with lookup tables and using enums we get a 20x speedup of parsing cards
    [Serializable]
    public partial struct Card
    {
        const int SuitCardMask = 0xF0;
        const int RankCardMask = 0x0F;

        #region Properties
        private SuitEnum suit
        {
            get
            {
                return (SuitEnum)((int)_card & SuitCardMask);
            }
        }

        private CardValueEnum rank
        {
            get
            {
                return (CardValueEnum)((int)_card & RankCardMask);
            }
        }

        [XmlIgnore]
        public string Rank
        {
            get
            {
                int rank = (int)_card & RankCardMask;
                return ((CardValueEnum)rank).ToString().Substring(1);
            }
        }

        [XmlIgnore]
        public int RankNumericValue
        {
            get
            {
                return (int)_card & RankCardMask;
            }
        }

        [XmlElement]
        public string Suit
        {
            get
            {
                int suit = (int)_card & SuitCardMask;
                return ((SuitEnum)suit).ToString().Remove(1).ToLower();
            }
        }

        [XmlElement]
        public string CardStringValue
        {
            get { return _card.ToString().Substring(1); }
        }

        [XmlIgnore]
        public bool isEmpty
        {
            get
            {
                return _card == CardEnum.Unknown;
            }
        }
        #endregion

        private CardEnum _card;

        [XmlAttribute(AttributeName = "Card")]
        public CardEnum _Card
        {
            get { return _card; }
            set { _card = value; }
        }

        #region Constructors

        public Card(char rank, char suit)
        {
            CardValueEnum _rank = rankParseLookup[rank];

            SuitEnum _suit = suitParseLookup[suit];

            _card = (CardEnum)((int)_suit + (int)_rank);

            if (_suit == SuitEnum.Unknown || _rank == CardValueEnum.Unknown)
            {
                throw new ArgumentException("Hand is not correctly formatted. Value: " + rank + " Suit: " + suit);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="rank">Rank should be 2-9,T,J,Q,K,A.</param>
        /// <param name="suit">Suit should be c,d,h,s.</param>
        public Card(string rank, string suit) : this(rank[0], suit[0])
        {
        }

        private Card(SuitEnum suit, CardValueEnum rank)
        {
            _card = (CardEnum)((int)suit + (int)rank);
        }
        #endregion

        #region Operators

        public static bool operator ==(Card c1, Card c2)
        {
            return c1._card == c2._card;
        }

        public static bool operator !=(Card c1, Card c2)
        {
            return c1._card != c2._card;
        }

        public static explicit operator Card(string card)
        {
            return Parse(card);
        }

        #endregion

        [XmlIgnore]
        public static string[] PossibleRanksHighCardFirst
        {
            get
            {
                return new string[]
                       {
                           "A",
                           "K",
                           "Q",
                           "J",
                           "T",
                           "9",
                           "8",
                           "7",
                           "6",
                           "5",
                           "4",
                           "3",
                           "2"
                       };
            }
        }

        public static string[] GetCardRanges()
        {
            var cardsRanks = PossibleRanksHighCardFirst;

            var cardRanges = new string[cardsRanks.Length * cardsRanks.Length];

            var sb = new StringBuilder();

            for (var i = 0; i < cardsRanks.Length; i++)
            {
                for (var j = 0; j < cardsRanks.Length; j++)
                {
                    if (j >= i)
                    {
                        sb.Append(cardsRanks[i]);
                        sb.Append(cardsRanks[j]);

                        if (j > i)
                        {
                            sb.Append("s");
                        }
                    }
                    else if (j < i)
                    {
                        sb.Append(cardsRanks[j]);
                        sb.Append(cardsRanks[i]);
                        sb.Append("o");
                    }

                    cardRanges[j + i * cardsRanks.Length] = sb.ToString();

                    sb.Clear();
                }
            }

            return cardRanges;
        }

        public static Card GetCardFromIntValue(int value)
        {
            //Sanity check
            if (value >= 52 || value <= -1)
            {
                //Because card is a struct we cant return null, 
                //however there is a property isEmpty that is true when this method fails
                return new Card();
            }

            var suit = (int)(value / 13);
            var rank = value % 13;

            //suit starts at zero and SuitEnum starts at 1
            SuitEnum suitValue = (SuitEnum)((suit + 1) << 4);

            //rank starts at zero and CardValueEnum starts at 2
            CardValueEnum rankValue = (CardValueEnum)rank + 2;
            return new Card(suitValue, rankValue);
        }

        public static Card GetPMCardFromIntValue(int value)
        {
            if (value >= 52 || value < 0)
            {
                return new Card();
            }

            var suit = value / 13;
            var rank = value % 13;

            var suitPMValue = ((suit + 1) << 4);

            SuitEnum suitValue;           

            if (suitPMValue == 0x10)
            {
                suitValue = SuitEnum.Spades;
            }
            else if (suitPMValue == 0x20)
            {
                suitValue = SuitEnum.Hearts;
            }
            else if (suitPMValue == 0x30)
            {
                suitValue = SuitEnum.Clubs;
            }
            else if (suitPMValue == 0x40)
            {
                suitValue = SuitEnum.Diamonds;
            }
            else
            {
                suitValue = SuitEnum.Unknown;
            }

            var rankPMValue = rank + 1;

            var rankValue = rankPMValue == 1 ? CardValueEnum._A : (CardValueEnum)rankPMValue;

            return new Card(suitValue, rankValue);
        }

        public static Card Parse(string card)
        {
            if (card.Length != 2)
            {
                throw new ArgumentException("Cards must be length 2. Format Rs where R is rank and s is suit.");
            }

            return new Card(card[0], card[1]);
        }

        /// <summary>
        /// 2c = 0, 3c = 1, ..., Ac = 12, ..., As = 51. Returns -1 if there was an error with the rank or suit values.
        /// </summary>
        [XmlIgnore]
        public int CardIntValue
        {
            get
            {
                int rankValue = (int)rank;
                int suitValue = ((int)suit >> 4) - 1;

                // note minus 1 is so we can index by 0
                return (suitValue * 13) + rankValue - 2;
            }
        }

        public static string GetMaximumRank(string rank1, string rank2)
        {
            return GetRankNumericValue(rank1) > GetRankNumericValue(rank2) ? rank1 : rank2;
        }

        public static string GetMinimumRank(string rank1, string rank2)
        {
            return GetRankNumericValue(rank1) < GetRankNumericValue(rank2) ? rank1 : rank2;
        }

        public static int GetRankNumericValue(string rank1)
        {
            return (int)rankParseLookup[rank1[0]];
        }

        public bool IsValid()
        {
            return rank != CardValueEnum.Unknown && suit != SuitEnum.Unknown;
        }

        public override string ToString()
        {
            return CardStringValue;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return obj.ToString().Equals(ToString());
        }

        public override int GetHashCode()
        {
            return _card.GetHashCode();
        }
    }
}