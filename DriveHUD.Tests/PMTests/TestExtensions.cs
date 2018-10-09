//-----------------------------------------------------------------------
// <copyright file="TestUtils.cs" company="Ace Poker Solutions">
// Copyright © 2018 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using System;
using System.Linq;

namespace PMCatcher.Tests
{
    internal static class TestExtensions
    {
        public static string ToHexString(this byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", " ");
        }

        public static byte[] FromHexStringToBytes(this string str)
        {
            return (
                from x in Enumerable.Range(0, str.Length)
                where x % 3 == 0
                select Convert.ToByte(str.Substring(x, 2), 16)).ToArray();
        }
    }
}