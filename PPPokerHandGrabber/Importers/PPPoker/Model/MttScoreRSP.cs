using ProtoBuf;

namespace PPPokerHandGrabber.Importers.PPPoker.Model
{
    [ProtoContract]
    class MttScoreRSP
    {
        [ProtoMember(1)]
        public int Score { get; set; }
    }
}
