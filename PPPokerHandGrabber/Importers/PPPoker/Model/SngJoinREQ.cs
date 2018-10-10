using ProtoBuf;

namespace PPPokerHandGrabber.Importers.PPPoker.Model
{
    [ProtoContract]
    class SngJoinREQ
    {
        [ProtoMember(1)]
        public string TempID { get; set; }

        [ProtoMember(2)]
        public int ClubID { get; set; }
    }
}
