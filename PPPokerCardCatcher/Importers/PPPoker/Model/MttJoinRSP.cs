using ProtoBuf;

namespace PPPokerCardCatcher.Importers.PPPoker.Model
{
    [ProtoContract]
    class MttJoinRSP
    {
        [ProtoMember(1)]
        public int Code { get; set; }

        [ProtoMember(2)]
        public bool Join { get; set; }

        [ProtoMember(3)]
        public int RoomID { get; set; }
    }
}
