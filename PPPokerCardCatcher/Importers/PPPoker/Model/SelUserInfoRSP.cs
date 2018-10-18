using ProtoBuf;

namespace PPPokerCardCatcher.Importers.PPPoker.Model
{
    [ProtoContract]
    class SelUserInfoRSP
    {
        [ProtoMember(1)]
        public UserBrief Brief { get; set; }

        [ProtoMember(2)]
        public string Country { get; set; }

        [ProtoMember(3)]
        public string Mail { get; set; }

        [ProtoMember(4)]
        public uint ValidMail { get; set; }

        [ProtoMember(5)]
        public bool NewUser { get; set; }

        [ProtoMember(6)]
        public bool ShowNovice { get; set; }
    }
}
