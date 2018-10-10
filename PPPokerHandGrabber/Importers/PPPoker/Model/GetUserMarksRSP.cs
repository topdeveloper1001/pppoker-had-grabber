using ProtoBuf;

namespace PPPokerHandGrabber.Importers.PPPoker.Model
{
    [ProtoContract]
    class GetUserMarksRSP
    {
        [ProtoMember(1)]
        public long Uid { get; set; }

        [ProtoMember(2)]
        public UserMarkInfo[] Mark { get; set; }
    }
}
