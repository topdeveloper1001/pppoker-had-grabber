using ProtoBuf;

namespace DriveHUD.Importers.PPPoker.Model
{
    [ProtoContract]
    class PineRoomStatus
    {
        [ProtoMember(1)]
        public PinePlayingStatus[] PinePlayingStatus { get; set; }

        [ProtoMember(2)]
        public int DealerSeatID { get; set; }

        [ProtoMember(3)]
        public PinePlayerResult[] PlayerResult { get; set; }

        [ProtoMember(4)]
        public int ActionType { get; set; }
    }
}
