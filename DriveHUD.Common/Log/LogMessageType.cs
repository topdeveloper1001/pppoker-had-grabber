//-----------------------------------------------------------------------
// <copyright file="LogMessageType.cs" company="Ace Poker Solutions">
// Copyright © 2015 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

namespace DriveHUD.Common.Log
{
    /// <summary>
    /// Type of log message
    /// </summary>
    public enum LogMessageType : int
    {
        Debug = 10000,
        Info = 20000,
        Warning = 30000,
        Error = 40000,
        FatalError = 50000
    }
}