using ProtoBuf;

namespace PPPokerHandGrabber.Importers.PPPoker.Model
{
    [ProtoContract]
    class MttListRSP
    {
        [ProtoMember(1)]
        public MttItem[] Mtt { get; set; }

        [ProtoMember(2)]
        public SngItem[] Sng { get; set; }
    }
}
