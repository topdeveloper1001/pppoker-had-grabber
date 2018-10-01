using ProtoBuf;

namespace DriveHUD.Importers.PPPoker.Model
{
    [ProtoContract]
    class PreActionRSP
    {
        [ProtoMember(1)]
        public int Code { get; set; }

        [ProtoMember(2)]
        public int Type { get; set; }

        [ProtoMember(3)]
        public long Chips { get; set; }

    }
}
