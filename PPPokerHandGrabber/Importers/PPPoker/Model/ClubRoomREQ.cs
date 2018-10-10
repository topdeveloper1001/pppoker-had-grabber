using ProtoBuf;

namespace PPPokerHandGrabber.Importers.PPPoker.Model
{
    [ProtoContract]
    class ClubRoomREQ
    {
        [ProtoMember(1)]
        public int ClubID { get; set; }

        [ProtoMember(2)]
        public int LeagueID { get; set; }
    }
}
