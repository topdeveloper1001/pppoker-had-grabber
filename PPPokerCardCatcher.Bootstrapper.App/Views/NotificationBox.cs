//-----------------------------------------------------------------------
// <copyright file="NotificationBox.cs" company="Ace Poker Solutions">
// Copyright © 2017 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using PPPokerCardCatcher.Bootstrapper.App.ViewModels;
using System.Windows.Forms;

namespace PPPokerCardCatcher.Bootstrapper.App.Views
{
    public class NotificationBox
    {
        public static DialogResult Show(string title, string notification, MessageBoxButtons buttons)
        {
            var notificationViewModelInfo = new NotificationViewModelInfo
            {
                Title = title,
                Notification = notification,
                Buttons = buttons
            };

            var notificationViewModel = new NotificationViewModel(notificationViewModelInfo);

            var notificationView = new NotificationView
            {
                DataContext = notificationViewModel,
                Owner = BootstrapperApp.RootView
            };

            notificationViewModel.Closed += (s, a) => notificationView?.Close();

            notificationView.ShowDialog();

            return notificationViewModel.DialogResult;
        }
    }
}