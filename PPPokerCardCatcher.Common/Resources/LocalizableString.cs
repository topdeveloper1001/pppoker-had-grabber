//-----------------------------------------------------------------------
// <copyright file="LocalizableString.cs" company="Ace Poker Solutions">
// Copyright © 2018 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using System.Globalization;

namespace PPPokerCardCatcher.Common.Resources
{
    public class LocalizableString : ILocalizableString
    {
        public LocalizableString(string resourceMessageKey, params object[] resourceMessageParams)
        {
            ResourceMessageKey = resourceMessageKey;
            ResourceMessageParams = resourceMessageParams;
        }

        public string ResourceMessageKey { get; private set; }

        public object[] ResourceMessageParams { get; private set; }

        private readonly static CultureInfo defaultCultureInfo = new CultureInfo("en");

        public override string ToString()
        {
            return ToString(defaultCultureInfo);
        }

        public string ToString(CultureInfo cultureInfo)
        {
            if (string.IsNullOrEmpty(ResourceMessageKey))
            {
                return string.Empty;
            }

            string message = CommonResourceManager.Instance.GetResourceString(ResourceMessageKey, cultureInfo ?? defaultCultureInfo);

            if (string.IsNullOrEmpty(message))
            {
                return ResourceMessageKey;
            }

            if (ResourceMessageParams == null || ResourceMessageParams.Length == 0)
            {
                return message;
            }

            return string.Format(message, ResourceMessageParams);
        }

        public static string ToString(string resourceMessageKey, params object[] resourceMessageParams)
        {
            var localizableString = new LocalizableString(resourceMessageKey, resourceMessageParams);
            return localizableString.ToString();
        }
    }
}