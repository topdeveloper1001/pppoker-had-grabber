using ProtoBuf;

namespace PPPokerCardCatcher.Importers.PPPoker.Model
{
    [ProtoContract]
    class SngJoinRSP
    {
        [ProtoMember(1)]
        public int Code { get; set; }

        [ProtoMember(2)]
        public int RoomID { get; set; }

        [ProtoMember(3)]
        public string TempID { get; set; }
    }
}
