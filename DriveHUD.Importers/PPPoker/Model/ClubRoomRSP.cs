using ProtoBuf;

namespace DriveHUD.Importers.PPPoker.Model
{
    [ProtoContract]
    class ClubRoomRSP
    {
        [ProtoMember(1)]
        public int LeagueID { get; set; }

        [ProtoMember(2)]
        public ClubRoomInfo[] Info { get; set; }

        [ProtoMember(3)]
        public int ClubID { get; set; }
    }
}
