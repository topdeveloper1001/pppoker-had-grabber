using ProtoBuf;

namespace PPPokerHandGrabber.Importers.PPPoker.Model
{
    [ProtoContract]
    class SitDownRSP
    {
        [ProtoMember(1)]
        public SitDownCode Code { get; set; }

        [ProtoMember(2)]
        public string Reason { get; set; }

        [ProtoMember(3)]
        public int SeatID { get; set; }

        [ProtoMember(4)]
        public long Chips { get; set; }

        [ProtoMember(5)]
        public int AddActionTimeCost { get; set; }
    }
}
