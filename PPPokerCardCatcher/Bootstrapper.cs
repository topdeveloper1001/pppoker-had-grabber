//-----------------------------------------------------------------------
// <copyright file="Bootstrapper.cs" company="Ace Poker Solutions">
// Copyright © 2018 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using PPPokerCardCatcher.Common.Log;
using PPPokerCardCatcher.Common.Wpf.Actions;
using PPPokerCardCatcher.Common.Wpf.Interactivity;
using PPPokerCardCatcher.Importers;
using PPPokerCardCatcher.Importers.PPPoker;
using PPPokerCardCatcher.Importers.PPPoker.Model;
using PPPokerCardCatcher.Importers.TcpBased;
using PPPokerCardCatcher.Licensing;
using PPPokerCardCatcher.Security;
using PPPokerCardCatcher.Settings;
using PPPokerCardCatcher.ViewModels;
using PPPokerCardCatcher.Views;
using Prism.Unity;
using System;
using System.Windows;

namespace PPPokerCardCatcher
{
    internal class Bootstrapper : UnityBootstrapper
    {
        private MainWindowViewModel mainWindowViewModel;

        private bool isLicenseValid;

        protected override DependencyObject CreateShell()
        {
            return ServiceLocator.Current.GetInstance<MainWindow>();
        }

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();
            ModuleCatalog.AddModule(null);
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterType<ISettingsService, SettingsService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ILicenseService, LicenseService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IWindowController, WindowController>(new ContainerControlledLifetimeManager());

            Container.RegisterType<IImporterService, ImporterService>(new ContainerControlledLifetimeManager());

            Container.RegisterType<ITcpImporter, TcpImporter>();
            Container.RegisterType<IPPPImporter, PPPImporter>();
            Container.RegisterType<IPackageBuilder<PPPokerPackage>, PPPokerPackageBuilder>();
            Container.RegisterType<IPacketManager<PPPokerPackage>, PPPokerPacketManager>();
            Container.RegisterType<INetworkConnectionsService, NetworkConnectionsService>();

            // licenses
            Container.RegisterType<ILicenseManager, PPTReg>(LicenseType.Trial.ToString());
            Container.RegisterType<ILicenseManager, PPSReg>(LicenseType.Normal.ToString());

            // views
            Container.RegisterType<IViewModelContainer, SettingsView>(RegionViewNames.SettingsPopupView);
            Container.RegisterType<IViewModelContainer, RegistrationView>(RegionViewNames.RegistrationPopupView);
            Container.RegisterType<IViewModelContainer, NotificationView>(RegionViewNames.NotificationPopupView);
            Container.RegisterType<IViewModelContainer, UpdateView>(RegionViewNames.UpdatePopupView);

            // view models
            Container.RegisterType<ISettingsViewModel, SettingsViewModel>();
            Container.RegisterType<IRegistrationViewModel, RegistrationViewModel>();
            Container.RegisterType<INotificationViewModel, NotificationViewModel>();
            Container.RegisterType<IUpdateViewModel, UpdateViewModel>();

            // loggers
            Container.RegisterType<IProtectedLogger, ProtectedLogger>();
        }

        protected override void ConfigureServiceLocator()
        {
            base.ConfigureServiceLocator();
            ConfigureImporters();
        }

        protected override void InitializeShell()
        {
            var licenseService = ServiceLocator.Current.GetInstance<ILicenseService>();
            isLicenseValid = licenseService.Validate();

            try
            {
                mainWindowViewModel = new MainWindowViewModel();

                ((Window)Shell).DataContext = mainWindowViewModel;
                ((Window)Shell).Topmost = true;
                ((Window)Shell).Show();
                ((Window)Shell).Topmost = false;

                if (App.IsUpdateAvailable)
                {
                    mainWindowViewModel.ShowUpdateView();
                }

                if (!isLicenseValid || licenseService.IsTrial ||
                    (licenseService.IsRegistered && licenseService.IsExpiringSoon) ||
                    !licenseService.IsRegistered)
                {
                    var registrationPopupRequestInfo = new RegistrationPopupRequestInfo(false);
                    mainWindowViewModel.RegistrationNotificationRequest.Raise(registrationPopupRequestInfo);

                    mainWindowViewModel.RefreshLicenseText();
                }

                if (!licenseService.IsRegistered)
                {
                    Application.Current.Shutdown();
                }

                mainWindowViewModel.IsTrial = licenseService.IsTrial;
                mainWindowViewModel.IsUpgradable = licenseService.IsUpgradable;
            }
            catch (Exception e)
            {
                LogProvider.Log.Error(this, "Could not launch main window.", e);
            }
        }

        private void ConfigureImporters()
        {
            var importerService = ServiceLocator.Current.GetInstance<IImporterService>();
            importerService.Register<ITcpImporter>();

            var tcpImporter = importerService.GetImporter<ITcpImporter>();
            tcpImporter.RegisterImporter<IPPPImporter>();
        }
    }
}