using ProtoBuf;

namespace PPPokerCardCatcher.Importers.PPPoker.Model
{
    [ProtoContract]
    class SngRoomInfo : IRoomInfo
    {
        [ProtoMember(1)]
        public int RoomID { get; set; }

        [ProtoMember(2)]
        public string RoomName { get; set; }

        [ProtoMember(3)]
        public long OwnerID { get; set; }

        [ProtoMember(4)]
        public string OwnerName { get; set; }

        [ProtoMember(5)]
        public string OwnerIcon { get; set; }

        [ProtoMember(6)]
        public int BlindType { get; set; }

        [ProtoMember(7)]
        public long[] BlindList { get; set; }

        [ProtoMember(8)]
        public long[] AnteList { get; set; }

        [ProtoMember(9)]
        public int UpBlindTime { get; set; }

        [ProtoMember(10)]
        public long BuyIn { get; set; }

        [ProtoMember(11)]
        public long BeginChips { get; set; }

        [ProtoMember(12)]
        public int ActionTime { get; set; }

        [ProtoMember(13)]
        public int SeatNum { get; set; }

        [ProtoMember(14)]
        public RoomType RoomType { get; set; }

        [ProtoMember(15)]
        public bool AuthLimit { get; set; }

        [ProtoMember(16)]
        public int Region { get; set; }

        [ProtoMember(17)]
        public int ClubID { get; set; }

        [ProtoMember(18)]
        public string Platform { get; set; }

        [ProtoMember(19)]
        public string ClubName { get; set; }

        [ProtoMember(20)]
        public long Blind { get; set; }

        [ProtoMember(21)]
        public long Ante { get; set; }

        [ProtoMember(22)]
        public long Charge { get; set; }

        [ProtoMember(23)]
        public int LeagueID { get; set; }

        [ProtoMember(24)]
        public string ItemName { get; set; }

        [ProtoMember(25)]
        public int Priority { get; set; }

        [ProtoMember(26)]
        public long ClubOwnerID { get; set; }

        [ProtoMember(27)]
        public string ClubIcon { get; set; }

        [ProtoMember(28)]
        public string TempID { get; set; }

        [ProtoMember(29)]
        public bool Official { get; set; }

        [ProtoMember(30)]
        public bool FixedReward { get; set; }

        [ProtoMember(31)]
        public MttRewardInfo RewardInfo { get; set; }

        [ProtoMember(32)]
        public bool GpsLimit { get; set; }

        [ProtoMember(33)]
        public bool IPLimit { get; set; }

        [ProtoMember(34)]
        public int GpsDistanceLimit { get; set; }

        [ProtoMember(35)]
        public RoomMode RoomMode { get; set; }

        [ProtoMember(36)]
        public uint OwnerVipLevel { get; set; }

        [ProtoMember(37)]
        public ItemClassInfo[] ItemInfo { get; set; }

        [ProtoMember(38)]
        public bool SittingOut { get; set; }

        [ProtoMember(39)]
        public SignUpType SignUpType { get; set; }

        [ProtoMember(40)]
        public OfficialDescType DescType { get; set; }

        [ProtoMember(41)]
        public OfficialLocation Location { get; set; }

        [ProtoMember(42)]
        public string[] PlatformLimits { get; set; }
    }
}
