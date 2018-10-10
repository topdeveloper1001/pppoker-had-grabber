using ProtoBuf;

namespace PPPokerHandGrabber.Importers.PPPoker.Model
{
    [ProtoContract]
    class PinePlayerResult
    {
        [ProtoMember(1)]
        public int Uid { get; set; }

        [ProtoMember(2)]
        public int SeatID { get; set; }

        [ProtoMember(3)]
        public PineCard Card { get; set; }

        [ProtoMember(4)]
        public int Fantasy { get; set; }

        [ProtoMember(5)]
        public string Name { get; set; }

        [ProtoMember(6)]
        public long Chips { get; set; }

        [ProtoMember(7)]
        public string Icon { get; set; }

        [ProtoMember(8)]
        public PineResultScore[] Score { get; set; }
    }
}
