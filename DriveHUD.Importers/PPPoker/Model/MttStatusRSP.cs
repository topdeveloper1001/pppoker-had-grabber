using ProtoBuf;

namespace DriveHUD.Importers.PPPoker.Model
{
    [ProtoContract]
    class MttStatusRSP
    {
        [ProtoMember(1)]
        public int Rank { get; set; }

        [ProtoMember(2)]
        public int BlindLevel { get; set; }

        [ProtoMember(3)]
        public int JoinStatus { get; set; }

        [ProtoMember(4)]
        public long TotalReward { get; set; }

        [ProtoMember(5)]
        public int TotalPlayerNum { get; set; }

        [ProtoMember(6)]
        public int CurrentPlayerNum { get; set; }

        [ProtoMember(7)]
        public long AvgChips { get; set; }

        [ProtoMember(8)]
        public long TopChips { get; set; }

        [ProtoMember(9)]
        public long LowChips { get; set; }

        [ProtoMember(10)]
        public int AddonNum { get; set; }

        [ProtoMember(11)]
        public int RebuyNum { get; set; }
    }
}
