namespace DriveHUD.Importers.PPPoker.Model
{
    enum ActionType
    {
        None,
        Fold,
        Check,
        Call,
        Raise,
        Wait,
        Sit, // Original: sited = ???
        Bet,
        SmallBlind,
        BigBlind,
        Ante,
        ForceBigBlind,
        SystemFold,
        SystemCheck,
        Straddle,
        Pot,
    }
}
