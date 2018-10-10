using ProtoBuf;

namespace PPPokerHandGrabber.Importers.PPPoker.Model
{
    [ProtoContract]
    class NoticeBRC
    {
        [ProtoMember(1)]
        public string Message { get; set; }

        [ProtoMember(2)]
        public int Type { get; set; }
    }
}
