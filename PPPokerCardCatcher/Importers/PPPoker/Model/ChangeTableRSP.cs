using ProtoBuf;

namespace PPPokerCardCatcher.Importers.PPPoker.Model
{
    [ProtoContract]
    class ChangeTableRSP
    {
        [ProtoMember(1)]
        public int Code { get; set; }

        [ProtoMember(2)]
        public TableStatus TableStatus { get; set; }

        [ProtoMember(3)]
        public RoomStatus RoomStatus { get; set; }

        [ProtoMember(4)]
        public PlayingStatus PlayingStatus { get; set; }
    }
}
