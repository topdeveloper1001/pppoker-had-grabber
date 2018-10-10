using ProtoBuf;

namespace PPPokerHandGrabber.Importers.PPPoker.Model
{
    [ProtoContract]
    class CroupierActionBRC
    {
        [ProtoMember(1)]
        public int Scene { get; set; }

        [ProtoMember(2)]
        public string Text { get; set; }

        [ProtoMember(3)]
        public long Time { get; set; }

        [ProtoMember(4)]
        public int To { get; set; }

        [ProtoMember(5)]
        public int Cost { get; set; }
    }
}
