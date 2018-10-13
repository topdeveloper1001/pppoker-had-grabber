using ProtoBuf;

namespace PPPokerCardCatcher.Importers.PPPoker.Model
{
    [ProtoContract]
    class ActionBRC
    {
        [ProtoMember(1)]
        public int SeatID { get; set; }

        [ProtoMember(2)]
        public ActionType ActionType { get; set; }

        [ProtoMember(3)]
        public long Chips { get; set; }

        [ProtoMember(4)]
        public long HandChips { get; set; }
    }
}
