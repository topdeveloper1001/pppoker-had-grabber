//-----------------------------------------------------------------------
// <copyright file="PPPokerPacketManager.cs" company="Ace Poker Solutions">
// Copyright © 2018 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using PPPokerCardCatcher.Importers.TcpBased;
using PPPokerCardCatcher.Importers.PPPoker.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PPPokerCardCatcher.Importers.PPPoker
{
    internal class PPPokerPacketManager : PacketManager<PPPokerPackage>
    {
        private const int packetHeaderLength = 4;

        protected override int PacketHeaderLength
        {
            get
            {
                return packetHeaderLength;
            }
        }

        public override bool IsStartingPacket(byte[] bytes)
        {
            // Synchronize with TCP stream using "pb." marker
            return bytes.Length > 8
                && bytes[6] == 0x70
                && bytes[7] == 0x62
                && bytes[8] == 0x2E;
        }

        public override int ReadPacketLength(byte[] bytes)
        {
            if (bytes.Length < packetHeaderLength)
            {
                throw new ArgumentException(nameof(bytes), $"Packet must have more than {packetHeaderLength} bytes");
            }

            var numArray = bytes.Take(packetHeaderLength).ToArray();

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(numArray);
            }

            // PPPoker first four bytes contain packet length excluding the header itself, i. e. minus four bytes
            return BitConverter.ToInt32(numArray, 0) + packetHeaderLength;
        }

        public override bool TryParse(CapturedPacket capturedPacket, out IList<PPPokerPackage> packages)
        {
            bool result = base.TryParse(capturedPacket, out packages);

            // Packages don't contain any client application ID, we're going to use client port for this
            var direction = capturedPacket.Source.Port == PPPConstants.Port ? PackageDirection.Incoming : PackageDirection.Outgoing;
            var clientPort = direction == PackageDirection.Incoming ? capturedPacket.Destination.Port : capturedPacket.Source.Port;

            foreach (var package in packages)
            {
                package.Direction = direction;
                package.ClientPort = clientPort;
            }

            return result;
        }
    }
}