//-----------------------------------------------------------------------
// <copyright file="IEmulatorProvider.cs" company="Ace Poker Solutions">
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
using System.Diagnostics;

namespace PPPokerCardCatcher.Importers.TcpBased.EmulatorProviders
{
    internal interface IEmulatorProvider
    {
        /// <summary>
        /// Determines whenever provider can provide data for the specified process 
        /// </summary>
        /// <param name="process">Process to check</param>
        /// <returns>True if provider matches the process; otherwise false</returns>
        bool CanProvide(Process process);

        /// <summary>
        /// Gets the handle of the window related to the specified process
        /// </summary>
        /// <param name="process">Process to get the handle</param>
        /// <returns>The handle of the window</returns>
        IntPtr GetProcessWindowHandle(Process process);
    }
}