using ProtoBuf;

namespace DriveHUD.Importers.PPPoker.Model
{
    [ProtoContract]
    class EnterRoomREQ
    {
        [ProtoMember(1)]
        public int RoomID { get; set; }

        [ProtoMember(2)]
        public int Tid { get; set; }

        [ProtoMember(3)]
        public long Uid { get; set; }

        [ProtoMember(4)]
        public int ClubID { get; set; }

        [ProtoMember(5)]
        public int LeagueID { get; set; }

        [ProtoMember(6)]
        public string IP { get; set; }
    }
}
