using ProtoBuf;

namespace PPPokerHandGrabber.Importers.PPPoker.Model
{
    [ProtoContract]
    class ItemClassInfo
    {
        [ProtoMember(1)]
        public string ItemName { get; set; }

        [ProtoMember(2)]
        public int ItemType { get; set; }

        [ProtoMember(3)]
        public string IconUrl { get; set; }

        [ProtoMember(4)]
        public int ExpireTime { get; set; }

        [ProtoMember(5)]
        public bool CanMerge { get; set; }

        [ProtoMember(6)]
        public long Price { get; set; }
    }
}
