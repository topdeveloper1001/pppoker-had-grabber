using ProtoBuf;

namespace DriveHUD.Importers.PPPoker.Model
{
    [ProtoContract]
    class AnteBRC
    {
        [ProtoMember(1)]
        public AnteInfo[] Info { get; set; }
    }
}
