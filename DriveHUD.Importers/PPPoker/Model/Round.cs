namespace DriveHUD.Importers.PPPoker.Model
{
    // Original lua code of pppoker.net client suggests following names:
    // - RoundStage instead of Round. We'll use Round instead of more common Street to avoid ambiguity with HandHistories.Objects.Cards.Street
    // - Complete instead of Showdown
    enum Round
    {
        None,
        PreFlop,
        Flop,
        Turn,
        River,
        Showdown,
    }
}
