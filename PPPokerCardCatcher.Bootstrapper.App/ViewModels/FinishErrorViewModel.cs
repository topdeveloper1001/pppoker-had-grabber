//-----------------------------------------------------------------------
// <copyright file="FinishErrorViewModel.cs" company="Ace Poker Solutions">
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
using PPPokerCardCatcher.Bootstrapper.App.Properties;

namespace PPPokerCardCatcher.Bootstrapper.App.ViewModels
{
    public class FinishErrorViewModel : PageViewModel
    {
        public FinishErrorViewModel(MainWindowViewModel mainViewModel) : base(mainViewModel)
        {
            CloseCommand = new RelayCommand(() => Bootstrapper.Engine.Quit(MainViewModel.Status));
        }

        public override PageType PageType
        {
            get
            {
                return PageType.FinishErrorPage;
            }
        }

        public string ErrorTitle
        {
            get
            {
                switch ((uint)MainViewModel.Status)
                {
                    case 0x80070642u:
                        return Resources.Common_FinishErrorView_FinishErrorCanceled;
                    default:
                        return string.Format(Resources.Common_FinishErrorView_FinishErrorUnknown, MainViewModel.Status);
                }
            }
        }

        public ICommand CloseCommand { get; private set; }
    }
}