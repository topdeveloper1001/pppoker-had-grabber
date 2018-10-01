using ProtoBuf;

namespace DriveHUD.Importers.PPPoker.Model
{
    [ProtoContract]
    class StandUpRSP
    {
        [ProtoMember(1)]
        public StandUpCode Code { get; set; }

        [ProtoMember(2)]
        public string Reason { get; set; }
    }
}
