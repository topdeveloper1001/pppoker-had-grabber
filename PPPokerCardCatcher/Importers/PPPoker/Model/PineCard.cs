using ProtoBuf;

namespace PPPokerCardCatcher.Importers.PPPoker.Model
{
    [ProtoContract]
    class PineCard
    {
        [ProtoMember(1)]
        public int HeadScore { get; set; }

        [ProtoMember(2)]
        public int MiddleScore { get; set; }

        [ProtoMember(3)]
        public int TailScore { get; set; }

        [ProtoMember(4)]
        public int[] HeadCard { get; set; }

        [ProtoMember(5)]
        public int[] MiddleCard { get; set; }

        [ProtoMember(6)]
        public int[] TailCard { get; set; }

        [ProtoMember(7)]
        public int[] AbandonCard { get; set; }

        [ProtoMember(8)]
        public HandType HeadType { get; set; }

        [ProtoMember(9)]
        public HandType MiddleType { get; set; }

        [ProtoMember(10)]
        public HandType TailType { get; set; }

        [ProtoMember(11)]
        public bool Bust { get; set; }

        [ProtoMember(12)]
        public int[] HandCard { get; set; }

        [ProtoMember(13)]
        public int Round { get; set; }
    }
}
