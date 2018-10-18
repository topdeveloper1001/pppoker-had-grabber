using ProtoBuf;

namespace DriveHUD.Importers.PPPoker.Model
{
    [ProtoContract]
    class WinningInfo
    {
        [ProtoMember(1)]
        public int SeatID { get; set; }

        [ProtoMember(2)]
        public int PoolID { get; set; }

        [ProtoMember(3)]
        public long Chips { get; set; }

        [ProtoMember(4)]
        public HandType Type { get; set; }

        [ProtoMember(5)]
        public int InsuranceChips { get; set; } // For some reason it's an "int", not a "long" type like in WinningProfit class

        [ProtoMember(6)]
        public long Uid { get; set; }

        [ProtoMember(7)]
        public long JackpotFee { get; set; }
    }
}
