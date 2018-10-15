//-----------------------------------------------------------------------
// <copyright file="PHGInternalException.cs" company="Ace Poker Solutions">
// Copyright © 2015 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using PPPokerCardCatcher.Common.Resources;
using System;

namespace PPPokerCardCatcher.Common.Exceptions
{
    /// <summary>
    /// Class for internal PHG exceptions
    /// </summary>
    public class PCCInternalException : LocalizableException
    {
        public PCCInternalException(ILocalizableString message)
            : base(message, null)
        {
        }

        public PCCInternalException(ILocalizableString message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}