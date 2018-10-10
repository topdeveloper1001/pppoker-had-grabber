using ProtoBuf;

namespace PPPokerHandGrabber.Importers.PPPoker.Model
{
    [ProtoContract]
    class GameComingRSP
    {
        [ProtoMember(1)]
        public int Left { get; set; }
    }
}
