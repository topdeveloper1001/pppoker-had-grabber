using ProtoBuf;

namespace DriveHUD.Importers.PPPoker.Model
{
    [ProtoContract]
    class StandUpBRC
    {
        [ProtoMember(1)]
        public int SeatID { get; set; }
    }
}
