using ProtoBuf;

namespace PPPokerHandGrabber.Importers.PPPoker.Model
{
    [ProtoContract]
    class DealerInfoRSP
    {
        [ProtoMember(1)]
        public int Dealer { get; set; }

        [ProtoMember(2)]
        public int SmallBlind { get; set; }

        [ProtoMember(3)]
        public int BigBlind { get; set; }

        [ProtoMember(4)]
        public StartInfo[] Stacks { get; set; }

        [ProtoMember(5)]
        public string GameID { get; set; }
    }
}
