using ProtoBuf;

namespace PPPokerHandGrabber.Importers.PPPoker.Model
{
    [ProtoContract]
    class SeatStatus
    {
        [ProtoMember(1)]
        public int SeatID { get; set; }

        [ProtoMember(2)]
        public ActionType ActionType { get; set; }

        [ProtoMember(3)]
        public UserBrief Player { get; set; }

        [ProtoMember(4)]
        public long RemainingChips { get; set; }

        [ProtoMember(5)]
        public long InvestedChips { get; set; }

        [ProtoMember(6)]
        public bool HasCard { get; set; }

        [ProtoMember(7)]
        public bool SeatReserve { get; set; }

        [ProtoMember(8)]
        public string Country { get; set; }

        [ProtoMember(9)]
        public uint VipLevel { get; set; }

        [ProtoMember(10)]
        public bool WaitBlind { get; set; }

        [ProtoMember(11)]
        public int RebuyLeftTime { get; set; }

        [ProtoMember(12)]
        public bool RebuyWaitAuth { get; set; }

        [ProtoMember(13)]
        public int ClubID { get; set; }

        [ProtoMember(14)]
        public int Longitude { get; set; } // Original name: gps_lon

        [ProtoMember(15)]
        public int Latitude { get; set; } // Original name: gps_lat

        [ProtoMember(16)]
        public string ClubName { get; set; }
    }
}
