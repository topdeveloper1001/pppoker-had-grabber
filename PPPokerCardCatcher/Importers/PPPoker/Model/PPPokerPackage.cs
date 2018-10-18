//-----------------------------------------------------------------------
// <copyright file="PPPokerPackage.cs" company="Ace Poker Solutions">
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

namespace PPPokerCardCatcher.Importers.PPPoker.Model
{
    internal class PPPokerPackage
    {
        public DateTime Timestamp { get; set; }

        public PackageType PackageType { get; set; }

        public int ClientPort { get; set; }

        public PackageDirection Direction { get; set; }

        public byte[] Body { get; set; }

#if DEBUG
        public override string ToString()
        {
            return $"{PackageType}: {Timestamp}; BodyLength: {Body.Length}";
        }
#endif
    }
}
