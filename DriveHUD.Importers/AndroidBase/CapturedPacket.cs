//-----------------------------------------------------------------------
// <copyright file="CapturedPacket.cs" company="Ace Poker Solutions">
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
using System.Net;

namespace DriveHUD.Importers.AndroidBase
{
    internal class CapturedPacket
    {
        public byte[] Bytes { get; set; }

        public IPEndPoint Source { get; set; }

        public IPEndPoint Destination { get; set; }

        public DateTime CreatedTimeStamp { get; set; }

        public uint SequenceNumber { get; set; }

        public string IPDestination
        {
            get
            {
                return Destination.Address.ToString();
            }
        }

        public string IPSource
        {
            get
            {
                return Source.Address.ToString();
            }
        }

        public override string ToString()
        {
            return $"{Source}->{Destination}";
        }
    }
}