//-----------------------------------------------------------------------
// <copyright file="PipeNative.cs" company="Ace Poker Solutions">
// Copyright © 2015 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using System.Security;

namespace PPPokerCardCatcher.Common.WinApi
{
    /// <summary>
    /// Native pipe constants
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    public static class PipeNative
    {
        /// <summary>
        /// Unlimited server pipe instances.
        /// </summary>
        public const uint PIPE_UNLIMITED_INSTANCES = 255;

        /// <summary>
        /// The operation completed successfully.
        /// </summary>
        public const ulong ERROR_SUCCESS = 0;

        /// <summary>
        /// The system cannot find the file specified.
        /// </summary>
        public const ulong ERROR_CANNOT_CONNECT_TO_PIPE = 2;

        /// <summary>
        /// All pipe instances are busy.
        /// </summary>
        public const ulong ERROR_PIPE_BUSY = 231;

        /// <summary>
        /// The pipe is being closed.
        /// </summary>
        public const ulong ERROR_NO_DATA = 232;

        /// <summary>
        /// No process is on the other end of the pipe.
        /// </summary>
        public const ulong ERROR_PIPE_NOT_CONNECTED = 233;

        /// <summary>
        /// More data is available.
        /// </summary>
        public const ulong ERROR_MORE_DATA = 234;

        /// <summary>
        /// There is a process on other end of the pipe.
        /// </summary>
        public const ulong ERROR_PIPE_CONNECTED = 535;

        /// <summary>
        /// Waiting for a process to open the other end of the pipe.
        /// </summary>
        public const ulong ERROR_PIPE_LISTENING = 536;

        /// <summary>
        /// Waits indefinitely when connecting to a pipe.
        /// </summary>
        public const uint NMPWAIT_WAIT_FOREVER = 0xffffffff;

        /// <summary>
        /// Does not wait for the named pipe.
        /// </summary>
        public const uint NMPWAIT_NOWAIT = 0x00000001;

        /// <summary>
        /// Uses the default time-out specified in a call to the 
        /// CreateNamedPipe method.
        /// </summary>
        public const uint NMPWAIT_USE_DEFAULT_WAIT = 0x00000000;

        /// <summary>
        /// Invalid operating system handle.
        /// </summary>
        public const int INVALID_HANDLE_VALUE = -1;
    }
}