using ProtoBuf;

namespace PPPokerCardCatcher.Importers.PPPoker.Model
{
    [ProtoContract]
    class RabbitCard
    {
        [ProtoMember(1)]
        public int[] RabbitCards { get; set; } // Original name: rabbit_card

        [ProtoMember(2)]
        public RoundStage FinishStage { get; set; }
    }
}
