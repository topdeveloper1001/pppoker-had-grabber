using ProtoBuf;

namespace PPPokerCardCatcher.Importers.PPPoker.Model
{
    [ProtoContract]
    class SngItem
    {
        [ProtoMember(1)]
        public string TempID { get; set; }

        [ProtoMember(2)]
        public int Priority { get; set; }

        [ProtoMember(3)]
        public bool IsPlaying { get; set; }

        [ProtoMember(4)]
        public long Cost { get; set; }

        [ProtoMember(5)]
        public string ItemName { get; set; }

        [ProtoMember(6)]
        public int TotalPlayerNum { get; set; }

        [ProtoMember(7)]
        public int CurrentPlayerNum { get; set; }

        [ProtoMember(8)]
        public int RoomID { get; set; }

        [ProtoMember(9)]
        public SignUpType SignUpType { get; set; }

        [ProtoMember(10)]
        public OfficialDescType DescType { get; set; }

        [ProtoMember(11)]
        public bool FixedReward { get; set; }

        [ProtoMember(12)]
        public OfficialLocation Location { get; set; }

        [ProtoMember(13)]
        public string[] PlatformLimits { get; set; }
    }
}
