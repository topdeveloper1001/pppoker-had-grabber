//-----------------------------------------------------------------------
// <copyright file="CompositeKeyProvider.cs" company="Ace Poker Solutions">
// Copyright © 2015 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace PPPokerCardCatcher.Common.Resources
{
    public class CompositeKeyProvider : IResXKeyProvider
    {
        private const string NoKey = "<nokey>";

        public string BaseKey { get; set; }

        public CompositeKeyProvider()
        {
            BaseKey = NoKey;
        }

        public CompositeKeyProvider(string baseKey)
        {
            BaseKey = baseKey ?? NoKey;
        }

        public static string GetKey(string prefix, IEnumerable<object> parameters)
        {
            if (parameters == null || !parameters.Any())
            {
                return prefix;
            }

            var stringBuilder = new StringBuilder(prefix);

            foreach (object parameter in parameters)
            {
                if (parameter is CultureInfo)
                {
                    continue;
                }

                if (parameter != null)
                {
                    stringBuilder.AppendFormat("_{0}_{1}", parameter.GetType().Name, parameter);
                }
            }

            return stringBuilder.ToString();
        }

        public string ProvideKey(IEnumerable<object> parameters)
        {
            return GetKey(BaseKey, parameters);
        }

        public static string GetEnumKey(Enum enumValue)
        {
            return GetKey("Enum", new object[] { enumValue });
        }
    }
}