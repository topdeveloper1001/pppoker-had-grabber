//-----------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Ace Poker Solutions">
// Copyright © 2015 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using System.Globalization;

namespace HandHistories.Parser.Utils.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Removes any currency symbols before parsing
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static decimal ParseAmount(this string str)
        {
            str = str.Trim('£', '€', '$');
            return decimal.Parse(str, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Removes any currency symbols before parsing
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static decimal ParseAmount(this string str, NumberFormatInfo numberFormat)
        {
            str = str.Trim('£', '€', '$');
            return decimal.Parse(str, numberFormat);
        }

        static readonly char[] ParseWSTrimChars = new char[]
        {
            '£', '€', '$', ' ', (char)160
        };

        /// <summary>
        /// Removes any currency symbols and white-spaces before parsing
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static decimal ParseAmountWS(this string str)
        {
            str = str.Trim(ParseWSTrimChars);
            return decimal.Parse(str, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Removes any currency symbols and white-spaces before parsing
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static decimal ParseAmountWS(this string str, NumberFormatInfo numberFormat)
        {
            str = str.Trim(ParseWSTrimChars);
            return decimal.Parse(str, numberFormat);
        }
    }
}