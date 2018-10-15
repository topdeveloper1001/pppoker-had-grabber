using PPPokerCardCatcher.Entities;
using System;

namespace HandHistories.Objects.Hand
{
    public class HandParseError
    {
        public HandParseError(string handText, EnumPokerSites site, Exception ex)
        {
            HandText = handText;
            Site = site;
            Exception = ex;
        }

        public string HandText { get; private set; }

        public EnumPokerSites Site { get; set; }

        public Exception Exception { get; private set; }
    }
}