using ProtoBuf;

namespace PPPokerCardCatcher.Importers.PPPoker.Model
{
    [ProtoContract]
    class ShowMyCardBRC : IHoleCardInfo
    {
        [ProtoMember(1)]
        public int SeatID { get; set; }

        [ProtoMember(2)]
        public int Card1 { get; set; }

        [ProtoMember(3)]
        public int Card2 { get; set; }

        [ProtoMember(4)]
        public int Card3 { get; set; }

        [ProtoMember(5)]
        public int Card4 { get; set; }
    }
}
