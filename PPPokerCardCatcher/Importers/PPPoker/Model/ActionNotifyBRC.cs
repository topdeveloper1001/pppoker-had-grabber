using ProtoBuf;

namespace PPPokerCardCatcher.Importers.PPPoker.Model
{
    [ProtoContract]
    class ActionNotifyBRC
    {
        [ProtoMember(1)]
        public int SeatID { get; set; }

        [ProtoMember(2)]
        public long CallNeedChips { get; set; }

        [ProtoMember(3)]
        public long MinChipIn { get; set; }

        [ProtoMember(4)]
        public long MaxChipIn { get; set; }
    }
}
