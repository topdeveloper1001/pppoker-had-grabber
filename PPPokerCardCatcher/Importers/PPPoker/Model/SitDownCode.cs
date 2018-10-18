namespace PPPokerCardCatcher.Importers.PPPoker.Model
{
    enum SitDownCode
    {
        OK,
        WaitAuth,
        ErrorSeatID = -4,
        ErrorAlreadySitted = -3,
        ErrorNotEmpty = -2,
        ErrorMoney = -1,
        ErrorAlreadyStarted = -5,
        ErrorWaitlistNotEmpty = -6,
        ErrorClub = -7,
        ErrorGps = -8,
        ErrorIP = -9,
        ErrorGpsInvalid = -10,
        ErrorTicket = -11,
    }
}
