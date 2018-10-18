using ProtoBuf;

namespace PPPokerCardCatcher.Importers.PPPoker.Model
{
    [ProtoContract]
    class RoundOverBRC
    {
        [ProtoMember(1)]
        public long[] Pool { get; set; }
    }
}
