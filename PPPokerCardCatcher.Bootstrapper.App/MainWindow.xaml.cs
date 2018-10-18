//-----------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Ace Poker Solutions">
// Copyright © 2015 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using PPPokerCardCatcher.Bootstrapper.App.Common;
using PPPokerCardCatcher.Bootstrapper.App.Controls;
using PPPokerCardCatcher.Bootstrapper.App.ViewModels;
using PPPokerCardCatcher.Bootstrapper.App.Views;
using System.Windows.Forms;
using System.Windows.Interop;

namespace PPPokerCardCatcher.Bootstrapper.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : PCCWindow
    {
        public MainWindow()
        {
            DataContext = new MainWindowViewModel(null);
            InitializeComponent();
        }

        public MainWindow(BootstrapperApp bootstrapperApp)
        {
            DataContext = new MainWindowViewModel(bootstrapperApp)
            {
                WindowHandle = new WindowInteropHelper(this).Handle
            };

            InitializeComponent();
        }

        public MainWindowViewModel ViewModel => (MainWindowViewModel)DataContext;

        private void PCCWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ViewModel.BurnInstallationState != BurnInstallationState.Applying &&
               (ViewModel.PageViewModel != null && (ViewModel.PageViewModel.PageType == PageType.FinishPage
                || ViewModel.PageViewModel.PageType == PageType.FinishErrorPage)))
            {
                return;
            }

            if (ViewModel.ShouldCancel)
            {
                NotificationBox.Show(Properties.Resources.Common_CancelAlreadyInProgressMessageBoxTitle,
                    Properties.Resources.Common_CancelAlreadyInProgressMessageBoxBody, MessageBoxButtons.OK);

                e.Cancel = true;

                return;
            }

            // if installing is in applying status?
        }
    }
}