//-----------------------------------------------------------------------
// <copyright file="PacketManager.cs" company="Ace Poker Solutions">
// Copyright © 2018 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace PPPokerCardCatcher.Importers.TcpBased
{
    internal abstract class PacketManager<T> : IPacketManager<T> where T : class
    {
        protected Dictionary<SourceDestination, PacketsSet<T>> packetsBytes = new Dictionary<SourceDestination, PacketsSet<T>>();

        protected abstract int PacketHeaderLength
        {
            get;
        }

        public abstract bool IsStartingPacket(byte[] bytes);

        public virtual bool TryParse(CapturedPacket capturedPacket, out IList<T> packages)
        {
            var packetsSet = GetPacketsSet(capturedPacket);

            var packets = ExtractPackets(capturedPacket, packetsSet);

            packages = new List<T>();

            foreach (var packet in packets)
            {
                var subPacket = !IsStartingPacket(packet.Bytes) ?
                    packetsSet.AddSubPacket(packet) :
                    packetsSet.AddStartingPacket(packet, ReadPacketLength(packet.Bytes));

                if (subPacket == null || !subPacket.TryParse(PacketHeaderLength, out T package))
                {
                    packetsSet.RemoveExpiredPackets();
                    continue;
                }

                packages.Add(package);
            }

            return packages.Count > 0;
        }

        public abstract int ReadPacketLength(byte[] bytes);

        protected virtual IList<CapturedPacket> ExtractPackets(CapturedPacket packet, PacketsSet<T> packetsSet)
        {
            var packets = new List<CapturedPacket>();
            var packetLength = 0;
            var offset = 0;

            if (!IsStartingPacket(packet.Bytes))
            {
                var lengthToComplete = packetsSet.GetLengthToCompletePacket();

                if (lengthToComplete == 0 ||
                    lengthToComplete >= packet.Bytes.Length)
                {
                    packets.Add(packet);
                    return packets;
                }

                var additionalPacket = packet.Bytes.Skip(lengthToComplete).ToArray();

                if (IsStartingPacket(additionalPacket))
                {
                    packetLength = lengthToComplete;
                }
            }
            else if ((packetLength = ReadPacketLength(packet.Bytes)) >= packet.Bytes.Length || packetLength == 0)
            {
                packets.Add(packet);
                return packets;
            }

            var sequenceNumber = packet.SequenceNumber;

            while (offset < packet.Bytes.Length)
            {
                var bytes = packet.Bytes.Skip(offset).ToArray();

                if (offset != 0)
                {
                    if (!IsStartingPacket(bytes))
                    {
                        break;
                    }

                    packetLength = ReadPacketLength(bytes);
                }

                var subPacket = new CapturedPacket
                {
                    Bytes = bytes.Take(packetLength).ToArray(),
                    CreatedTimeStamp = packet.CreatedTimeStamp,
                    Destination = packet.Destination,
                    SequenceNumber = sequenceNumber++,
                    Source = packet.Source
                };

                packets.Add(subPacket);

                offset += packetLength;

                if (packetLength == 0)
                {
                    break;
                }
            }

            return packets;
        }

        protected virtual PacketsSet<T> GetPacketsSet(CapturedPacket packet)
        {
            var sourceDestination = new SourceDestination
            {
                IPSource = packet.IPSource,
                PortSource = packet.Source.Port,
                IPDestination = packet.IPDestination,
                PortDestination = packet.Destination.Port
            };

            if (packetsBytes.TryGetValue(sourceDestination, out PacketsSet<T> packetsSet))
            {
                return packetsSet;
            }

            packetsSet = new PacketsSet<T>();

            packetsBytes.Add(sourceDestination, packetsSet);

            return packetsSet;
        }

        protected static bool StartsWith(byte[] bytes, byte[] value)
        {
            if (bytes == null || value == null || bytes.Length < value.Length)
            {
                return false;
            }

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] != bytes[i])
                {
                    return false;
                }
            }

            return true;
        }

        protected class SourceDestination
        {
            public string IPSource { get; set; }

            public int PortSource { get; set; }

            public string IPDestination { get; set; }

            public int PortDestination { get; set; }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashcode = 23;
                    hashcode = (hashcode * 31) + IPSource.GetHashCode();
                    hashcode = (hashcode * 31) + PortSource;
                    hashcode = (hashcode * 31) + IPDestination.GetHashCode();
                    hashcode = (hashcode * 31) + PortDestination;
                    return hashcode;
                }
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as SourceDestination);
            }

            public bool Equals(SourceDestination obj)
            {
                return obj != null && IPSource == obj.IPSource && PortSource == obj.PortSource &&
                    IPDestination == obj.IPDestination && PortDestination == obj.PortDestination;
            }
        }
    }
}