using ProtoBuf;

namespace PPPokerHandGrabber.Importers.PPPoker.Model
{
    [ProtoContract]
    class SitDownBRC
    {
        [ProtoMember(1)]
        public int SeatID { get; set; }

        [ProtoMember(2)]
        public long Chips { get; set; }

        [ProtoMember(3)]
        public UserBrief Brief { get; set; }

        [ProtoMember(4)]
        public SeatStatus Status { get; set; }
    }
}
