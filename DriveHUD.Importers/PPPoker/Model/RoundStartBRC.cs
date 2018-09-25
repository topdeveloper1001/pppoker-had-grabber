using ProtoBuf;

namespace DriveHUD.Importers.PPPoker.Model
{
    [ProtoContract]
    class RoundStartBRC
    {
        [ProtoMember(1)]
        public Street Street { get; set; }

        [ProtoMember(2)]
        public int[] Board { get; set; }
    }
}
