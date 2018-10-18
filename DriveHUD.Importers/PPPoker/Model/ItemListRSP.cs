using ProtoBuf;

namespace DriveHUD.Importers.PPPoker.Model
{
    [ProtoContract]
    class ItemListRSP
    {
        [ProtoMember(1)]
        public ItemInfo[] Info { get; set; }
    }
}
