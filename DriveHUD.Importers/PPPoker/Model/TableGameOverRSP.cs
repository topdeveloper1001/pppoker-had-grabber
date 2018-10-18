using ProtoBuf;

namespace DriveHUD.Importers.PPPoker.Model
{
    [ProtoContract]
    class TableGameOverRSP
    {
        [ProtoMember(1)]
        public int Type { get; set; }
    }
}
