//-----------------------------------------------------------------------
// <copyright file="TableWindowProvider.cs" company="Ace Poker Solutions">
// Copyright © 2018 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using PPPokerCardCatcher.Common.Log;
using PPPokerCardCatcher.Importers.TcpBased.EmulatorProviders;
using System;
using System.Diagnostics;

namespace PPPokerCardCatcher.Importers.AndroidBase
{
    internal class TableWindowProvider : ITableWindowProvider
    {
        private readonly IEmulatorProvider[] providers = new IEmulatorProvider[]
        {
            new PPPDesktopEmulatorProvider()
        };

        public IntPtr GetTableWindowHandle(Process process)
        {
            try
            {
                foreach (var provider in providers)
                {
                    if (provider.CanProvide(process))
                    {
                        return provider.GetProcessWindowHandle(process);
                    }
                }
            }
            catch (Exception e)
            {
                LogProvider.Log.Error(this, $"Could not find window associated with process={process.Id}.", e);
            }

            return IntPtr.Zero;
        }
    }
}