using ProtoBuf;

namespace PPPokerHandGrabber.Importers.PPPoker.Model
{
    [ProtoContract]
    class MttRewardInfo
    {
        [ProtoMember(1)]
        public long Chips { get; set; }

        [ProtoMember(2)]
        public string[] ItemName { get; set; }

        [ProtoMember(3)]
        public int Score { get; set; }
    }
}
