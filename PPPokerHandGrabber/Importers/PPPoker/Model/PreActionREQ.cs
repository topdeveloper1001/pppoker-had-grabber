using ProtoBuf;

namespace PPPokerHandGrabber.Importers.PPPoker.Model
{
    [ProtoContract]
    class PreActionREQ
    {
        [ProtoMember(1)]
        public int Type { get; set; }

        [ProtoMember(2)]
        public long Chips { get; set; }
    }
}
