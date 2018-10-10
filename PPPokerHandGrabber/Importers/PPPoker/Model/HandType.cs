namespace PPPokerHandGrabber.Importers.PPPoker.Model
{
    enum HandType
    {
        None,
        Fold = -1,
        HighCard = 1,
        Pair,
        TwoPairs,
        ThreOfAKind,
        Straight,
        Flush,
        FullHouse,
        FourOfAKind,
        StraightFlush,
        RoyalFlush
    }
}
