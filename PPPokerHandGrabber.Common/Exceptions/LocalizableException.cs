//-----------------------------------------------------------------------
// <copyright file="LocalizableException.cs" company="Ace Poker Solutions">
// Copyright © 2018 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using PPPokerHandGrabber.Common.Resources;
using System;
using System.Runtime.Serialization;

namespace PPPokerHandGrabber.Common.Exceptions
{
    /// <summary>
    /// Base class for all localizable exceptions in DH
    /// </summary>
    public abstract class LocalizableException : Exception
    {
        public LocalizableException()
           : base(null, null)
        {
        }

        public LocalizableException(ILocalizableString localizableString, Exception innerException)
            : base(null, innerException)
        {
            LocalizableString = localizableString;
        }

        public LocalizableException(SerializationInfo si, StreamingContext sc)
            : base(si, sc)
        {
        }

        public ILocalizableString LocalizableString { get; private set; }

        public override string Message
        {
            get
            {
                if (LocalizableString != null)
                {
                    return LocalizableString.ToString();
                }

                return null;
            }
        }
    }
}