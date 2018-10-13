//-----------------------------------------------------------------------
// <copyright file="PackageSet.cs" company="Ace Poker Solutions">
// Copyright © 2018 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using PPPokerCardCatcher.Common.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PPPokerCardCatcher.Importers.TcpBased
{
    internal class PacketsSet<T> where T : class
    {
        private const int ProcessedPacketsMaxSize = 150;

        private SortedSet<uint> processedPackets = new SortedSet<uint>();

        private SortedDictionary<uint, SubPacket<T>> packets = new SortedDictionary<uint, SubPacket<T>>();

        public SubPacket<T> AddSubPacket(CapturedPacket capturedPacket)
        {
            return AddPacket(capturedPacket, 0, false);
        }

        public SubPacket<T> AddStartingPacket(CapturedPacket capturedPacket, int expectedLength)
        {
            return AddPacket(capturedPacket, expectedLength, true);
        }

        public int GetLengthToCompletePacket()
        {
            SubPacket<T> startingPacket = null;

            foreach (var packet in packets.Values.ToArray())
            {
                if (packet.IsStarting)
                {
                    if (packet.IsCompleted)
                    {
                        continue;
                    }

                    startingPacket = packet.Clone();
                    continue;
                }

                if (startingPacket == null)
                {
                    continue;
                }

                var packetBytes = packet.Bytes.ToArray();

                if (startingPacket.CanAddSubPacket(packetBytes, packet.SequenceNumber))
                {
                    if (startingPacket.CanCompleteBySubPacket(packetBytes, packet.SequenceNumber))
                    {
                        return 0;
                    }

                    startingPacket.Add(packetBytes);
                }
            }

            return startingPacket != null ? startingPacket.ExpectedLength - startingPacket.CurrentLength : 0;
        }

        private SubPacket<T> AddPacket(CapturedPacket capturedPacket, int expectedLength, bool isStarting)
        {
            if (processedPackets.Contains(capturedPacket.SequenceNumber))
            {
                return null;
            }

            var subPacket = new SubPacket<T>(capturedPacket.Bytes, expectedLength, capturedPacket.CreatedTimeStamp, capturedPacket.SequenceNumber, isStarting);

            packets.Add(capturedPacket.SequenceNumber, subPacket);

            processedPackets.Add(capturedPacket.SequenceNumber);

            if (TryAssemblyPacket(out SubPacket<T> reassembliedPacket))
            {
                return reassembliedPacket;
            }

            return subPacket;
        }

        private bool TryAssemblyPacket(out SubPacket<T> subPacket)
        {
            subPacket = null;

            SubPacket<T> startingPacket = null;

            var packetsToRemove = new List<uint>();

            foreach (var packet in packets.Values.ToArray())
            {
                if (packet.IsStarting)
                {
                    if (packet.IsCompleted)
                    {
                        subPacket = packet;
                        Remove(packet);
                        return true;
                    }

                    packetsToRemove.Clear();
                    packetsToRemove.Add(packet.SequenceNumber);

                    startingPacket = packet.Clone();

                    continue;
                }

                if (startingPacket == null)
                {
                    continue;
                }

                var packetBytes = packet.Bytes.ToArray();

                if (startingPacket.CanAddSubPacket(packetBytes, packet.SequenceNumber))
                {
                    packetsToRemove.Add(packet.SequenceNumber);

                    if (startingPacket.CanCompleteBySubPacket(packetBytes, packet.SequenceNumber))
                    {
                        startingPacket.Add(packetBytes);
                        subPacket = startingPacket;
                        packetsToRemove.ForEach(x => packets.Remove(x));
                        return true;
                    }

                    startingPacket.Add(packetBytes);
                }
            }

            return false;
        }

        public void Remove(SubPacket<T> packet)
        {
            packets.Remove(packet.SequenceNumber);
        }

        public void RemoveExpiredPackets(int expirationPeriod = 3000)
        {
            packets.RemoveByCondition(p => p.Value.IsExpired(expirationPeriod));

            if (processedPackets.Count > ProcessedPacketsMaxSize)
            {
                var packetsToRemove = processedPackets.Take(processedPackets.Count - ProcessedPacketsMaxSize).ToArray();
                processedPackets.RemoveRange(packetsToRemove);
            }
        }
    }
}