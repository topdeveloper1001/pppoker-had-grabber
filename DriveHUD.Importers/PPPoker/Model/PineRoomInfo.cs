using ProtoBuf;

namespace DriveHUD.Importers.PPPoker.Model
{
    [ProtoContract]
    class PineRoomInfo
    {
        [ProtoMember(1)]
        public int RoomID { get; set; }

        [ProtoMember(2)]
        public long OwnerID { get; set; }

        [ProtoMember(3)]
        public int ClubID { get; set; }

        [ProtoMember(4)]
        public int LeagueID { get; set; }

        [ProtoMember(5)]
        public int SeatNum { get; set; }

        [ProtoMember(6)]
        public string RoomName { get; set; }

        [ProtoMember(7)]
        public int ActionTime { get; set; }

        [ProtoMember(8)]
        public int BaseScore { get; set; }

        [ProtoMember(9)]
        public int DefaultBuyIn { get; set; }

        [ProtoMember(10)]
        public int MinBuyIn { get; set; }

        [ProtoMember(11)]
        public int ActionType { get; set; }

        [ProtoMember(12)]
        public int FeePoint { get; set; }

        [ProtoMember(13)]
        public int FeeCap { get; set; }

        [ProtoMember(14)]
        public int GameTime { get; set; }

        [ProtoMember(15)]
        public bool GpsLimit { get; set; }

        [ProtoMember(16)]
        public bool IPLimit { get; set; }

        [ProtoMember(17)]
        public int GpsDistanceLimit { get; set; }

        [ProtoMember(18)]
        public string OwnerName { get; set; }

        [ProtoMember(19)]
        public string OwnerIcon { get; set; }

        [ProtoMember(20)]
        public string ClubName { get; set; }

        [ProtoMember(21)]
        public string ClubIcon { get; set; }

        [ProtoMember(22)]
        public long ClubOwnerID { get; set; }

        [ProtoMember(23)]
        public string Platform { get; set; }

        [ProtoMember(24)]
        public int Region { get; set; }

        [ProtoMember(25)]
        public int MaxBuyIn { get; set; }

        [ProtoMember(26)]
        public RoomMode RoomMode { get; set; }

        [ProtoMember(27)]
        public int CreateClubID { get; set; }

        [ProtoMember(28)]
        public int GameMode { get; set; }

        [ProtoMember(29)]
        public int AutoStart { get; set; }

        [ProtoMember(30)]
        public int CardHolderVipLevel { get; set; }
    }
}
