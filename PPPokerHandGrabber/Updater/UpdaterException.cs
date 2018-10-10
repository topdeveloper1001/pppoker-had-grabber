//-----------------------------------------------------------------------
// <copyright file="UpdaterException.cs" company="Ace Poker Solutions">
// Copyright © 2015 Ace Poker Solutions. All Rights Reserved.
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

namespace PPPokerHandGrabber.Updater.Core
{
    internal class UpdaterException : Exception
    {
        private UpdaterError errorCode;

        public UpdaterException(UpdaterError errorCode) :
            base(CommonResourceManager.Instance.GetEnumResource(errorCode))
        {
            this.errorCode = errorCode;
        }

        public UpdaterException(UpdaterError errorCode, Exception innerException) :
            base(CommonResourceManager.Instance.GetEnumResource(errorCode), innerException)
        {
            this.errorCode = errorCode;
        }

        public UpdaterError ErrorCode
        {
            get
            {
                return errorCode;
            }
        }
    }
}
