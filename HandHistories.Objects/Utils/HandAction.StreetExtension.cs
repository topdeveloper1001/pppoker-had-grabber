//-----------------------------------------------------------------------
// <copyright file="StreetExtension.cs" company="Ace Poker Solutions">
// Copyright © 2015 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using HandHistories.Objects.Cards;
using System.Collections.Generic;
using System.Linq;

namespace HandHistories.Objects.Actions
{
    public static class StreetExtension
    {
        public static IEnumerable<HandAction> Street(this IEnumerable<HandAction> actions, Street street)
        {
            return actions.Where(p => p.Street == street);
        }
    }

    public static class StringExtension
    {
        public static int IndexOfDigit(this string text)
        {
            int ix = text.IndexOfAny(new[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' });
            return ix;
        }

        public static int IndexOfDigit(this string text, int startingIndex)
        {
            int ix = text.IndexOfAny(new[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' }, startingIndex);
            return ix;
        }       
    }
}