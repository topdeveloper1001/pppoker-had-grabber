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

using PPPokerHandGrabber.Common.Wpf.Mvvm;
using ReactiveUI;
using System.Windows.Forms;

namespace PPPokerHandGrabber.ViewModels
{
    public class NotificationViewModel : PopupWindowViewModel, INotificationViewModel
    {
        private NotificationViewModelInfo notificationViewModelInfo;

        public override void Configure(object viewModelInfo)
        {
            notificationViewModelInfo = viewModelInfo as NotificationViewModelInfo;

            if (notificationViewModelInfo == null)
            {
                OnClosed();
                return;
            }

            Title = notificationViewModelInfo.Title;
            Notification = notificationViewModelInfo.Notification;

            InitializeButtons();
            InitializeCommands();
            OnInitialized();
        }

        #region Properties

        private string notification;

        public string Notification
        {
            get
            {
                return notification;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref notification, value);
            }
        }

        private string title;

        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref title, value);
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
                this.RaiseAndSetIfChanged(ref yesButtonVisible, value);
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
                this.RaiseAndSetIfChanged(ref noButtonVisible, value);
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
                this.RaiseAndSetIfChanged(ref cancelButtonVisible, value);
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
                this.RaiseAndSetIfChanged(ref okButtonVisible, value);
            }
        }

        #endregion

        #region Commands

        public ReactiveCommand YesCommand { get; private set; }

        public ReactiveCommand NoCommand { get; private set; }

        public ReactiveCommand OKCommand { get; private set; }

        public ReactiveCommand CancelCommand { get; private set; }

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
            YesCommand = ReactiveCommand.Create(() =>
            {
                notificationViewModelInfo?.YesAction?.Invoke();
                OnClosed();
            });

            NoCommand = ReactiveCommand.Create(() =>
            {
                notificationViewModelInfo?.NoAction?.Invoke();
                OnClosed();
            });


            OKCommand = ReactiveCommand.Create(() =>
            {
                notificationViewModelInfo?.OKAction?.Invoke();
                OnClosed();
            });

            CancelCommand = ReactiveCommand.Create(() =>
            {
                notificationViewModelInfo?.CancelAction?.Invoke();
                OnClosed();
            });
        }
    }
}