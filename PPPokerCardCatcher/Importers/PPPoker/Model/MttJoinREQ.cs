using ProtoBuf;

namespace PPPokerCardCatcher.Importers.PPPoker.Model
{
    [ProtoContract]
    class MttJoinREQ
    {
        [ProtoMember(1)]
        public int RoomID { get; set; }

        [ProtoMember(2)]
        public bool Join { get; set; }

        [ProtoMember(3)]
        public int ClubID { get; set; }
    }
}
