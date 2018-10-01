using ProtoBuf;

namespace DriveHUD.Importers.PPPoker.Model
{
    [ProtoContract]
    class LeaveRoomRSP
    {
        [ProtoMember(1)]
        public int Code { get; set; }

        [ProtoMember(2)]
        public string Reason { get; set; }

        [ProtoMember(3)]
        public int Flag { get; set; }

        [ProtoMember(4)]
        public int ClubID { get; set; }

        [ProtoMember(5)]
        public int LeagueID { get; set; }
    }
}
