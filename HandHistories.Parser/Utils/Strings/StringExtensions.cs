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

using System;

namespace HandHistories.Parser.Utils.Strings
{
    internal static class StringExtensions
    {
        public static int LastIndexLoopsBackward(this string str, char c, int lastIndex)
        {
            for (int i = lastIndex; i >= 0; i--)
            {
                if (str[i] == c)
                {
                    return i;
                }
            }

            return -1;
        }

        public static bool FastEndsWith(this string str, string end)
        {
            return str.EndsWith(end, StringComparison.Ordinal);
        }
    }
}
