//-----------------------------------------------------------------------
// <copyright file="MibTcpExtenstions.cs" company="Ace Poker Solutions">
// Copyright © 2015 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using PPPokerHandGrabber.Common.WinApi;
using System.Net;
using System.Net.Sockets;

namespace PPPokerHandGrabber.Common.Utils.Network
{
    public static class MibTcpExtenstions
    {
        public static LocalConnection ToLocalConnection(this MibTcpRowOwnerPid tcpRow)
        {
            return new LocalConnection
            {
                LocalAddress = new IPEndPoint(tcpRow.LocalAddress, tcpRow.LocalPort),
                Destination = new IPEndPoint(tcpRow.RemoteAddress, tcpRow.RemotePort),
                ProcessId = tcpRow.ProcessId,
                Protocol = ProtocolType.Tcp
            };
        }

        public static LocalConnection ToLocalConnection(this MibTcp6RowOwnerPid tcpRow)
        {
            return new LocalConnection
            {
                LocalAddress = new IPEndPoint(tcpRow.LocalAddress, tcpRow.LocalPort),
                Destination = new IPEndPoint(tcpRow.RemoteAddress, tcpRow.RemotePort),
                ProcessId = tcpRow.ProcessId,
                Protocol = ProtocolType.Tcp
            };
        }

        public static LocalConnection ToLocalConnection(this MibUdpRowOwnerPid udpRow)
        {
            return new LocalConnection
            {
                LocalAddress = new IPEndPoint(udpRow.LocalAddress, udpRow.LocalPort),
                Destination = new IPEndPoint(IPAddress.Any, 0),
                ProcessId = udpRow.ProcessId,
                Protocol = ProtocolType.Udp
            };
        }

        public static LocalConnection ToLocalConnection(this MibUdp6RowOwnerPid udpRow)
        {
            return new LocalConnection
            {
                LocalAddress = new IPEndPoint(udpRow.LocalAddress, udpRow.LocalPort),
                Destination = new IPEndPoint(IPAddress.IPv6Any, 0),
                ProcessId = udpRow.ProcessId,
                Protocol = ProtocolType.Udp
            };
        }
    }
}