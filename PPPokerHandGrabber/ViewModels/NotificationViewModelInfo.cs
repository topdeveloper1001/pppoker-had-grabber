//-----------------------------------------------------------------------
// <copyright file="NotificationViewModelInfo.cs" company="Ace Poker Solutions">
// Copyright © 2017 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using System;
using System.Windows.Forms;

namespace PPPokerHandGrabber.ViewModels
{
    internal class NotificationViewModelInfo
    {
        public string Title { get; set; }

        public string Notification { get; set; }

        public MessageBoxButtons Buttons { get; set; }

        public Action OKAction { get; set; }

        public Action YesAction { get; set; }

        public Action NoAction { get; set; }

        public Action CancelAction { get; set; }
    }
}