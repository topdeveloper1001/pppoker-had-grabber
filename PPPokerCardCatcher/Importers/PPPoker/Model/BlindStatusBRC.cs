using ProtoBuf;

namespace PPPokerCardCatcher.Importers.PPPoker.Model
{
    [ProtoContract]
    class BlindStatusBRC
    {
        [ProtoMember(1)]
        public long Blind { get; set; }

        [ProtoMember(2)]
        public int Time { get; set; }

        [ProtoMember(3)]
        public long NextBlind { get; set; }

        [ProtoMember(4)]
        public long CurBlind { get; set; }

        [ProtoMember(5)]
        public bool Started { get; set; }

        [ProtoMember(6)]
        public long Ante { get; set; }

        [ProtoMember(7)]
        public long NextAnte { get; set; }
    }
}
