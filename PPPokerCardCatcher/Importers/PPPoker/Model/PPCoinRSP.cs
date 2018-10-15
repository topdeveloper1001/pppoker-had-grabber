using ProtoBuf;

namespace PPPokerCardCatcher.Importers.PPPoker.Model
{
    [ProtoContract]
    class PPCoinRSP
    {
        [ProtoMember(1)]
        public int ClubID { get; set; }

        [ProtoMember(2)]
        public long PPCoin { get; set; }

        [ProtoMember(3)]
        public int LeagueID { get; set; }
    }
}
