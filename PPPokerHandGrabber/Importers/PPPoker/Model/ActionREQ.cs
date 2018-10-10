using ProtoBuf;

namespace PPPokerHandGrabber.Importers.PPPoker.Model
{
    [ProtoContract]
    class ActionREQ
    {
        [ProtoMember(1)]
        public ActionType ActionType { get; set; }

        [ProtoMember(2)]
        public long Chips { get; set; }
    }
}
