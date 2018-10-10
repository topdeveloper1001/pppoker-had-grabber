using ProtoBuf;

namespace PPPokerHandGrabber.Importers.PPPoker.Model
{
    [ProtoContract]
    class EnterRoomRSP
    {
        [ProtoMember(1)]
        public int Code { get; set; }

        [ProtoMember(2)]
        public string Reason { get; set; }

        [ProtoMember(3)]
        public TableStatus TableStatus { get; set; }

        [ProtoMember(4)]
        public RoomStatus RoomStatus { get; set; }

        [ProtoMember(5)]
        public PlayingStatus PlayingStatus { get; set; }

        [ProtoMember(6)]
        public RoomInfo RoomInfo { get; set; }

        [ProtoMember(7)]
        public RoomType RoomType { get; set; }

        [ProtoMember(8)]
        public SngRoomInfo SngRoomInfo { get; set; }

        [ProtoMember(9)]
        public MttRoomInfo MttRoomInfo { get; set; }

        [ProtoMember(10)]
        public int RoomID { get; set; }

        [ProtoMember(11)]
        public RoomMode RoomMode { get; set; }

        [ProtoMember(12)]
        public PineRoomInfo PineRoomInfo { get; set; }

        [ProtoMember(13)]
        public PineRoomStatus PineRoomStatus { get; set; }
    }
}
