using ProtoBuf;

namespace PPPokerCardCatcher.Importers.PPPoker.Model
{
    [ProtoContract]
    class OtherEnterRoomBRC
    {
        [ProtoMember(1)]
        public UserBrief User { get; set; }
    }
}
