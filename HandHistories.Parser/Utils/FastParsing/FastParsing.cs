//-----------------------------------------------------------------------
// <copyright file="FastInt.cs" company="Ace Poker Solutions">
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

namespace HandHistories.Parser.Utils.FastParsing
{
    internal static class FastInt
    {
        public static int Parse(string text, int startindex = 0)
        {
            int Value = 0;
            char currentChar = text[startindex++];
            bool negative = currentChar == '-';

            while (currentChar >= 0x30 && currentChar <= 0x39)
            {
                Value = (Value * 10) + currentChar - 0x30;
                if (startindex >= text.Length)
                {
                    break;
                }
                currentChar = text[startindex++];
            }

            return negative ? -Value : Value;
        }

        public static int Parse(char text, int? defaultValueIfNotParsed = null)
        {
            if (text >= 0x30 && text <= 0x39)
            {
                return text - 0x30;
            }

            if (defaultValueIfNotParsed.HasValue)
            {
                return defaultValueIfNotParsed.Value;
            }

            throw new ArgumentOutOfRangeException(text.ToString());
        }
    }
}