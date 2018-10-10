using ProtoBuf;

namespace PPPokerHandGrabber.Importers.PPPoker.Model
{
    [ProtoContract]
    class Item
    {
        [ProtoMember(1)]
        public long ID { get; set; }

        [ProtoMember(2)]
        public string Name { get; set; }
    }
}
