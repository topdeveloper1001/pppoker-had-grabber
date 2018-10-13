using ProtoBuf;

namespace PPPokerCardCatcher.Importers.PPPoker.Model
{
    [ProtoContract]
    class PinePlayingStatus
    {
        [ProtoMember(1)]
        public int Uid { get; set; }

        [ProtoMember(2)]
        public int SeatID { get; set; }

        [ProtoMember(3)]
        public PineCard Card { get; set; }

        [ProtoMember(4)]
        public int RebuyLeftTime { get; set; }

        [ProtoMember(5)]
        public uint VipLevel { get; set; }

        [ProtoMember(6)]
        public int Fantasy { get; set; }

        [ProtoMember(7)]
        public bool SittingOut { get; set; }

        [ProtoMember(8)]
        public int ActionLeftTime { get; set; }

        [ProtoMember(9)]
        public string Icon { get; set; }

        [ProtoMember(10)]
        public string Name { get; set; }

        [ProtoMember(11)]
        public long Chips { get; set; }

        [ProtoMember(12)]
        public bool Ready { get; set; }

        [ProtoMember(13)]
        public bool AbleReady { get; set; }

        [ProtoMember(14)]
        public bool AbleReadyLeftTime { get; set; }

        [ProtoMember(15)]
        public int AddActionTimeDiamond { get; set; }

        [ProtoMember(16)]
        public int AddActionTimeVipFreeTimes { get; set; }

        [ProtoMember(17)]
        public int AddActionTimeGiftBagFreeTimes { get; set; }

        [ProtoMember(18)]
        public bool LeaveRoomAfterThisHand { get; set; }
    }
}
