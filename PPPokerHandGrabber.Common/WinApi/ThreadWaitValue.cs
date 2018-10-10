//-----------------------------------------------------------------------
// <copyright file="ThreadWaitValue.cs" company="Ace Poker Solutions">
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
    /// Signal state of object, used in <see cref="WinApi.WaitForSingleObject"/>
    /// </summary>
    public enum ThreadWaitValue : uint
    {
        Object0 = 0x00000000,
        Abandoned = 0x00000080,
        Timeout = 0x00000102,
        Failed = 0xFFFFFFFF,
        Infinite = 0xFFFFFFFF
    }
}