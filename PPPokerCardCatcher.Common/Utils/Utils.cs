//-----------------------------------------------------------------------
// <copyright file="Utils.cs" company="Ace Poker Solutions">
// Copyright © 2015 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using DeployLX.Licensing.v5;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PPPokerCardCatcher.Common.Utils
{
    /// <summary>
    /// Utilities
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Validate email
        /// </summary>
        /// <param name="strIn">Input string</param>
        /// <returns>True if email is valid, otherwise - false</returns>
        public static bool IsValidEmail(string strIn)
        {
            if (String.IsNullOrEmpty(strIn))
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(strIn,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Get error codes from licensing exception
        /// </summary>
        /// <param name="e">Exception</param>
        /// <returns>List of error codes</returns>
        public static List<string> GetErrorCodes(NoLicenseException e)
        {
            var errorCodes = new List<string>();

            foreach (ValidationRecord validationRecord in e.ValidationRecords)
            {
                foreach (ValidationRecord subRecord in validationRecord.SubRecords)
                {
                    errorCodes.Add(subRecord.ErrorCode);
                }

                errorCodes.Add(validationRecord.ErrorCode);
            }

            return errorCodes;
        }
    }
}