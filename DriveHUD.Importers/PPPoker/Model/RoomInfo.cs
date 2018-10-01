using ProtoBuf;

namespace DriveHUD.Importers.PPPoker.Model
{
    [ProtoContract]
    class RoomInfo
    {
        [ProtoMember(1)]
        public int RoomID { get; set; }

        [ProtoMember(2)]
        public string RoomName { get; set; }

        [ProtoMember(3)]
        public string OwnerName { get; set; }

        [ProtoMember(4)]
        public long Blind { get; set; }

        [ProtoMember(5)]
        public long Ante { get; set; }

        [ProtoMember(6)]
        public long MinBuyIn { get; set; }

        [ProtoMember(7)]
        public int ActionTime { get; set; }

        [ProtoMember(8)]
        public int GameTime { get; set; }

        [ProtoMember(9)]
        public int SeatNum { get; set; }

        [ProtoMember(10)]
        public long OwnerID { get; set; }

        [ProtoMember(11)]
        public RoomType RoomType { get; set; }

        [ProtoMember(12)]
        public int FeeType { get; set; }

        [ProtoMember(13)]
        public int FeePoint { get; set; }

        [ProtoMember(14)]
        public bool AuthLimit { get; set; }

        [ProtoMember(15)]
        public int Region { get; set; }

        [ProtoMember(16)]
        public string OwnerIcon { get; set; }

        [ProtoMember(17)]
        public int ClubID { get; set; }

        [ProtoMember(18)]
        public string Platform { get; set; }

        [ProtoMember(19)]
        public long DefaultBuyIn { get; set; }

        [ProtoMember(20)]
        public int Cap { get; set; }

        [ProtoMember(21)]
        public bool PoolLimit { get; set; }

        [ProtoMember(22)]
        public long MaxBuyIn { get; set; }

        [ProtoMember(23)]
        public bool IsRunMultiTimes { get; set; }

        [ProtoMember(24)]
        public bool IsInsurance { get; set; }

        [ProtoMember(25)]
        public int LeagueID { get; set; }

        [ProtoMember(26)]
        public string ClubName { get; set; }

        [ProtoMember(27)]
        public long ClubOwnerID { get; set; }

        [ProtoMember(28)]
        public string ClubIcon { get; set; }

        [ProtoMember(29)]
        public string TempID { get; set; }

        [ProtoMember(30)]
        public bool Official { get; set; }

        [ProtoMember(31)]
        public bool GpsLimit { get; set; }

        [ProtoMember(32)]
        public bool IPLimit { get; set; }

        [ProtoMember(33)]
        public int GpsDistanceLimit { get; set; }

        [ProtoMember(34)]
        public int CityID { get; set; }

        [ProtoMember(35)]
        public RoomMode RoomMode { get; set; }

        [ProtoMember(36)]
        public uint OwnerVipLevel { get; set; }

        [ProtoMember(37)]
        public int AutoStart { get; set; }
    }
}
