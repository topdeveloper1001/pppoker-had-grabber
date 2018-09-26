using ProtoBuf;

namespace DriveHUD.Importers.PPPoker.Model
{
    [ProtoContract]
    class WinnerRSP
    {
        [ProtoMember(1)]
        public WinningInfo[] Winner { get; set; }

        [ProtoMember(2)]
        public WinningProfit[] Profit { get; set; }

        [ProtoMember(3)]
        public RabbitCard Rabbit { get; set; }
    }
}
