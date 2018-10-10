using ProtoBuf;

namespace PPPokerHandGrabber.Importers.PPPoker.Model
{
    [ProtoContract]
    class SngOverBRC
    {
        [ProtoMember(1)]
        public ProfitInfo[] Info { get; set; }

        [ProtoMember(2)]
        public bool IsStarted { get; set; }
    }
}
