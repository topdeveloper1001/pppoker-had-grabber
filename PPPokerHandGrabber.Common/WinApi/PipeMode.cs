//-----------------------------------------------------------------------
// <copyright file="PipeMode.cs" company="Ace Poker Solutions">
// Copyright © 2015 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

namespace PPPokerHandGrabber.Common.WinApi
{
    /// <summary>
    /// Named Pipe Type, Read, and Wait Modes
    /// http://msdn.microsoft.com/en-us/library/aa365605.aspx
    /// </summary>
    public enum PipeMode : uint
    {
        // Byte pipe type.
        PIPE_TYPE_BYTE = 0x00000000,
        // Message pipe type.
        PIPE_TYPE_MESSAGE = 0x00000004,
        // Read mode of type Byte.
        PIPE_READMODE_BYTE = 0x00000000,
        // Read mode of type Message.
        PIPE_READMODE_MESSAGE = 0x00000002,
        // Pipe blocking mode.
        PIPE_WAIT = 0x00000000,
        // Pipe non-blocking mode.
        PIPE_NOWAIT = 0x00000001
    }
}