using ProtoBuf;

namespace PPPokerCardCatcher.Importers.PPPoker.Model
{
    [ProtoContract]
    class ChipsBackBRC
    {
        [ProtoMember(1)]
        public int SeatID { get; set; }

        [ProtoMember(2)]
        public long Chips { get; set; }
    }
}
