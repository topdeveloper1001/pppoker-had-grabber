//-----------------------------------------------------------------------
// <copyright file="NonLocalizableString.cs" company="Ace Poker Solutions">
// Copyright © 2018 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

namespace PPPokerHandGrabber.Common.Resources
{
    public class NonLocalizableString : ILocalizableString
    {
        public NonLocalizableString(string message, params object[] messageParams)
        {
            Message = message;
            MessageParams = messageParams;
        }

        public string Message
        {
            get;
            private set;
        }

        public object[] MessageParams
        {
            get;
            private set;
        }

        public override string ToString()
        {
            return ToString(null);
        }

        public string ToString(System.Globalization.CultureInfo cultureInfo)
        {
            if (MessageParams == null || MessageParams.Length == 0)
            {
                return Message;
            }
            else
            {
                return string.Format(Message, MessageParams);
            }
        }
    }
}