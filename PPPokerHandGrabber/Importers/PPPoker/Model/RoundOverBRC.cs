using ProtoBuf;

namespace PPPokerHandGrabber.Importers.PPPoker.Model
{
    [ProtoContract]
    class RoundOverBRC
    {
        [ProtoMember(1)]
        public long[] Pool { get; set; }
    }
}
