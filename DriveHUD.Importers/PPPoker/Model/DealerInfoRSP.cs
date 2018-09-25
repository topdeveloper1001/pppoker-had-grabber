using ProtoBuf;

namespace DriveHUD.Importers.PPPoker.Model
{
    [ProtoContract]
    class PlayerInfo // Original: StartInfo
    {
        [ProtoMember(1)]
        public int SeatID { get; set; }

        [ProtoMember(2)]
        public long Chips { get; set; }
    }

    [ProtoContract]
    class DealerInfoRSP
    {
        [ProtoMember(1)]
        public int Dealer { get; set; }

        [ProtoMember(2)]
        public int SmallBlind { get; set; }

        [ProtoMember(3)]
        public int BigBlind { get; set; }

        [ProtoMember(4)]
        public PlayerInfo[] Players { get; set; }

        [ProtoMember(5)]
        public string GameID { get; set; }
    }
}
