//-----------------------------------------------------------------------
// <copyright file="ProgressViewModel.cs" company="Ace Poker Solutions">
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
    public class ProgressViewModel : PageViewModel
    {
        public ProgressViewModel(MainWindowViewModel mainViewModel) : base(mainViewModel)
        {
            InitializeCommands();
            InitializeEvents();

            switch (MainViewModel.LaunchAction)
            {
                case LaunchAction.Install:
                    ActionText = Resources.Common_PlanView_InstallAction;
                    ActionDescription = Resources.Common_PlanView_InstallActionDescription;
                    break;
                case LaunchAction.Uninstall:
                    ActionText = Resources.Common_PlanView_UninstallAction;
                    ActionDescription = Resources.Common_PlanView_UninstallActionDescription;
                    break;
            }
        }

        public override PageType PageType
        {
            get
            {
                return PageType.ProgressPage;
            }
        }

        #region Properties

        private int progress;

        public int Progress
        {
            get
            {
                return progress;
            }
            set
            {
                Set(nameof(Progress), ref progress, value);
            }
        }

        private string actionText;

        public string ActionText
        {
            get
            {
                return actionText;
            }
            set
            {
                Set(nameof(ActionText), ref actionText, value);
            }
        }

        private string actionDescription;

        public string ActionDescription
        {
            get
            {
                return actionDescription;
            }
            set
            {
                Set(nameof(ActionDescription), ref actionDescription, value);
            }
        }


        private string currentPackage;

        public string CurrentPackage
        {
            get
            {
                return currentPackage;
            }
            set
            {
                Set(nameof(CurrentPackage), ref currentPackage, value);
            }
        }

        #endregion

        #region Commands

        public ICommand CancelCommand { get; private set; }

        #endregion

        private void InitializeCommands()
        {
            CancelCommand = new RelayCommand(() =>
            {
                if (NotificationBox.Show(Resources.Common_CancelDialogTitle,
                    Resources.Common_CancelDialogBody, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    MainViewModel.ShouldCancel = true;
                    CommandManager.InvalidateRequerySuggested();
                }
            }, () => !MainViewModel.ShouldCancel);
        }

        private void InitializeEvents()
        {
            Bootstrapper.CacheAcquireProgress += Bootstrapper_CacheAcquireProgress;
            Bootstrapper.CacheAcquireBegin += Bootstrapper_CacheAcquireBegin;
            Bootstrapper.CacheAcquireComplete += Bootstrapper_CacheAcquireComplete;
            Bootstrapper.ExecutePackageBegin += Bootstrapper_ExecuteBegin;
            Bootstrapper.ExecutePackageComplete += Bootstrapper_ExecuteComplete;
            Bootstrapper.ExecuteMsiMessage += Bootstrapper_ExecuteMsiMessage;
            Bootstrapper.ExecuteProgress += Bootstrapper_ExecuteProgress;
            Bootstrapper.Progress += Bootstrapper_Progress;
        }

        private void Bootstrapper_Progress(object sender, ProgressEventArgs e)
        {
            HandleCancellation(e);
        }

        private void Bootstrapper_ExecuteProgress(object sender, ExecuteProgressEventArgs e)
        {
            Progress = e.OverallPercentage;
            HandleCancellation(e);
        }

        private void Bootstrapper_ExecuteMsiMessage(object sender, ExecuteMsiMessageEventArgs e)
        {
            HandleCancellation(e);
        }

        private void Bootstrapper_ExecuteComplete(object sender, ExecutePackageCompleteEventArgs e)
        {
            HandleCancellation(e);
        }

        private void Bootstrapper_ExecuteBegin(object sender, ExecutePackageBeginEventArgs e)
        {
            CurrentPackage = e.PackageId;
            HandleCancellation(e);
        }

        private void Bootstrapper_CacheAcquireComplete(object sender, CacheAcquireCompleteEventArgs e)
        {
            HandleCancellation(e);
        }

        private void Bootstrapper_CacheAcquireBegin(object sender, CacheAcquireBeginEventArgs e)
        {
            CurrentPackage = e.PackageOrContainerId;
            HandleCancellation(e);
        }

        private void Bootstrapper_CacheAcquireProgress(object sender, CacheAcquireProgressEventArgs e)
        {
            HandleCancellation(e);
        }

        private void HandleCancellation(ResultEventArgs e)
        {
            if (!MainViewModel.ShouldCancel)
            {
                return;
            }

            e.Result = Result.Cancel;
        }
    }
}