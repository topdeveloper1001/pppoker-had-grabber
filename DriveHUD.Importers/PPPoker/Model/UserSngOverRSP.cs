using ProtoBuf;

namespace DriveHUD.Importers.PPPoker.Model
{
    [ProtoContract]
    class UserSngOverRSP
    {
        [ProtoMember(1)]
        public int Rank { get; set; }

        [ProtoMember(2)]
        public int Money { get; set; }

        [ProtoMember(3)]
        public string[] ItemReward { get; set; }

        [ProtoMember(4)]
        public int MttScore { get; set; }
    }
}
