using ProtoBuf;

namespace DriveHUD.Importers.PPPoker.Model
{
    [ProtoContract]
    class PlayingStatus
    {
        [ProtoMember(1)]
        public int Card1 { get; set; }

        [ProtoMember(2)]
        public int Card2 { get; set; }

        [ProtoMember(3)]
        public int ActionSeatID { get; set; }

        [ProtoMember(4)]
        public long CallNeedChips { get; set; }

        [ProtoMember(5)]
        public long MinChipIn { get; set; }

        [ProtoMember(6)]
        public long MaxChipIn { get; set; }

        [ProtoMember(7)]
        public int ActionTime { get; set; }

        [ProtoMember(8)]
        public int AddActionTimeCost { get; set; }

        [ProtoMember(9)]
        public int Card3 { get; set; }

        [ProtoMember(10)]
        public int Card4 { get; set; }

        [ProtoMember(11)]
        public bool ShowStraddle { get; set; }

        [ProtoMember(12)]
        public int Role { get; set; }

        [ProtoMember(13)]
        public bool RoomAuthority { get; set; }

        [ProtoMember(14)]
        public int PreActionType { get; set; }

        [ProtoMember(15)]
        public long PreActionChips { get; set; }

        [ProtoMember(16)]
        public bool BannedTalk { get; set; }
    }
}
