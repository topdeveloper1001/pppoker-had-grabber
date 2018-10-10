//-----------------------------------------------------------------------
// <copyright file="PipeOpenMode.cs" company="Ace Poker Solutions">
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

namespace PPPokerHandGrabber.Common.WinApi
{
    /// <summary>
    /// Named Pipe Open Modes
    /// http://msdn.microsoft.com/en-us/library/aa365596.aspx
    /// </summary>
    [Flags]
    public enum PipeOpenMode : uint
    {
        // Inbound pipe access.
        PIPE_ACCESS_INBOUND = 0x00000001,
        // Outbound pipe access.
        PIPE_ACCESS_OUTBOUND = 0x00000002,
        // Duplex pipe access.
        PIPE_ACCESS_DUPLEX = 0x00000003
    }
}