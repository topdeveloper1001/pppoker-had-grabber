//-----------------------------------------------------------------------
// <copyright file="LicenseType.cs" company="Ace Poker Solutions">
// Copyright © 2015 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using System.Reflection;

namespace PPPokerHandGrabber.Licensing
{
    /// <summary>
    /// Types of license
    /// </summary>
    [Obfuscation(Exclude = true)]
    internal enum LicenseType : short
    {
        Trial = 1,
        Holdem = 2,
        Omaha = 3,
        Combo = 4
    }

    internal static class LicenseTypeExtension
    {
        internal static string ToService(this LicenseType licenseType)
        {
            switch (licenseType)
            {
                case LicenseType.Combo:
                    return "Combo";
                case LicenseType.Holdem:
                    return "Holdem";
                case LicenseType.Omaha:
                    return "Omaha";
                case LicenseType.Trial:
                    return "Trial";
            }

            return null;
        }
    }
}