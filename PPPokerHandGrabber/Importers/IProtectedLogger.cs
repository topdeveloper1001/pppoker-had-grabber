//-----------------------------------------------------------------------
// <copyright file="IProtectedLogger.cs" company="Ace Poker Solutions">
// Copyright © 2018 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

namespace PPPokerHandGrabber.Importers
{
    /// <summary>
    /// Interface for poker client loggers
    /// </summary>
    internal interface IProtectedLogger
    {
        /// <summary>
        /// Initialize logger
        /// </summary>
        /// <param name="configuration">Logger configuration</param>
        void Initialize(ProtectedLoggerConfiguration configuration);

        /// <summary>
        /// Start writing in log
        /// </summary>
        void StartLogging();

        /// <summary>
        /// Stop writing in log
        /// </summary>
        void StopLogging();

        /// <summary>
        /// Log message
        /// </summary>
        /// <param name="message">Message to be saved in log</param>
        void Log(string message);

        /// <summary>
        /// Delete outdated logs
        /// </summary>
        void CleanLogs();
    }
}