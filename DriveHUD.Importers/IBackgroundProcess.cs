//-----------------------------------------------------------------------
// <copyright file="IBackgroundProcess.cs" company="Ace Poker Solutions">
// Copyright © 2015 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using System;

namespace DriveHUD.Importers
{
    /// <summary>
    /// Base background process
    /// </summary>
    internal interface IBackgroundProcess : IDisposable
    {
        /// <summary>
        /// Process has been stopped
        /// </summary>
        event EventHandler ProcessStopped;

        /// <summary>
        /// True if process is running
        /// </summary>
        bool IsRunning { get; }
            
        /// <summary>
        /// Starts background process
        /// </summary>    
        void Start();

        /// <summary>
        /// Stop background process
        /// </summary>     
        void Stop();        
    }
}