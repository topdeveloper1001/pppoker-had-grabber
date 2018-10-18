//-----------------------------------------------------------------------
// <copyright file="IDHLog.cs" company="Ace Poker Solutions">
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

namespace DriveHUD.Common.Log
{
    /// <summary>
    /// Interface generalize log definition and writing in one method    
    /// </summary>
    public interface IDHLog
    {
        /// <summary>
        /// Write message to log 
        /// </summary>
        /// <param name="senderType">Class - source of message. If null then mainLog will be used.</param>
        /// <param name="message">Message to log</param>
        /// <param name="logMessageType">Type of message</param>
        void Log(Type senderType, object message, LogMessageType logMessageType);

        /// <summary>
        /// Write message to log 
        /// </summary>
        /// <param name="senderType">Class - source of message. If null then mainLog will be used.</param>
        /// <param name="message">Message to log</param>
        /// <param name="exception">Exception to log</param>
        /// <param name="logMessageType">Type of message</param>
        void Log(Type senderType, object message, Exception exception, LogMessageType logMessageType);

        /// <summary>
        /// Write message to log 
        /// </summary>
        /// <param name="loggerName">Logger name</param>
        /// <param name="message">Message to log</param>
        /// <param name="logMessageType">Type of message</param>
        void Log(string loggerName, object message, LogMessageType logMessageType);

        /// <summary>
        /// Write message to log 
        /// </summary>
        /// <param name="loggerName">Logger name</param>
        /// <param name="message">Message to log</param>
        /// <param name="exception">Exception to log</param>
        /// <param name="logMessageType">Type of message</param>
        void Log(string loggerName, object message, Exception exception, LogMessageType logMessageType);
    }
}