//-----------------------------------------------------------------------
// <copyright file="ILicenseInfo.cs" company="Ace Poker Solutions">
// Copyright © 2015 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using DeployLX.Licensing.v5;
using System;

namespace PPPokerCardCatcher.Licensing
{
    internal interface ILicenseInfo : IBaseLicenseInfo
    {
        /// <summary>
        /// License 
        /// </summary>
        SecureLicense License { get; }

        /// <summary>
        /// Type of license
        /// </summary>
        LicenseType LicenseType { get; }      

        /// <summary>
        /// True if user has valid serial (trial or common)
        /// </summary>
        bool IsRegistered { get; }

        /// <summary>
        /// True if License is trial
        /// </summary>
        bool IsTrial { get; }

        /// <summary>
        /// True if trial has expired
        /// </summary>
        bool IsTrialExpired { get; }      

        /// <summary>
        /// Expiry date
        /// </summary>
        DateTime ExpiryDate { get; set; }

        /// <summary>
        /// True if license is expiring in predefined period of time
        /// </summary>
        bool IsExpiringSoon { get; }

        /// <summary>
        /// Determine when license expired
        /// </summary>
        bool IsExpired { get; set; }
     
        /// <summary>
        /// Exception in validation
        /// </summary>
        NoLicenseException ValidationException { get; set; }

        /// <summary>
        /// Tournament buy-in limit
        /// </summary>
        decimal TournamentLimit { get; }

        /// <summary>
        /// Cash limit
        /// </summary>
        int CashLimit { get; }
    }
}