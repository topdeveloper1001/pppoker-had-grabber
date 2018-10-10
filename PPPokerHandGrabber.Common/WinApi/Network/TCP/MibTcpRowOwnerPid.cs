//-----------------------------------------------------------------------
// <copyright file="MibTcpRowOwnerPid.cs" company="Ace Poker Solutions">
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
using System.Runtime.InteropServices;

namespace PPPokerHandGrabber.Common.WinApi
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MibTcpRowOwnerPid
    {
        public uint state;

        public uint localAddr;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] localPort;

        public uint remoteAddr;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]

        public byte[] remotePort;

        public uint owningPid;

        public uint ProcessId
        {
            get { return owningPid; }
        }

        public IPAddress LocalAddress
        {
            get { return new IPAddress(localAddr); }
        }

        public ushort LocalPort
        {
            get
            {
                return BitConverter.ToUInt16(new byte[2] { localPort[1], localPort[0] }, 0);
            }
        }

        public IPAddress RemoteAddress
        {
            get { return new IPAddress(remoteAddr); }
        }

        public ushort RemotePort
        {
            get
            {
                return BitConverter.ToUInt16(new byte[2] { remotePort[1], remotePort[0] }, 0);
            }
        }

        public MibTcpState State
        {
            get { return (MibTcpState)state; }
        }
    }
}