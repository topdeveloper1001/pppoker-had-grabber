//-----------------------------------------------------------------------
// <copyright file="NetworkUtils.cs" company="Ace Poker Solutions">
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

namespace PPPokerCardCatcher.Common.Utils.Network
{
    public class NetworkUtils
    {
        public static IEnumerable<LocalConnection> GetAllTcpUdpConnections()
        {
            var connections = WinApi.WinApi.
                GetAllTCPConnections().
                Select(x => x.ToLocalConnection()).
                Concat(WinApi.WinApi.GetAllTCPv6Connections().
                    Select(x => x.ToLocalConnection())).
                Concat(WinApi.WinApi.GetAllUDPConnections().
                    Select(x => x.ToLocalConnection())).
                Concat(WinApi.WinApi.GetAllUDPv6Connections().
                    Select(x => x.ToLocalConnection())).
                ToArray();

            return connections;
        }

        public static IEnumerable<LocalConnection> GetAllTcpConnections()
        {
            var connections = WinApi.WinApi.
                GetAllTCPConnections().
                Select(x => x.ToLocalConnection()).
                Concat(WinApi.WinApi.GetAllTCPv6Connections().
                    Select(x => x.ToLocalConnection())).
                ToArray();

            return connections;
        }
    }
}