//-----------------------------------------------------------------------
// <copyright file="TimeZoneUtil.cs" company="Ace Poker Solutions">
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

namespace HandHistories.Parser.Utils.Time
{
    internal enum TimeZoneType
    {
        EST,
        PST,
        CET,
        GMT,
        GMT1,
        CEST,
        Current
    }

    internal static class TimeZoneUtil
    {
        public static DateTime ConvertDateTimeToUtc(DateTime dateTime, TimeZoneType timeZone)
        {
            DateTime convertedUtcTime = TimeZoneInfo.ConvertTimeToUtc(dateTime, GetTimeZoneInfo(timeZone));
            convertedUtcTime = DateTime.SpecifyKind(convertedUtcTime, DateTimeKind.Utc);

            return convertedUtcTime;
        }

        // Reference for time zones:
        // http://www.xiirus.net/articles/article-_net-convert-datetime-from-one-timezone-to-another-7e44y.aspx
        public static TimeZoneInfo GetTimeZoneInfo(TimeZoneType timeZoneTypes)
        {
            string timeZoneId = "NotSet";
            try
            {
                switch (timeZoneTypes)
                {
                    case TimeZoneType.EST:
                        timeZoneId = "Eastern Standard Time";
                        break;
                    case TimeZoneType.PST:
                        timeZoneId = "Pacific Standard Time";
                        break;
                    case TimeZoneType.CET:
                        timeZoneId = "Central European Standard Time";
                        break;
                    case TimeZoneType.GMT:
                        timeZoneId = "GMT Standard Time";
                        break;
                    case TimeZoneType.GMT1:
                        timeZoneId = "W. Europe Standard Time";
                        break;
                    case TimeZoneType.CEST:
                        timeZoneId = "Central European Summer Time";
                        break;
                    case TimeZoneType.Current:
                        timeZoneId = TimeZoneInfo.Local.Id;
                        break;
                    default:
                        throw new NotImplementedException("GetTimeZoneInfo: Not implemented for time zone " +
                                                          timeZoneTypes);
                }

                return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            }
            catch (TimeZoneNotFoundException)
            {
                throw;
            }
            catch (InvalidTimeZoneException)
            {
                throw;
            }
        }
    }
}