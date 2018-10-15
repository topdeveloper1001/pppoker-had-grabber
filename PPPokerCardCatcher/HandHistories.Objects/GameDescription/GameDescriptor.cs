using PPPokerCardCatcher.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

namespace HandHistories.Objects.GameDescription
{
    [Serializable]
    public class GameDescriptor
    {
        public GameDescriptor() : this(PokerFormat.Unknown,
                                       EnumPokerSites.Unknown,
                                       GameType.Unknown,
                                       null,
                                       TableType.FromTableTypeDescriptions(),
                                       SeatType.AllSeatType(),
                                       null)
        {
        }

        public GameDescriptor(EnumPokerSites siteName,
                              GameType gameType,
                              Limit limit,
                              TableType tableType,
                              SeatType seatType, TournamentDescriptor tournament)
            : this(PokerFormat.CashGame, siteName, gameType, limit, tableType, seatType, tournament)
        {
        }

        public GameDescriptor(PokerFormat pokerFormat,
                              EnumPokerSites siteName,
                              GameType gameType,
                              Limit limit,
                              TableType tableType,
                              SeatType seatType,
                              TournamentDescriptor tournament)
        {
            PokerFormat = pokerFormat;
            Site = siteName;
            GameType = gameType;
            Limit = limit;
            TableType = tableType;
            SeatType = seatType;
            Tournament = tournament;
        }

        [XmlElement]
        public PokerFormat PokerFormat { get; set; }

        [XmlElement]
        public EnumPokerSites Site { get; set; }

        [XmlElement]
        public GameType GameType { get; set; }

        [XmlElement]
        public Limit Limit { get; set; }

        [XmlElement]
        public SeatType SeatType { get; set; }

        [XmlElement(ElementName = "TableType")]
        public string TableTypeText
        {
            get
            {
                return TableType.ToString();
            }
            set
            {
                TableType = TableType.Parse(value);
            }
        }

        [XmlIgnore]
        public TableType TableType { get; set; }

        [XmlIgnore]
        public List<TableTypeDescription> TableTypeDescriptors
        {
            get
            {
                return TableType.GetTableTypeDescriptions().ToList();
            }
        }

        [XmlElement]
        public TournamentDescriptor Tournament { get; set; }

        [XmlElement, DefaultValue(false)]
        public bool IsStraddle { get; set; }

        [XmlElement]
        public bool IsTournament
        {
            get
            {
                return Tournament != null;
            }
        }

        [XmlElement]
        public long Identifier
        {
            get; set;
        }

        [XmlElement, DefaultValue(0)]
        public decimal CashBuyInHigh
        {
            get; set;
        }

        public override bool Equals(object obj)
        {
            var descriptor = obj as GameDescriptor;

            if (descriptor == null)
            {
                return false;
            }

            return (descriptor.ToString().Equals(ToString()));
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}.{1}.{2}.{3}.{4}.{5}", Site, GameType, Limit, TableType, SeatType, Tournament);
        }
    }
}