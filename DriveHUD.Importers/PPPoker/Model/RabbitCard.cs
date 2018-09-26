using ProtoBuf;

namespace DriveHUD.Importers.PPPoker.Model
{
    [ProtoContract]
    class RabbitCard
    {
        [ProtoMember(1)]
        public int[] RabbitCards { get; set; } // Original name: rabbit_card

        [ProtoMember(2)]
        public Round FinalStreet { get; set; } // Original name: finish_stage
    }
}
