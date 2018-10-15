using ProtoBuf;

namespace PPPokerCardCatcher.Importers.PPPoker.Model
{
    [ProtoContract]
    class AnteBRC
    {
        [ProtoMember(1)]
        public AnteInfo[] Info { get; set; }
    }
}
