using ProtoBuf;

namespace DriveHUD.Importers.PPPoker.Model
{
    [ProtoContract]
    class InsurancePoolInfo
    {
        [ProtoMember(1)]
        public int PoolID { get; set; }

        [ProtoMember(2)]
        public long AlreadyBuyin { get; set; }
    }
}
