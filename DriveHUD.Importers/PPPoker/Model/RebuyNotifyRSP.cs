using ProtoBuf;

namespace DriveHUD.Importers.PPPoker.Model
{
    [ProtoContract]
    class RebuyNotifyRSP
    {
        [ProtoMember(1)]
        public int LeftTime { get; set; }

        [ProtoMember(2)]
        public int SeatID { get; set; }
    }
}
