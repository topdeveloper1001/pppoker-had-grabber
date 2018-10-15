//-----------------------------------------------------------------------
// <copyright file="BurnInstallationState.cs" company="Ace Poker Solutions">
// Copyright © 2015 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using GalaSoft.MvvmLight;
using PPPokerCardCatcher.Bootstrapper.App.Common;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using System;

namespace PPPokerCardCatcher.Bootstrapper.App.ViewModels
{
    public abstract class PageViewModel : ViewModelBase
    {
        public PageViewModel(MainWindowViewModel mainViewModel)
        {
            if (mainViewModel == null)
            {
                throw new ArgumentNullException(nameof(mainViewModel));
            }

            MainViewModel = mainViewModel;
        }

        protected MainWindowViewModel MainViewModel { get; }

        public abstract PageType PageType { get; }

        protected BootstrapperApp Bootstrapper
        {
            get
            {
                return MainViewModel.Bootstrapper;
            }
        }

        protected void Log(LogLevel level, string message)
        {
            MainViewModel.Log(level, $"{PageType} page: $message");
        }
    }
}