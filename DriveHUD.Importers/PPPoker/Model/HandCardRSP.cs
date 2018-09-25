using ProtoBuf;

namespace DriveHUD.Importers.PPPoker.Model
{
    [ProtoContract]
    class HandCardRSP
    {
        [ProtoMember(1)]
        public int Card1 { get; set; }

        [ProtoMember(2)]
        public int Card2 { get; set; }

        [ProtoMember(3)]
        public int Card3 { get; set; }

        [ProtoMember(4)]
        public int Card4 { get; set; }
    }
}
