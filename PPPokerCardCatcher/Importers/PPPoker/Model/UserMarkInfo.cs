using ProtoBuf;

namespace PPPokerCardCatcher.Importers.PPPoker.Model
{
    [ProtoContract]
    class UserMarkInfo
    {
        [ProtoMember(1)]
        public long Uid { get; set; }

        [ProtoMember(2)]
        public string Content { get; set; }

        [ProtoMember(3)]
        public MarkColor MarkColor { get; set; }
    }
}
