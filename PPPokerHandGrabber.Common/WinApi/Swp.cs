//-----------------------------------------------------------------------
// <copyright file="Swp.cs" company="Ace Poker Solutions">
// Copyright © 2018 Ace Poker Solutions. All Rights Reserved.
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
    /// <summary>The <see cref="flags"/> member can be one or more of the following values. 
    /// </summary>
    [Flags]
    public enum Swp : uint
    {
        /// <summary>If the calling thread and the thread that owns the window are attached to
        /// <para>different input queues, the system posts the request to the thread that owns the window.</para> 
        /// <para>This prevents the calling thread from blocking its execution while other threads process the request</para>
        /// </summary>
        ASYNCWINDOWPOS = 0x4000,

        /// <summary>Prevents generation of the WM_SYNCPAINT message
        /// </summary>
        DEFERERASE = 0x2000,

        /// <summary>Draws a frame (defined in the window's class description) around the window. 
        /// Same as the FRAMECHANGED flag.
        /// </summary>
        DRAWFRAME = 0x0020,

        /// <summary>Sends a WM_NCCALCSIZE message to the window, even if the window's size is not being changed. 
        /// If this flag is not specified, WM_NCCALCSIZE is sent only when the window's size is being changed.
        /// </summary>
        FRAMECHANGED = 0x0020,

        /// <summary>Hides the window.
        /// </summary>
        HIDEWINDOW = 0x0080,

        /// <summary>Does not activate the window. 
        /// If this flag is not set, the window is activated and moved to the top of either the topmost or non-topmost group 
        /// (depending on the setting of the hwndInsertAfter member).
        /// </summary>
        NOACTIVATE = 0x0010,

        /// <summary>Discards the entire contents of the client area. 
        /// If this flag is not specified, the valid contents of the client area are saved 
        /// and copied back into the client area after the window is sized or repositioned.
        /// </summary>
        NOCOPYBITS = 0x0100,

        /// <summary>Retains the current position (ignores the x and y members).
        /// </summary>
        NOMOVE = 0x0002,

        /// <summary>Does not change the owner window's position in the Z order.
        /// </summary>
        NOOWNERZORDER = 0x0200,

        /// <summary>Does not redraw changes. 
        /// If this flag is set, no repainting of any kind occurs. 
        /// This applies to the client area, the non-client area (including the title bar and scroll bars), 
        /// and any part of the parent window uncovered as a result of the window being moved. 
        /// When this flag is set, the application must explicitly invalidate or 
        /// redraw any parts of the window and parent window that need redrawing.
        /// </summary>
        NOREDRAW = 0x0008,

        /// <summary>Does not change the owner window's position in the Z order. 
        /// Same as the NOOWNERZORDER flag.
        /// </summary>
        NOREPOSITION = 0x200,

        /// <summary>Prevents the window from receiving the WM_WINDOWPOSCHANGING message.
        /// </summary>
        NOSENDCHANGING = 0x0400,

        /// <summary>Retains the current size (ignores the cx and cy members).
        /// </summary>
        NOSIZE = 0x0001,

        /// <summary>Retains the current Z order (ignores the hwndInsertAfter member).
        /// </summary>
        NOZORDER = 0x0004,

        /// <summary>Displays the window.
        /// </summary>
        SHOWWINDOW = 0x0040,
    }
}