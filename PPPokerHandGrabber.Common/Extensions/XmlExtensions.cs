//-----------------------------------------------------------------------
// <copyright file="XmlExtensions.cs" company="Ace Poker Solutions">
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
using System.Linq;
using System.Xml.Linq;

namespace PPPokerHandGrabber.Common.Extensions
{
    public static class XmlExtensions
    {
        public static XElement GetFirstElement(this XDocument xDocument, string name)
        {
            return xDocument.Root.GetFirstElement(name);
        }

        public static XElement GetFirstElement(this XElement xElement, string name)
        {
            var element = xElement.Descendants(name).FirstOrDefault();

            if (element == null)
            {
                throw new InvalidOperationException(string.Format("Element {0} could not be found", name));
            }

            return element;
        }

        public static XElement GetFirstElementOrDefault(this XElement xElement, string name)
        {
            var element = xElement.Descendants(name).FirstOrDefault();
            return element;
        }

        public static bool HasElement(this XElement xElement, string name)
        {
            return xElement.Descendants(name).FirstOrDefault() != null;
        }
    }
}