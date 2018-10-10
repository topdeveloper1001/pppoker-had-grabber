//-----------------------------------------------------------------------
// <copyright file="MemoryProtection.cs" company="Ace Poker Solutions">
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
    /// Memory protection type - taken from #defines in WinNT.h
    /// </summary>
    public enum MemoryProtection : uint
    {
        NoAccess = 0x001,
        ReadOnly = 0x002,
        ReadWrite = 0x004,
        WriteCopy = 0x008,
        Execute = 0x010,
        ExecuteRead = 0x020,
        ExecuteReadWrite = 0x040,
        ExecuteWriteCopy = 0x080,
        PageGuard = 0x100,
        NoCache = 0x200,
        WriteCombine = 0x400
    }
}