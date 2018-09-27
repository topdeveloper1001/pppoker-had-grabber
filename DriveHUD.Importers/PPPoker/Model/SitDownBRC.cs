using ProtoBuf;

namespace DriveHUD.Importers.PPPoker.Model
{
    [ProtoContract]
    class SitDownBRC
    {
        [ProtoMember(1)]
        public int SeatID { get; set; }

        [ProtoMember(2)]
        public long Chips { get; set; }

        [ProtoMember(3)]
        public UserInfo Player { get; set; } // Original name: brief

        [ProtoMember(4)]
        public SeatStatus Status { get; set; }
    }
}
