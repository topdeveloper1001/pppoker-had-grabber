﻿//-----------------------------------------------------------------------
// <copyright file="MibTcp6RowOwnerPid.cs" company="Ace Poker Solutions">
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
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;

namespace PPPokerCardCatcher.Common.WinApi
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MibTcp6RowOwnerPid
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] localAddr;

        public uint localScopeId;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] localPort;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] remoteAddr;

        public uint remoteScopeId;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] remotePort;

        public uint state;

        public uint owningPid;

        public uint ProcessId
        {
            get { return owningPid; }
        }

        public long LocalScopeId
        {
            get { return localScopeId; }
        }
        public IPAddress LocalAddress
        {
            get { return new IPAddress(localAddr, LocalScopeId); }
        }

        public ushort LocalPort
        {
            get { return BitConverter.ToUInt16(localPort.Take(2).Reverse().ToArray(), 0); }
        }

        public long RemoteScopeId
        {
            get { return remoteScopeId; }
        }

        public IPAddress RemoteAddress
        {
            get { return new IPAddress(remoteAddr, RemoteScopeId); }
        }

        public ushort RemotePort
        {
            get { return BitConverter.ToUInt16(remotePort.Take(2).Reverse().ToArray(), 0); }
        }

        public MibTcpState State
        {
            get { return (MibTcpState)state; }
        }
    }
}