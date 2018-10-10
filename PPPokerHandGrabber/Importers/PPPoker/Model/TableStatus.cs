using ProtoBuf;

namespace PPPokerHandGrabber.Importers.PPPoker.Model
{
    [ProtoContract]
    class TableStatus
    {
        [ProtoMember(1)]
        public bool IsPlaying { get; set; }

        [ProtoMember(2)]
        public int ActionIndex { get; set; }

        [ProtoMember(3)]
        public int DealerIndex { get; set; }

        [ProtoMember(4)]
        public int SmallBlindIndex { get; set; }

        [ProtoMember(5)]
        public int BigBlindIndex { get; set; }

        [ProtoMember(6)]
        public SeatStatus[] Seat { get; set; }

        [ProtoMember(7)]
        public long[] Pool { get; set; }

        [ProtoMember(8)]
        public RoundStage Stage { get; set; }

        [ProtoMember(9)]
        public int[] Board { get; set; }

        [ProtoMember(10)]
        public int Tid { get; set; }

        [ProtoMember(11)]
        public bool IsFinalTable { get; set; }

        [ProtoMember(12)]
        public MultiBoard[] MultiBoards { get; set; }

        [ProtoMember(13)]
        public InsurancePoolInfo[] InsurancePools { get; set; }

        [ProtoMember(14)]
        public bool IsWaitSync { get; set; }
    }
}
