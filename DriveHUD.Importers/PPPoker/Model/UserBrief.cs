using ProtoBuf;

namespace DriveHUD.Importers.PPPoker.Model
{
    [ProtoContract]
    class UserBrief
    {
        [ProtoMember(1)]
        public long Uid { get; set; }

        [ProtoMember(2)]
        public string Name { get; set; }

        [ProtoMember(3)]
        public string IconUrl { get; set; }
    }
}
