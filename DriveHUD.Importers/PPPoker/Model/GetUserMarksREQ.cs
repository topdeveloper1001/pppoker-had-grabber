using ProtoBuf;

namespace DriveHUD.Importers.PPPoker.Model
{
    [ProtoContract]
    class GetUserMarksREQ
    {
        [ProtoMember(1)]
        public long Uid { get; set; }

        [ProtoMember(2)]
        public long[] MarkUid { get; set; }
    }
}
