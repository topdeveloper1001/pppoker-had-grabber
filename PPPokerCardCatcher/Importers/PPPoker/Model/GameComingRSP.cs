using ProtoBuf;

namespace PPPokerCardCatcher.Importers.PPPoker.Model
{
    [ProtoContract]
    class GameComingRSP
    {
        [ProtoMember(1)]
        public int Left { get; set; }
    }
}
