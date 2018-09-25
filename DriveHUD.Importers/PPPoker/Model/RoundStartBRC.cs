using ProtoBuf;

namespace DriveHUD.Importers.PPPoker.Model
{
    // Original lua code of pppoker.net client suggests following names:
    // - RoundStage instead of Street
    // - Complete instead of Showdown
    enum Street
    {
        None,
        PreFlop,
        Flop,
        Turn,
        River,
        Showdown,
    }

    [ProtoContract]
    class RoundStartBRC
    {
        [ProtoMember(1)]
        public Street Street { get; set; }

        [ProtoMember(2)]
        public int[] Board { get; set; }
    }
}
