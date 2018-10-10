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
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PPPokerHandGrabber.Common.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveEOL(this string text)
        {
            return text.Replace("\r", string.Empty).Replace("\n", string.Empty);
        }

        public static string RemoveWhitespace(this string text)
        {
            return new string(text.Where(c => !char.IsWhiteSpace(c)).ToArray());
        }

        public static string RemoveInvalidFileNameChars(this string text)
        {
            return Path.GetInvalidFileNameChars().Aggregate(text, (current, c) => current.Replace(c.ToString(), string.Empty));
        }

        public static string RemoveControlChars(this string text)
        {
            return new string(text.Where(c => !char.IsControl(c)).ToArray());
        }

        public static IEnumerable<string> GetLines(this string str, bool removeEmptyLines = false)
        {
            using (var sr = new StringReader(str))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    if (removeEmptyLines && string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    yield return line;
                }
            }
        }

        public static byte[] ToBytes(this string input)
        {
            return input.Select(x => (byte)x).ToArray();
        }

        public static string Reverse(this string input)
        {
            return new string(input.ToCharArray().Reverse().ToArray());
        }

        public static string TakeBetween(this string input, string start, string end, bool useStartLastIndex = false, bool userEndLastIndex = false)
        {
            try
            {
                var startIndex = useStartLastIndex ?
                    input.LastIndexOf(start, StringComparison.Ordinal) :
                    input.IndexOf(start, StringComparison.Ordinal);

                if (startIndex < 0)
                {
                    return string.Empty;
                }

                startIndex += start.Length;

                var endIndex = !string.IsNullOrEmpty(end) ?
                    (userEndLastIndex ?
                        input.LastIndexOf(end, StringComparison.Ordinal) :
                        input.IndexOf(end, startIndex, StringComparison.Ordinal)) :
                    input.Length;

                if (endIndex <= startIndex)
                {
                    return string.Empty;
                }

                return input.Substring(startIndex, endIndex - startIndex).Trim();
            }
            catch
            {
                return string.Empty;
            }
        }

        public static bool ContainsIgnoreCase(this string input, string value)
        {
            return input.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}