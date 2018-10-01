using ProtoBuf;

namespace DriveHUD.Importers.PPPoker.Model
{
    [ProtoContract]
    class ClubRoomInfo
    {
        [ProtoMember(1)]
        public int RoomID { get; set; }

        [ProtoMember(2)]
        public string RoomName { get; set; }

        [ProtoMember(3)]
        public string OwnerName { get; set; }

        [ProtoMember(4)]
        public string OwnerIcon { get; set; }

        [ProtoMember(5)]
        public long Blind { get; set; }

        [ProtoMember(6)]
        public long Ante { get; set; }

        [ProtoMember(7)]
        public int GameTime { get; set; }

        [ProtoMember(8)]
        public int LeftTime { get; set; }

        [ProtoMember(9)]
        public int SeatNum { get; set; }

        [ProtoMember(10)]
        public int Players { get; set; }

        [ProtoMember(11)]
        public bool IsStarted { get; set; }

        [ProtoMember(12)]
        public int AuthNum { get; set; }

        [ProtoMember(13)]
        public RoomType RoomType { get; set; }

        [ProtoMember(14)]
        public int UpblindTime { get; set; }

        [ProtoMember(15)]
        public long Buyin { get; set; }

        [ProtoMember(16)]
        public int TotalPlayerNum { get; set; }

        [ProtoMember(17)]
        public int CurrentPlayerNum { get; set; }

        [ProtoMember(18)]
        public int MttStatus { get; set; }

        [ProtoMember(19)]
        public long EndRebuyTimestamp { get; set; }

        [ProtoMember(20)]
        public long StartTimestamp { get; set; }

        [ProtoMember(21)]
        public long Now { get; set; }

        [ProtoMember(22)]
        public long MttStartTime { get; set; }

        [ProtoMember(23)]
        public WaitListUser[] WaitUsers { get; set; }

        [ProtoMember(24)]
        public bool IsHunter { get; set; }

        [ProtoMember(25)]
        public bool GpsLimit { get; set; }

        [ProtoMember(26)]
        public bool IpLimit { get; set; }

        [ProtoMember(27)]
        public uint OwnerVipLevel { get; set; }

        [ProtoMember(28)]
        public long CreateTimestamp { get; set; }

        [ProtoMember(29)]
        public int GameMode { get; set; }

        [ProtoMember(30)]
        public bool IsIn { get; set; }

        [ProtoMember(31)]
        public bool IsInsurance { get; set; }
    }
}
