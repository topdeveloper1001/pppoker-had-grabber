//-----------------------------------------------------------------------
// <copyright file="ProtectedLoggerConfiguration.cs" company="Ace Poker Solutions">
// Copyright © 2018 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

namespace PPPokerCardCatcher.Importers
{
    /// <summary>
    /// Logger configuration
    /// </summary>
    internal class ProtectedLoggerConfiguration
    {
        public string LogDirectory { get; set; }

        public string LogTemplate { get; set; }

        public string LogCleanupTemplate { get; set; }

        public string DateFormat { get; set; }

        public string DateTimeFormat { get; set; }

        public string PublicKey { get; set; }

        public int MessagesInBuffer { get; set; }
    }
}