using ProtoBuf;

namespace PPPokerHandGrabber.Importers.PPPoker.Model
{
    [ProtoContract]
    class ItemInfo
    {
        [ProtoMember(1)]
        public long ItemId { get; set; }

        [ProtoMember(2)]
        public string ItemName { get; set; }

        [ProtoMember(3)]
        public int Num { get; set; }

        [ProtoMember(4)]
        public long Deadline { get; set; }
    }
}
