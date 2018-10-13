//-----------------------------------------------------------------------
// <copyright file="LicenseInfoViewModel.cs" company="Ace Poker Solutions">
// Copyright © 2015 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using PPPokerHandGrabber.Common.Wpf.Mvvm;
using PPPokerHandGrabber.Licensing;
using System;

namespace PPPokerHandGrabber.ViewModels
{
    public class LicenseInfoViewModel : ViewModelBase
    {
        private IBaseLicenseInfo licenseInfo;

        internal LicenseInfoViewModel(IBaseLicenseInfo licenseInfo)
        {
            this.licenseInfo = licenseInfo ?? throw new ArgumentNullException(nameof(licenseInfo));
        }

        public string Serial
        {
            get
            {
                return licenseInfo.Serial;
            }
        }

        public int DaysLeft
        {
            get
            {
                return licenseInfo.TimeRemaining.Days;
            }
        }
    }
}