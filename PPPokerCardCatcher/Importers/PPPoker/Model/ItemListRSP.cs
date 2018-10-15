using ProtoBuf;

namespace PPPokerCardCatcher.Importers.PPPoker.Model
{
    [ProtoContract]
    class ItemListRSP
    {
        [ProtoMember(1)]
        public ItemInfo[] Info { get; set; }
    }
}
