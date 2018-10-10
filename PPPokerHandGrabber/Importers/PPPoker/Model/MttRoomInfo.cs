using ProtoBuf;

namespace PPPokerHandGrabber.Importers.PPPoker.Model
{
    [ProtoContract]
    class MttRoomInfo
    {
        [ProtoMember(1)]
        public long MttStartTime { get; set; }

        [ProtoMember(2)]
        public int DelayJoinLevel { get; set; }

        [ProtoMember(3)]
        public int RebuyNum { get; set; }

        [ProtoMember(4)]
        public int AddonRate { get; set; }

        [ProtoMember(5)]
        public int BreakTime { get; set; }

        [ProtoMember(6)]
        public int MinPlayerNum { get; set; }

        [ProtoMember(7)]
        public int MaxPlayerNum { get; set; }

        [ProtoMember(24)]
        public long EnsureChips { get; set; }

        [ProtoMember(25)]
        public long HunterReward { get; set; }

        [ProtoMember(26)]
        public double ScoreRate { get; set; }

        [ProtoMember(27)]
        public long MttSignupTime { get; set; }

        [ProtoMember(8)]
        public long StartTimestamp { get; set; }

        [ProtoMember(9)]
        public long TotalReward { get; set; }

        [ProtoMember(10)]
        public int TotalPlayerNum { get; set; }

        [ProtoMember(11)]
        public int CurrentPlayerNum { get; set; }

        [ProtoMember(12)]
        public long AvgChips { get; set; }

        [ProtoMember(13)]
        public int CurrentLevel { get; set; }

        [ProtoMember(14)]
        public int AddonTimeLeft { get; set; }

        [ProtoMember(15)]
        public int BreakTimeLeft { get; set; }

        [ProtoMember(18)]
        public int MttStatus { get; set; }

        [ProtoMember(19)]
        public long EndRebuyTimestamp { get; set; }

        [ProtoMember(20)]
        public long Now { get; set; }

        [ProtoMember(16)]
        public int JoinStatus { get; set; }

        [ProtoMember(17)]
        public int RebuyNumLeft { get; set; }

        [ProtoMember(21)]
        public int AddonNumLeft { get; set; }

        [ProtoMember(22)]
        public bool IsPlaying { get; set; }

        [ProtoMember(23)]
        public int Tid { get; set; }

        [ProtoMember(28)]
        public RoomMode RoomMode { get; set; }

        [ProtoMember(29)]
        public bool IsPlayed { get; set; }

        [ProtoMember(30)]
        public long ReBuyIn { get; set; }

        [ProtoMember(31)]
        public long AddOnBuyIn { get; set; }

        [ProtoMember(32)]
        public MttRewardPercentType PercentType { get; set; }

        [ProtoMember(33)]
        public long RebuyCharge { get; set; }

        [ProtoMember(34)]
        public long AddonCharge { get; set; }
    }
}
