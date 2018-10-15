//-----------------------------------------------------------------------
// <copyright file="Hwnd.cs" company="Ace Poker Solutions">
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

namespace PPPokerCardCatcher.Common.WinApi
{
    public static class Hwnd
    {
        public static IntPtr 
          NoTopMost = new IntPtr(-2),
          TopMost = new IntPtr(-1),
          Top = new IntPtr(0),
          Bottom = new IntPtr(1);
    }
}