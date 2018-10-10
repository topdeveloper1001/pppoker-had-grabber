using ProtoBuf;

namespace PPPokerHandGrabber.Importers.PPPoker.Model
{
    [ProtoContract]
    class MttItem
    {
        [ProtoMember(1)]
        public int RoomID { get; set; }

        [ProtoMember(2)]
        public int Priority { get; set; }

        [ProtoMember(3)]
        public long EndRebuyTimestamp { get; set; }

        [ProtoMember(4)]
        public long Now { get; set; }

        [ProtoMember(5)]
        public long MttStartTime { get; set; }

        [ProtoMember(6)]
        public bool IsHunter { get; set; }

        [ProtoMember(7)]
        public long Cost { get; set; }

        [ProtoMember(8)]
        public string ItemName { get; set; }

        [ProtoMember(9)]
        public string TempID { get; set; }

        [ProtoMember(10)]
        public bool CanRebuy { get; set; }

        [ProtoMember(11)]
        public bool CanAddon { get; set; }

        [ProtoMember(12)]
        public int TotalPlayerNum { get; set; }

        [ProtoMember(13)]
        public int CurrentPlayerNum { get; set; }

        [ProtoMember(14)]
        public int MttStatus { get; set; }

        [ProtoMember(15)]
        public long StartTimestamp { get; set; }

        [ProtoMember(16)]
        public long SignUpTimestamp { get; set; }

        [ProtoMember(17)]
        public int JoinStatus { get; set; }

        [ProtoMember(18)]
        public bool IsPlaying { get; set; }

        [ProtoMember(19)]
        public int MaxPlayerNum { get; set; }

        [ProtoMember(20)]
        public SignUpType SignupType { get; set; }

        [ProtoMember(21)]
        public OfficialDescType DescType { get; set; }

        [ProtoMember(22)]
        public bool FixedReward { get; set; }

        [ProtoMember(23)]
        public bool IsPlayed { get; set; }

        [ProtoMember(24)]
        public long HunterReward { get; set; }

        [ProtoMember(25)]
        public OfficialLocation Location { get; set; }

        [ProtoMember(26)]
        public long Rebuyin { get; set; }

        [ProtoMember(27)]
        public long AddonBuyin { get; set; }

        [ProtoMember(28)]
        public int LeftRebuyNum { get; set; }

        [ProtoMember(29)]
        public string[] PlatformLimits { get; set; }

        [ProtoMember(30)]
        public MttRewardPercentType PercentType { get; set; }

        [ProtoMember(31)]
        public GameSetPlayStatus GameSetStatus { get; set; }
    }
}
