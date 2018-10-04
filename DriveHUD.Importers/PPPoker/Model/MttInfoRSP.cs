using ProtoBuf;

namespace DriveHUD.Importers.PPPoker.Model
{
    [ProtoContract]
    class MttInfoRSP
    {
        [ProtoMember(1)]
        public int Code { get; set; }

        [ProtoMember(2)]
        public SngRoomInfo SngInfo { get; set; }

        [ProtoMember(3)]
        public MttRoomInfo MttInfo { get; set; }
    }
}
