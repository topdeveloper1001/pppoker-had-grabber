//-----------------------------------------------------------------------
// <copyright file="EnumPokerSitesExtension.cs" company="Ace Poker Solutions">
// Copyright © 2015 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using DriveHUD.Entities;
using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace HandHistories.Parser.Utils.Extensions
{
    public static class EnumPokerSitesExtension
    {
        public static bool TryParse(string handText, out EnumPokerSites siteName)
        {
            siteName = EnumPokerSites.Unknown;

            if (string.IsNullOrEmpty(handText))
            {
                return false;
            }

            handText = handText.TrimStart();

            if (string.IsNullOrEmpty(handText))
            {
                return false;
            }

            if (handText.Length >= 50 && handText.IndexOf("PokerStars", 0, 50) > -1)
            {
                siteName = EnumPokerSites.PokerStars;
                return true;
            }

            if (handText.StartsWith("#Game No", StringComparison.OrdinalIgnoreCase))
            {
                if (handText.IndexOf("***** H", 0, 50) > -1)
                {
                    siteName = EnumPokerSites.PartyPoker;
                    return true;
                }

                siteName = EnumPokerSites.Poker888;
                return true;
            }

            if (handText.StartsWith("***** Hand History", StringComparison.OrdinalIgnoreCase)
                || handText.StartsWith("Game #", StringComparison.OrdinalIgnoreCase))
            {
                siteName = EnumPokerSites.PartyPoker;
                return true;
            }

            if (handText.Contains("Hand#"))
            {
                siteName = EnumPokerSites.Horizon;
                return true;
            }

            if (handText.Contains("Winamax Poker"))
            {
                siteName = EnumPokerSites.Winamax;
                return true;
            }

            if (handText.StartsWith("***** Cassava Tournament Summary *****", StringComparison.OrdinalIgnoreCase))
            {
                siteName = EnumPokerSites.Poker888;
                return true;
            }

            if (handText.StartsWith("Game started", StringComparison.OrdinalIgnoreCase) ||
                handText.StartsWith("<Game Information>", StringComparison.OrdinalIgnoreCase) ||
                handText.StartsWith("Game Hand", StringComparison.OrdinalIgnoreCase))
            {
                siteName = EnumPokerSites.WinningPokerNetwork;
                return true;
            }

            // xml file
            if (handText[0] == '<')
            {
                try
                {
                    var xElement = XElement.Parse(handText);

                    // iPoker starts with <session>
                    if (xElement.Name.LocalName.Equals("session", StringComparison.OrdinalIgnoreCase))
                    {
                        TryParseSite(xElement, "pokersite", out siteName, EnumPokerSites.IPoker);
                        return true;
                    }

                    // command hand starts with <HandHistory
                    if (xElement.Name.LocalName.Equals("HandHistory", StringComparison.OrdinalIgnoreCase))
                    {
                        return TryParseSite(xElement, "Site", out siteName);
                    }
                }
                catch (XmlException)
                {
                    return false;
                }
            }

            return false;
        }

        private static bool TryParseSite(XElement xElement, string siteTag, out EnumPokerSites pokerSite, EnumPokerSites defaultSite = EnumPokerSites.Unknown)
        {
            pokerSite = defaultSite;

            var siteNode = xElement.Descendants(siteTag).FirstOrDefault();

            if (siteNode == null)
            {
                return false;
            }

            if (!Enum.TryParse(siteNode.Value, out pokerSite))
            {
                return false;
            }

            return true;
        }
    }
}