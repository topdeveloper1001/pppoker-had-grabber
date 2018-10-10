using ProtoBuf;

namespace PPPokerHandGrabber.Importers.PPPoker.Model
{
    [ProtoContract]
    class ShowHandRSP
    {
        [ProtoMember(1)]
        public ShowHandInfo[] Info { get; set; }

        [ProtoMember(2)]
        public int[] WinnerSeatID { get; set; }
    }
}
