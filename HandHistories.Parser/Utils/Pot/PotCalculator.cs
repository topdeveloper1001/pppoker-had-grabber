//-----------------------------------------------------------------------
// <copyright file="PotCalculator.cs" company="Ace Poker Solutions">
// Copyright © 2015 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using HandHistories.Objects.Hand;
using System;
using System.Linq;

namespace HandHistories.Parser.Utils.Pot
{
    internal static class PotCalculator
    {
        public static decimal CalculateTotalPot(HandHistory hand)
        {
            var sum = hand.HandActions.Where(p => !p.IsWinningsAction).Sum(a => a.Amount);

            return Math.Abs(sum);

        }

        public static decimal CalculateRake(HandHistory hand)
        {
            var totalCollected = hand.HandActions
                .Where(p => p.IsWinningsAction)
                .Sum(p => p.Amount);

            return hand.TotalPot.Value - totalCollected;
        }
    }
}