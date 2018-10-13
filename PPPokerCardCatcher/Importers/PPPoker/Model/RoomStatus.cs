using ProtoBuf;

namespace PPPokerCardCatcher.Importers.PPPoker.Model
{
    [ProtoContract]
    class RoomStatus
    {
        [ProtoMember(1)]
        public int TimeLeft { get; set; }

        [ProtoMember(2)]
        public ProfitInfo[] Profit { get; set; }

        [ProtoMember(3)]
        public UserBrief[] Observer { get; set; }

        [ProtoMember(4)]
        public bool AuthLimit { get; set; }

        [ProtoMember(5)]
        public bool IsStarted { get; set; }

        [ProtoMember(6)]
        public bool IsBlindRunning { get; set; }

        [ProtoMember(7)]
        public bool IsJackpotOpen { get; set; }

        [ProtoMember(8)]
        public int AddRoomtimeQuota { get; set; }
    }
}
