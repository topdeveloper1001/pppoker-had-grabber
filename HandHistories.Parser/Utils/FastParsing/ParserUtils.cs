//-----------------------------------------------------------------------
// <copyright file="ParserUtils.cs" company="Ace Poker Solutions">
// Copyright © 2015 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using DriveHUD.Common.Extensions;
using DriveHUD.Common.Log;
using HandHistories.Objects.Actions;
using HandHistories.Objects.Cards;
using HandHistories.Objects.GameDescription;
using HandHistories.Objects.Hand;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace HandHistories.Parser.Utils.FastParsing
{
    public static class ParserUtils
    {
        public static TournamentSpeed ParseTournamentSpeed(string input)
        {
            if (input.IndexOf("Super Turbo", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                return TournamentSpeed.SuperTurbo;
            }

            if (input.IndexOf("Hyper Turbo", StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                  input.IndexOf("Hyper-Turbo", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                return TournamentSpeed.HyperTurbo;
            }

            if (input.IndexOf("Turbo", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                return TournamentSpeed.Turbo;
            }

            return TournamentSpeed.Regular;
        }

        public static TournamentSpeed? ParseNullableTournamentSpeed(string input, TournamentSpeed? defaultValue = TournamentSpeed.Regular)
        {
            if (input.IndexOf("Super Turbo", StringComparison.InvariantCultureIgnoreCase) > 0)
            {
                return TournamentSpeed.SuperTurbo;
            }

            if (input.IndexOf("Hyper Turbo", StringComparison.InvariantCultureIgnoreCase) > 0 ||
                  input.IndexOf("Hyper-Turbo", StringComparison.InvariantCultureIgnoreCase) > 0)
            {
                return TournamentSpeed.HyperTurbo;
            }

            if (input.IndexOf("Turbo", StringComparison.InvariantCultureIgnoreCase) > 0)
            {
                return TournamentSpeed.Turbo;
            }

            return defaultValue;
        }

        private static readonly Regex MoneyRegex = new Regex(@"^(?<currency1>[^\d\.]+)?\s?(?<money>\d+(?:\.\d+)?)\s?(?<currency2>[^\.\d]+)?$", RegexOptions.Compiled);

        public static bool TryParseMoney(string moneyText, out decimal money, out Currency currency, NumberFormatInfo numberFormatInfo = null)
        {
            if (numberFormatInfo == null)
            {
                numberFormatInfo = new NumberFormatInfo
                {
                    NegativeSign = "-",
                    CurrencyDecimalSeparator = ".",
                    CurrencyGroupSeparator = ",",
                    CurrencySymbol = "$"
                };
            }

            money = 0;
            currency = Currency.USD;

            var match = MoneyRegex.Match(moneyText);

            if (!match.Success)
            {
                return false;
            }

            if (!decimal.TryParse(match.Groups["money"].Value, NumberStyles.AllowCurrencySymbol | NumberStyles.Number, numberFormatInfo, out money))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Parse money text into money value (we expect that only 2 number are allowed after decimal separator)
        /// </summary>
        /// <param name="moneyText"></param>
        /// <returns></returns>
        public static decimal ParseMoney(string moneyText)
        {
            var money = 0m;

            if (!TryParseMoney(moneyText, out money))
            {
                throw new FormatException($"{moneyText} is not in the correct format");
            }

            return money;
        }

        public static bool TryParseMoney(string moneyText, out decimal money)
        {
            money = 0m;

            // Char #65533 present on TowerTorneos and WSOP hand history.
            moneyText = moneyText.RemoveWhitespace().Replace(",", ".").Replace(((char)65533).ToString(), "");

            var lastIndexOfDot = moneyText.LastIndexOf('.');

            if (lastIndexOfDot > 0)
            {
                if ((moneyText.Length > (lastIndexOfDot + 3)) && char.IsNumber(moneyText[lastIndexOfDot + 3]))
                {
                    moneyText = moneyText.Replace(".", string.Empty);
                }
                else
                {

                    var indexOfDot = moneyText.IndexOf(".");

                    while (indexOfDot < lastIndexOfDot)
                    {
                        moneyText = moneyText.Remove(indexOfDot, 1);
                        indexOfDot = moneyText.IndexOf(".", indexOfDot);
                        lastIndexOfDot--;
                    }
                }
            }

            var match = MoneyRegex.Match(moneyText);

            if (!match.Success)
            {
                return false;
            }

            money = decimal.Parse(match.Groups["money"].Value, NumberStyles.AllowCurrencySymbol | NumberStyles.Number, CultureInfo.InvariantCulture);

            return true;
        }

        private static string[] dateTimeCultures = new[] { "en-US", "ru-RU" };

        public static DateTime ParseDate(string dateText)
        {
            var cultures = dateTimeCultures.Select(x => new CultureInfo(x)).ToArray();

            DateTime dateTime;

            if (DateTime.TryParse(dateText, out dateTime))
            {
                return dateTime;
            }

            foreach (var culture in cultures)
            {
                if (DateTime.TryParse(dateText, culture, DateTimeStyles.None, out dateTime))
                {
                    return dateTime;
                }
            }

            throw new FormatException($"Unsupported date format: dateText");
        }

        /// <summary>
        /// Converts cards to the range of cards (e.g. AsKd -> AKo, AsAc -> AA, AsKs -> AKs)
        /// </summary>
        /// <param name="cards">Cards to convert</param>
        /// <returns>The range of cards</returns>
        public static string ConvertToCardRange(string cards)
        {
            if (string.IsNullOrWhiteSpace(cards) || cards.Length != 4)
            {
                return string.Empty;
            }

            try
            {
                var card1 = new Card(cards[0], cards[1]);
                var card2 = new Card(cards[2], cards[3]);

                if (!card1.IsValid() || !card2.IsValid() || (card1 == card2))
                {
                    return string.Empty;
                }

                if (card1.Rank == card2.Rank)
                {
                    return $"{card1.Rank}{card2.Rank}";
                }

                if (card1.Suit == card2.Suit)
                {
                    return $"{card1.Rank}{card2.Rank}s";
                }

                return $"{card1.Rank}{card2.Rank}o";
            }
            catch (Exception e)
            {
                LogProvider.Log.Error(typeof(ParserUtils), "Could not convert cards to the range of cards", e);
            }

            return string.Empty;
        }       
    }
}