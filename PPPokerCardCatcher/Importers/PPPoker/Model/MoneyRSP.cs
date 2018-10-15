using ProtoBuf;

namespace PPPokerCardCatcher.Importers.PPPoker.Model
{
    [ProtoContract]
    class MoneyRSP
    {
        [ProtoMember(1)]
        public long Money { get; set; }
    }
}
