using ProtoBuf;

namespace PPPokerCardCatcher.Importers.PPPoker.Model
{
    [ProtoContract]
    class WaitListUser
    {
        [ProtoMember(1)]
        public long UserID { get; set; }

    }
}
