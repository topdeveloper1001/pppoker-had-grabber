//-----------------------------------------------------------------------
// <copyright file="FinishViewModel.cs" company="Ace Poker Solutions">
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PPPokerCardCatcher.Bootstrapper.App.Common;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using System.Diagnostics;
using System.IO;
using PPPokerCardCatcher.Bootstrapper.App.Properties;

namespace PPPokerCardCatcher.Bootstrapper.App.ViewModels
{
    public class FinishViewModel : PageViewModel
    {
        public FinishViewModel(MainWindowViewModel mainViewModel) : base(mainViewModel)
        {
            if (MainViewModel.ExecutedAction == LaunchAction.Install)
            {
                IsLaunchButtonVisible = true;
                FinishText = Resources.Common_FinishView_ThankYouText;
            }
            else if (MainViewModel.ExecutedAction == LaunchAction.Uninstall)
            {
                FinishText = Resources.Common_FinishView_UninstallText;
            }

            InitializeCommands();
        }

        public override PageType PageType
        {
            get
            {
                return PageType.FinishPage;
            }
        }

        private bool isLaunchButtonVisible;

        public bool IsLaunchButtonVisible
        {
            get
            {
                return isLaunchButtonVisible;
            }
            set
            {
                Set(nameof(IsLaunchButtonVisible), ref isLaunchButtonVisible, value);
            }
        }

        private string finishText;

        public string FinishText
        {
            get
            {
                return finishText;
            }
            set
            {
                Set(nameof(FinishText), ref finishText, value);
            }
        }

        #region Commands 

        public ICommand LaunchCommand { get; private set; }

        public ICommand CloseCommand { get; private set; }

        #endregion

        private void InitializeCommands()
        {
            LaunchCommand = new RelayCommand(Launch);

            CloseCommand = new RelayCommand(() => Bootstrapper.Engine.Quit(MainViewModel.Status));
        }

        private void Launch()
        {
            var installDirectory = Bootstrapper.Engine.StringVariables[Variables.InstallFolder].Trim('"');
            var executable = Bootstrapper.Engine.StringVariables[Variables.LaunchProgram].Trim('"');

            var fileToLaunch = Path.Combine(installDirectory, executable);

            try
            {
                var startInfo = new ProcessStartInfo();
                startInfo.WorkingDirectory = Path.GetDirectoryName(fileToLaunch);
                startInfo.FileName = executable;
                startInfo.UseShellExecute = true;

                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                Log(LogLevel.Error, $"Could not launch {fileToLaunch}: {ex}");
            }

            Bootstrapper.Engine.Quit(MainViewModel.Status);
        }
    }
}