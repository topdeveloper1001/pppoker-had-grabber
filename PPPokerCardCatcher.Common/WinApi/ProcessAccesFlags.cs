//-----------------------------------------------------------------------
// <copyright file="ProcessAccessFlags.cs" company="Ace Poker Solutions">
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

namespace PPPokerCardCatcher.Common.WinApi
{
    /// <summary>
    /// Process access flags
    /// </summary>
    [Flags]
    public enum ProcessAccessFlags : uint
    {
        // #define PROCESS_TERMINATE                  (0x0001)  
        Terminate = 0x0001,
        // #define PROCESS_CREATE_THREAD              (0x0002) 
        CreateThread = 0x0002,
        // #define PROCESS_SET_SESSIONID              (0x0004)
        SetSessionID = 0x0004,
        // #define PROCESS_VM_OPERATION               (0x0008)   
        VMOperation = 0x0008,
        // #define PROCESS_VM_READ                    (0x0010) 
        VMRead = 0x0010,
        // #define PROCESS_VM_WRITE                   (0x0020)
        VMWrite = 0x0020,
        // #define PROCESS_DUP_HANDLE                 (0x0040)
        DUPHandle = 0x0040,
        // #define PROCESS_CREATE_PROCESS             (0x0080) 
        CreateProcess = 0x0080,
        // #define PROCESS_SET_QUOTA                  (0x0100) 
        SetQuota = 0x0100,
        // #define PROCESS_SET_INFORMATION            (0x0200)
        SetInformation = 0x0200,
        // #define PROCESS_QUERY_INFORMATION          (0x0400) 
        QueryInformation = 0x0400,
        // #define PROCESS_SUSPEND_RESUME             (0x0800)
        SuspendResume = 0x0800,
        // #define PROCESS_QUERY_LIMITED_INFORMATION  (0x1000)
        QueryLimitedInformation = 0x1000,
        // #define PROCESS_ALL_ACCESS        (STANDARD_RIGHTS_REQUIRED | SYNCHRONIZE | 0xFFFF)
        AllAccess = Synchronize | StandardRightsRequired | 0xFFFF,
        // #define SYNCHRONIZE                      (0x00100000L)
        Synchronize = 0x100000,
        // #define STANDARD_RIGHTS_REQUIRED         (0x000F0000L)
        StandardRightsRequired = 0x0F0000      
    }
}