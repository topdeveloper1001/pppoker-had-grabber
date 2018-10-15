//-----------------------------------------------------------------------
// <copyright file="NotificationViewModel.cs" company="Ace Poker Solutions">
// Copyright © 2017 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Windows.Forms;
using System.Windows.Input;

namespace PPPokerCardCatcher.Bootstrapper.App.ViewModels
{
    public class NotificationViewModel : ViewModelBase
    {
        public event EventHandler Closed;

        private NotificationViewModelInfo notificationViewModelInfo;

        public NotificationViewModel(NotificationViewModelInfo notificationViewModelInfo)
        {
            this.notificationViewModelInfo = notificationViewModelInfo;

            Title = notificationViewModelInfo.Title;
            Notification = notificationViewModelInfo.Notification;

            InitializeButtons();
            InitializeCommands();
        }

        #region Properties

        private string title;

        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                Set(nameof(Title), ref title, value);
            }
        }

        private string notification;

        public string Notification
        {
            get
            {
                return notification;
            }
            set
            {
                Set(nameof(Notification), ref notification, value);
            }
        }

        private bool yesButtonVisible;

        public bool YesButtonVisible
        {
            get
            {
                return yesButtonVisible;
            }
            set
            {
                Set(nameof(YesButtonVisible), ref yesButtonVisible, value);
            }
        }

        private bool noButtonVisible;

        public bool NoButtonVisible
        {
            get
            {
                return noButtonVisible;
            }
            set
            {
                Set(nameof(NoButtonVisible), ref noButtonVisible, value);
            }
        }

        private bool cancelButtonVisible;

        public bool CancelButtonVisible
        {
            get
            {
                return cancelButtonVisible;
            }
            set
            {
                Set(nameof(CancelButtonVisible), ref cancelButtonVisible, value);
            }
        }

        private bool okButtonVisible;

        public bool OKButtonVisible
        {
            get
            {
                return okButtonVisible;
            }
            set
            {
                Set(nameof(OKButtonVisible), ref okButtonVisible, value);
            }
        }

        public DialogResult DialogResult { get; set; }

        #endregion

        #region Commands

        public ICommand YesCommand { get; private set; }

        public ICommand NoCommand { get; private set; }

        public ICommand OKCommand { get; private set; }

        public ICommand CancelCommand { get; private set; }

        #endregion

        private void InitializeButtons()
        {
            switch (notificationViewModelInfo.Buttons)
            {
                case MessageBoxButtons.YesNo:
                    YesButtonVisible = true;
                    NoButtonVisible = true;
                    break;
                case MessageBoxButtons.YesNoCancel:
                    YesButtonVisible = true;
                    NoButtonVisible = true;
                    CancelButtonVisible = true;
                    break;
                case MessageBoxButtons.OK:
                    OKButtonVisible = true;
                    break;
                case MessageBoxButtons.OKCancel:
                    OKButtonVisible = true;
                    CancelButtonVisible = true;
                    break;
            }
        }

        private void InitializeCommands()
        {
            YesCommand = new RelayCommand(() =>
            {
                DialogResult = DialogResult.Yes;
                OnClosed();
            });

            NoCommand = new RelayCommand(() =>
            {
                DialogResult = DialogResult.No;
                OnClosed();
            });

            OKCommand = new RelayCommand(() =>
            {
                DialogResult = DialogResult.OK;
                OnClosed();
            });

            CancelCommand = new RelayCommand(() =>
            {
                DialogResult = DialogResult.Cancel;
                OnClosed();
            });
        }

        private void OnClosed()
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }
    }
}