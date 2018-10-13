//-----------------------------------------------------------------------
// <copyright file="SettingsViewModel.cs" company="Ace Poker Solutions">
// Copyright © 2017 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using Microsoft.Practices.ServiceLocation;
using PPPokerHandGrabber.Common.Wpf.Mvvm;
using PPPokerHandGrabber.Importers;
using PPPokerHandGrabber.Settings;
using Prism.Interactivity.InteractionRequest;
using ReactiveUI;
using System.Reactive.Linq;

namespace PPPokerHandGrabber.ViewModels
{
    public class SettingsViewModel : PopupWindowViewModel, ISettingsViewModel
    {
        private readonly ISettingsService settingsService;

        private SettingsModel settingsModel;

        public SettingsViewModel()
        {
            settingsService = ServiceLocator.Current.GetInstance<ISettingsService>();            
        }

        #region Properties

        public InteractionRequest<INotification> NotificationRequest { get; private set; }

        public bool IsAdvancedLoggingEnabled
        {
            get
            {
                return settingsModel.IsAdvancedLoggingEnabled;
            }
            set
            {
                if (settingsModel.IsAdvancedLoggingEnabled == value)
                {
                    return;
                }

                settingsModel.IsAdvancedLoggingEnabled = value;

                this.RaisePropertyChanged();
            }
        }

        public bool AutomaticUpdatingEnabled
        {
            get
            {
                return settingsModel.AutomaticUpdatingEnabled;
            }
            set
            {
                if (settingsModel.AutomaticUpdatingEnabled == value)
                {
                    return;
                }

                settingsModel.AutomaticUpdatingEnabled = value;

                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region Commands        

        public ReactiveCommand ApplyCommand { get; private set; }

        public ReactiveCommand CancelCommand { get; private set; }

        #endregion

        public override void Configure(object viewModelInfo)
        {
            Initialize();
            OnInitialized();
        }

        private void Initialize()
        {
            NotificationRequest = new InteractionRequest<INotification>();

            settingsModel = settingsService.GetSettings();

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            var importerService = ServiceLocator.Current.GetInstance<IImporterService>();

            var canReset = Observable.Return(!importerService.IsStarted);

            ApplyCommand = ReactiveCommand.Create(() =>
            {
                settingsService.SaveSettings(settingsModel);
                OnClosed();
            });

            CancelCommand = ReactiveCommand.Create(() => OnClosed());
        }
    }
}