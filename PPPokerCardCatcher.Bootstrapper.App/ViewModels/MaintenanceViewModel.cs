//-----------------------------------------------------------------------
// <copyright file="MaintenanceViewModel.cs" company="Ace Poker Solutions">
// Copyright © 2015 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using GalaSoft.MvvmLight.Command;
using PPPokerCardCatcher.Bootstrapper.App.Common;
using PPPokerCardCatcher.Bootstrapper.App.Properties;
using PPPokerCardCatcher.Bootstrapper.App.Views;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using System.Windows.Forms;
using System.Windows.Input;

namespace PPPokerCardCatcher.Bootstrapper.App.ViewModels
{
    public class MaintenanceViewModel : PageViewModel
    {
        public MaintenanceViewModel(MainWindowViewModel mainViewModel) : base(mainViewModel)
        {
            InitializeCommands();
        }

        public override PageType PageType
        {
            get
            {
                return PageType.MaintenancePage;
            }
        }

        #region Commands

        public ICommand UninstallCommand { get; private set; }

        public ICommand CancelCommand { get; private set; }

        #endregion

        private void InitializeCommands()
        {
            CancelCommand = new RelayCommand(() =>
            {
                if (NotificationBox.Show(Resources.Common_ExitMessage_Title, Resources.Common_ExitMessage_Text,
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    BootstrapperApp.BootstrapperDispatcher.InvokeShutdown();
                }
            });

            UninstallCommand = new RelayCommand(Uninstall);
        }

        private void Uninstall()
        {
            Log(LogLevel.Standard, $"InstallView: Calling {nameof(Uninstall)}");
            MainViewModel.PlanAction(LaunchAction.Uninstall);
        }
    }
}