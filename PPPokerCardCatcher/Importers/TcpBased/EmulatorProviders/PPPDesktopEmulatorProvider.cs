//-----------------------------------------------------------------------
// <copyright file="PPPDesktopEmulatorProvider.cs" company="Ace Poker Solutions">
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
using System;
using System.Diagnostics;

namespace PPPokerCardCatcher.Importers.TcpBased.EmulatorProviders
{
    internal class PPPDesktopEmulatorProvider : IEmulatorProvider
    {
        protected string EmulatorName => "PPPDesktopEmulator";

        public bool CanProvide(Process process)
        {
            try
            {
                return process != null && process.ProcessName.StartsWith("PPPoker", StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception e)
            {
                LogProvider.Log.Error(this, $"Could not check if process is associated with {EmulatorName} emulator {process.Id}", e);
                return false;
            }
        }

        public IntPtr GetProcessWindowHandle(Process process)
        {
            try
            {
                if (process == null || process.HasExited)
                {
                    return IntPtr.Zero;
                }

                return process != null ? process.MainWindowHandle : IntPtr.Zero;
            }
            catch (Exception e)
            {
                LogProvider.Log.Error(this, $"Could not get window handle of {EmulatorName} emulator for process {process.Id}", e);
            }

            return IntPtr.Zero;
        }
    }
}