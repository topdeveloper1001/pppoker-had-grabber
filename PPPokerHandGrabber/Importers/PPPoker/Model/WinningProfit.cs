using ProtoBuf;

namespace PPPokerHandGrabber.Importers.PPPoker.Model
{
    [ProtoContract]
    class WinningProfit
    {
        [ProtoMember(1)]
        public int SeatID { get; set; }

        [ProtoMember(2)]
        public long Chips { get; set; }

        [ProtoMember(3)]
        public long InsuranceChips { get; set; }
    }
}
