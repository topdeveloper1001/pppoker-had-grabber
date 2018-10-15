//-----------------------------------------------------------------------
// <copyright file="LicenseInfo.cs" company="Ace Poker Solutions">
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
using PPPokerCardCatcher.Common.Utils;
using System;
using System.Linq;

namespace PPPokerCardCatcher.Licensing
{
    /// <summary>
    /// License information class
    /// </summary>
    internal class LicenseInfo : ILicenseInfo
    {
        private const string cashLimitKey = "cashlimit";
        private const string tournamentLimitKey = "tournamentlimit";
        private const string nolimitKey = "nolimit";

        private readonly SecureLicense license;

        private readonly LicenseType licenseType;

        public LicenseInfo(SecureLicense license, LicenseType licenseType)
        {
            this.license = license;
            this.licenseType = licenseType;

            if (IsTrial)
            {
                var timeMonitor = license.GetTimeMonitor();
                ExpiryDate = DateTime.Now + timeMonitor.TimeRemaining;
            }

            InitializeLimits();
        }

        public LicenseType LicenseType
        {
            get
            {
                return licenseType;
            }
        }

        public LicenseSubType LicenseSubType
        {
            get
            {
                return GetLicenseSubType(Serial);
            }
        }

        public SecureLicense License
        {
            get
            {
                return license;
            }
        }

        public bool IsRegistered
        {
            get
            {
                return license != null && !license.IsTrial;
            }
        }

        public bool IsTrial
        {
            get
            {
                return license != null && license.SerialNumber != null && (license.SerialNumber.StartsWith("PPT", StringComparison.InvariantCulture) || licenseType == LicenseType.Trial);
            }
        }

        public bool IsTrialExpired
        {
            get
            {
                var hasExpiredErrorCode = ValidationException != null ?
                    ValidationException.Message.Contains("LCS_EXP") || Utils.GetErrorCodes(ValidationException).Any(x => x.Equals("E_TimeExpired")) :
                    false;

                return license != null && license.IsTrial && hasExpiredErrorCode;
            }
        }

        private string serial = string.Empty;

        public string Serial
        {
            get
            {
                return license != null ? license.SerialNumber : serial;
            }
            set
            {
                serial = value;
            }
        }

        public TimeSpan TimeRemaining
        {
            get
            {
                var currentTime = DateTime.Now;
                var timeRemaining = ExpiryDate > currentTime ? ExpiryDate - currentTime : TimeSpan.Zero;
                return timeRemaining;
            }
        }

        public DateTime ExpiryDate
        {
            get;
            set;
        }

        public bool IsExpiringSoon
        {
            get
            {
                return IsRegistered && !IsTrial && TimeRemaining < TimeSpan.FromDays(30);
            }
        }

        public bool IsExpired
        {
            get;
            set;
        }

        public NoLicenseException ValidationException
        {
            get;
            set;
        }

        public decimal TournamentLimit
        {
            get;
            private set;
        }

        public int CashLimit
        {
            get;
            private set;
        }

        private static LicenseSubType GetLicenseSubType(string serial)
        {
            if (string.IsNullOrWhiteSpace(serial) || serial.Length < 3)
            {
                return LicenseSubType.None;
            }

            switch (serial[2])
            {
                case 'S':
                    return LicenseSubType.Standard;
                case 'P':
                    return LicenseSubType.Pro;
                case 'T':
                    return LicenseSubType.Trial;
                default:
                    return LicenseSubType.None;
            }
        }

        /// <summary>
        /// Initialize license limits
        /// </summary>
        private void InitializeLimits()
        {
            if (license == null || !license.Values.ContainsKey(cashLimitKey) || !license.Values.ContainsKey(tournamentLimitKey))
            {
                return;
            }

            var cashLimitText = license.Values[cashLimitKey];
            var tournamentLimitText = license.Values[tournamentLimitKey];

            int cashLimit = 0;

            if (cashLimitText.Equals(nolimitKey))
            {
                CashLimit = int.MaxValue;
            }
            else if (int.TryParse(cashLimitText, out cashLimit))
            {
                CashLimit = cashLimit;
            }

            decimal tournamentLimit = 0;

            if (tournamentLimitText.Equals(nolimitKey))
            {
                TournamentLimit = decimal.MaxValue;
            }
            else if (decimal.TryParse(tournamentLimitText, out tournamentLimit))
            {
                TournamentLimit = tournamentLimit;
            }
        }
    }
}