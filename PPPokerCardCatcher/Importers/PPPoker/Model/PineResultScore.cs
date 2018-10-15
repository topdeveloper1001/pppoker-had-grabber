using ProtoBuf;

namespace PPPokerCardCatcher.Importers.PPPoker.Model
{
    [ProtoContract]
    class PineResultScore
    {
        [ProtoMember(1)]
        public int Uid { get; set; }

        [ProtoMember(2)]
        public int SeatID { get; set; }

        [ProtoMember(3)]
        public int HeadScore { get; set; }

        [ProtoMember(4)]
        public int MiddleScore { get; set; }

        [ProtoMember(5)]
        public int TailScore { get; set; }

        [ProtoMember(6)]
        public int AllWinScore { get; set; }

        [ProtoMember(7)]
        public long Profit { get; set; }
    }
}
