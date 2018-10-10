using ProtoBuf;

namespace PPPokerHandGrabber.Importers.PPPoker.Model
{
    [ProtoContract]
    class ProfitInfo
    {
        [ProtoMember(1)]
        public string UserName { get; set; }

        [ProtoMember(2)]
        public long BuyIn { get; set; }

        [ProtoMember(3)]
        public long Profit { get; set; }

        [ProtoMember(4)]
        public long Uid { get; set; }

        [ProtoMember(5)]
        public string IconUrl { get; set; }

        [ProtoMember(6)]
        public int HandsNum { get; set; }

        [ProtoMember(7)]
        public long Fee { get; set; }

        [ProtoMember(8)]
        public long Cost { get; set; }

        [ProtoMember(9)]
        public long Reward { get; set; }

        [ProtoMember(10)]
        public int Rank { get; set; }

        [ProtoMember(11)]
        public long InsuranceChips { get; set; }

        [ProtoMember(12)]
        public long HunterReward { get; set; }

        [ProtoMember(13)]
        public long AgentUid { get; set; }

        [ProtoMember(14)]
        public MttRewardInfo RewardInfo { get; set; }

        [ProtoMember(15)]
        public int RebuyNum { get; set; }

        [ProtoMember(16)]
        public int AddonNum { get; set; }

        [ProtoMember(17)]
        public int MttScore { get; set; }

        [ProtoMember(18)]
        public Item[] Tickets { get; set; } // Original name: ticket

        [ProtoMember(19)]
        public int ClubID { get; set; }

        [ProtoMember(20)]
        public long JackpotFee { get; set; }

        [ProtoMember(21)]
        public long JackpotReward { get; set; }
    }
}
