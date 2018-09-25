using ProtoBuf;

namespace DriveHUD.Importers.PPPoker.Model
{
    [ProtoContract]
    class ActionNotifyBRC
    {
        [ProtoMember(1)]
        public int SeatID { get; set; }

        [ProtoMember(2)]
        public long ChipsToCall { get; set; } // Original: call_need_chips

        [ProtoMember(3)]
        public long MinChipIn { get; set; } // Original: min_chipin

        [ProtoMember(4)]
        public long MaxChipIn { get; set; } // Original: max_chipin
    }
}
