//-----------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="Ace Poker Solutions">
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
using PPPokerCardCatcher.Bootstrapper.App.Properties;
using PPPokerCardCatcher.Bootstrapper.App.Views;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;

namespace PPPokerCardCatcher.Bootstrapper.App.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private const string BurnBundleVersionVariable = "WixBundleVersion";

        public MainWindowViewModel(BootstrapperApp bootstrapperApp)
        {
            Bootstrapper = bootstrapperApp;

            WireUpEventHandlers();
        }

        #region Properties

        public BootstrapperApp Bootstrapper { get; }

        public IntPtr WindowHandle { get; set; }

        public Version BundleVersion => new Version(Bootstrapper.Engine.VersionVariables[BurnBundleVersionVariable].ToString());

        public string Version
        {
            get
            {
                return string.Format(Resources.Common_Version, Assembly.GetExecutingAssembly().GetName().Version);
            }
        }

        public string Title
        {
            get
            {
                return Resources.Common_Title;
            }
        }

        private PageViewModel pageViewModel;

        public PageViewModel PageViewModel
        {
            get
            {
                return pageViewModel;
            }

            set
            {
                Set(() => PageViewModel, ref pageViewModel, value);
            }
        }

        private bool isInstalled;

        public bool IsInstalled
        {
            get
            {
                return isInstalled;
            }
            set
            {
                Set(() => IsInstalled, ref isInstalled, value);
            }
        }

        private VersionStatus versionStatus;

        public VersionStatus VersionStatus
        {
            get
            {
                return versionStatus;
            }
            set
            {
                Set(() => VersionStatus, ref versionStatus, value);
            }
        }

        private BurnInstallationState burnInstallationState;

        public BurnInstallationState BurnInstallationState
        {
            get
            {
                return burnInstallationState;
            }
            set
            {
                Set(() => BurnInstallationState, ref burnInstallationState, value);
            }
        }

        public bool ShouldCancel { get; set; }

        private LaunchAction launchAction;

        public LaunchAction LaunchAction
        {
            get
            {
                return launchAction;
            }
            set
            {
                Set(nameof(LaunchAction), ref launchAction, value);
            }
        }


        private LaunchAction executedAction;

        public LaunchAction ExecutedAction
        {
            get
            {
                return executedAction;
            }
            set
            {
                Set(nameof(ExecutedAction), ref executedAction, value);
            }
        }


        private UpdateState bootstrapperUpdateState;

        public UpdateState BootstrapperUpdateState
        {
            get
            {
                return bootstrapperUpdateState;
            }
            set
            {
                Set(nameof(BootstrapperUpdateState), ref bootstrapperUpdateState, value);
            }
        }

        private int status;

        public int Status
        {
            get
            {
                return status;
            }
            private set
            {
                Set(nameof(Status), ref status, value);
            }
        }

        private string errorMessage;

        public string ErrorMessage
        {
            get
            {
                return errorMessage;
            }
            private set
            {
                Set(nameof(ErrorMessage), ref errorMessage, value);
            }
        }

        public bool IsInteractive
        {
            get
            {
                return Bootstrapper.Command.Display == Display.Full;
            }
        }

        #endregion

        #region Public methods

        public void PlanAction(LaunchAction action)
        {
            Log(LogLevel.Standard, $"Calling {nameof(PlanAction)}: {action}");
            LaunchAction = action;
            Bootstrapper.Engine.Plan(action);
        }

        #endregion

        #region Infrastructure

        public void NavigateToPage(PageType pageType)
        {
            Log(LogLevel.Standard, $"Navigate to page: {pageType}");

            var pageViewModel = CreateNewPageViewModel(pageType);
            PageViewModel = pageViewModel;
        }

        private PageViewModel CreateNewPageViewModel(PageType pageType)
        {
            switch (pageType)
            {
                case PageType.InstallPage:
                    return new InstallViewModel(this);
                case PageType.PlanPage:
                    return new PlanViewModel(this);
                case PageType.ProgressPage:
                    return new ProgressViewModel(this);
                case PageType.FinishPage:
                    return new FinishViewModel(this);
                case PageType.MaintenancePage:
                    return new MaintenanceViewModel(this);
                case PageType.FinishErrorPage:
                    return new FinishErrorViewModel(this);
            }

            throw new ArgumentException("Not supported page type");
        }

        private void ApplyAction()
        {
            Log(LogLevel.Standard, "ApplyAction called.");
            BurnInstallationState = BurnInstallationState.Applying;
            Bootstrapper.Engine.Apply(WindowHandle);
        }

        private void WireUpEventHandlers()
        {
            Bootstrapper.DetectUpdateBegin += Bootstrapper_DetectUpdateBegin;
            Bootstrapper.DetectUpdate += Bootstrapper_DetectUpdate;
            Bootstrapper.DetectUpdateComplete += Bootstrapper_DetectUpdateComplete;
            Bootstrapper.DetectBegin += Bootstrapper_DetectBegin;
            Bootstrapper.DetectComplete += Bootstrapper_DetectComplete;
            Bootstrapper.DetectPackageComplete += Bootstrapper_DetectPackageComplete;
            Bootstrapper.DetectRelatedBundle += Bootstrapper_DetectRelatedBundle;
            Bootstrapper.DetectRelatedMsiPackage += Bootstrapper_DetectRelatedMsiPackage;
            Bootstrapper.Error += Bootstrapper_Error;
            Bootstrapper.PlanBegin += Bootstrapper_PlanBegin;
            Bootstrapper.PlanComplete += Bootstrapper_PlanComplete;
            Bootstrapper.ApplyBegin += Bootstrapper_ApplyBegin;
            Bootstrapper.ApplyComplete += Bootstrapper_ApplyComplete;
            Bootstrapper.ResolveSource += Bootstrapper_ResolveSource;
        }

        private void Bootstrapper_ResolveSource(object sender, ResolveSourceEventArgs e)
        {
            Log(LogLevel.Standard,
                $"Bootstrapper has called {nameof(this.Bootstrapper_ResolveSource)}, Download source: {e.DownloadSource}");

            e.Result = Result.Download;
        }

        private void Bootstrapper_DetectUpdateComplete(object sender, DetectUpdateCompleteEventArgs e)
        {
            Log(LogLevel.Debug, $"Bootstrapper has called {nameof(this.Bootstrapper_DetectUpdateComplete)}!");

            if (BootstrapperUpdateState != UpdateState.Failed && e.Status < 0)
            {
                Log(LogLevel.Standard, $"Failed to detect updates, status: {e.Status:X8}");

                BootstrapperUpdateState = UpdateState.Failed;
                // Re-detect, updates are now disabled
                Bootstrapper.Engine.Detect();
            }
            else if (Bootstrapper.Command.Action == LaunchAction.Uninstall
                     || BootstrapperUpdateState == UpdateState.Initializing
                     || BootstrapperUpdateState == UpdateState.Checking
                     || !IsInteractive)
            {
                BootstrapperUpdateState = UpdateState.Unknown;
            }
        }

        private void Bootstrapper_DetectUpdate(object sender, DetectUpdateEventArgs e)
        {
            Log(LogLevel.Debug, $"Bootstrapper has called {nameof(this.Bootstrapper_DetectUpdate)}!");
            Log(LogLevel.Standard, $"Bootstrapper found bundle version {e.Version} at \"{e.UpdateLocation}\", current version: {Version}");

            if (e.Version > BundleVersion)
            {
                BootstrapperUpdateState = UpdateState.Available;
                e.Result = Result.Ok;
            }
            else if (e.Version <= BundleVersion)
            {
                BootstrapperUpdateState = UpdateState.Current;
                e.Result = Result.Cancel;
            }
        }

        private void Bootstrapper_DetectUpdateBegin(object sender, DetectUpdateBeginEventArgs e)
        {
            Log(LogLevel.Debug, $"Bootstrapper has called {nameof(this.Bootstrapper_DetectUpdateBegin)}");

            if (IsInteractive && (Bootstrapper.Command.Resume == ResumeType.None || Bootstrapper.Command.Resume == ResumeType.Arp) &&
                    BootstrapperUpdateState != UpdateState.Failed && Bootstrapper.Command.Action != LaunchAction.Uninstall)
            {
                BootstrapperUpdateState = UpdateState.Checking;
                e.Result = Result.Ok;
            }
        }

        private void Bootstrapper_DetectRelatedMsiPackage(object sender, DetectRelatedMsiPackageEventArgs e)
        {
            Log(LogLevel.Standard, $"Bootstrapper_DetectRelatedMsiPackage: PackageId {e.PackageId}, operation={e.PackageId}");

        }

        private void Bootstrapper_ApplyComplete(object sender, ApplyCompleteEventArgs e)
        {
            Log(LogLevel.Debug, $"Bootstrapper has called {nameof(this.Bootstrapper_ApplyComplete)}");

            lock (this)
            {
                Bootstrapper.ApplyComplete -= Bootstrapper_ApplyComplete;

                BurnInstallationState = e.Status >= 0 ?
                    BurnInstallationState.Applied :
                    BurnInstallationState.Failed;

                ExecutedAction = LaunchAction;

                Status = e.Status;

                if (IsInteractive)
                {
                    NavigateToPage(e.Status >= 0 ? PageType.FinishPage : PageType.FinishErrorPage);
                }
                else
                {
                    Bootstrapper.Engine.Quit(Status);
                }
            }
        }

        private void Bootstrapper_ApplyBegin(object sender, ApplyBeginEventArgs e)
        {
            Log(LogLevel.Debug, $"Bootstrapper has called {nameof(this.Bootstrapper_ApplyBegin)}");

            BurnInstallationState = BurnInstallationState.Applying;

            if (IsInteractive)
            {
                NavigateToPage(PageType.ProgressPage);
            }
        }

        private void Bootstrapper_PlanComplete(object sender, PlanCompleteEventArgs e)
        {
            Log(LogLevel.Standard, $"Bootstrapper has called {nameof(this.Bootstrapper_PlanComplete)}: Status {e.Status}");

            Bootstrapper.PlanComplete -= Bootstrapper_PlanComplete;

            if (BurnInstallationState != BurnInstallationState.Applying)
            {
                ApplyAction();
            }
        }

        private void Bootstrapper_PlanBegin(object sender, PlanBeginEventArgs e)
        {
            Log(LogLevel.Debug, $"Bootstrapper has called {nameof(this.Bootstrapper_PlanBegin)}");

            if (IsInteractive)
            {
                NavigateToPage(PageType.PlanPage);
            }
        }

        private void Bootstrapper_Error(object sender, ErrorEventArgs e)
        {
            e.Result = Result.Restart;

            Log(LogLevel.Standard, $"Bootstrapper has called {nameof(this.Bootstrapper_Error)}");

            lock (this)
            {
                try
                {
                    Log(LogLevel.Error, $"Bootstrapper received error code {e.ErrorCode}: {e.ErrorMessage}");

                    if (ShouldCancel)
                    {
                        e.Result = Result.Cancel;
                        return;
                    }

                    if (BurnInstallationState == BurnInstallationState.Applying && e.ErrorCode == 1223)
                    {
                        return;
                    }

                    ErrorMessage = e.ErrorMessage;

                    if (!IsInteractive)
                    {
                        return;
                    }

                    var messageBoxButtonValue = e.UIHint & 0xF;

                    MessageBoxButtons messageBoxButton;

                    if (Enum.GetValues(typeof(MessageBoxButtons)).OfType<int>().Contains(messageBoxButtonValue))
                    {
                        messageBoxButton = (MessageBoxButtons)messageBoxButtonValue;
                    }
                    else
                    {
                        messageBoxButton = MessageBoxButtons.OK;
                    }

                    var result = DialogResult.None;

                    BootstrapperApp.BootstrapperDispatcher.Invoke(
                        (Action)
                        delegate
                        {
                            result = NotificationBox.Show(string.Format(Resources.Common_InstallErrorMessageTitle, Bootstrapper.BundleName),
                                e.ErrorMessage, messageBoxButton);
                        });

                    if (messageBoxButtonValue == (int)messageBoxButton)
                    {
                        e.Result = (Result)result;
                    }
                }
                finally
                {
                    Log(LogLevel.Error, $"Bootstrapper handled error {e.ErrorCode}: {e.ErrorMessage}");
                }
            }
        }

        private void Bootstrapper_DetectPackageComplete(object sender, DetectPackageCompleteEventArgs e)
        {
            Log(LogLevel.Standard, $"Package detection complete: {e.PackageId}, State: {e.State}");
        }

        private void Bootstrapper_DetectRelatedBundle(object sender, DetectRelatedBundleEventArgs e)
        {
            Log(LogLevel.Standard, $"Bootstrapper_DetectRelatedBundle: Bundle {e.ProductCode}, operation: {e.Operation}");

            if (e.Operation == RelatedOperation.None)
            {
                Log(LogLevel.Error, $"This version of {Bootstrapper.BundleName} is already installed");

                VersionStatus = VersionStatus.Current;
            }
            else if (e.Operation == RelatedOperation.Downgrade)
            {
                Log(LogLevel.Error, $"A newer version of {Bootstrapper.BundleName} is already installed");
                VersionStatus = VersionStatus.NewerAlreadyInstalled;
            }
            else if (e.Operation == RelatedOperation.MajorUpgrade || e.Operation == RelatedOperation.MinorUpdate)
            {
                Log(LogLevel.Standard, $"Adding product {e.ProductCode} to list of bundles to update");

                VersionStatus = VersionStatus.OlderInstalled;
            }
        }

        private void Bootstrapper_DetectComplete(object sender, DetectCompleteEventArgs e)
        {
            Log(LogLevel.Standard, $"Detection complete. VersionStatus: {VersionStatus}");

            if (VersionStatus == VersionStatus.Current)
            {
                BurnInstallationState = BurnInstallationState.Failed;
                Log(LogLevel.Standard, "An attempt to reinstall the application was made.");

                if (!IsInteractive)
                {
                    if (Bootstrapper.Command.Action == LaunchAction.Uninstall)
                    {
                        PlanAction(Bootstrapper.Command.Action);
                        return;
                    }

                    ShutDownWithCancelCode();
                }
            }

            if (VersionStatus == VersionStatus.NewerAlreadyInstalled)
            {
                BurnInstallationState = BurnInstallationState.DetectedNewer;
                Log(LogLevel.Standard, "An attempt to downgrade the application was made.");

                if (IsInteractive)
                {
                    BootstrapperApp.BootstrapperDispatcher.Invoke((Action)delegate
                    {
                        NotificationBox.Show(string.Format(Resources.Common_NewerVersionInstalledTitle, Bootstrapper.BundleName),
                            string.Format(Resources.Common_NewerVersionInstalledMessage, Bootstrapper.BundleName),
                            MessageBoxButtons.OK);
                    });
                }

                ShutDownWithCancelCode();
            }
            else if (IsInteractive)
            {
                if (IsInstalled)
                {
                    if (BootstrapperUpdateState == UpdateState.Available)
                    {
                        // update page
                        NavigateToPage(PageType.InstallPage);
                    }
                    else
                    {
                        NavigateToPage(PageType.MaintenancePage);
                    }
                }
                else
                {
                    NavigateToPage(PageType.InstallPage);
                }
            }
            else
            {
                PlanAction(Bootstrapper.Command.Action);
            }
        }

        private void Bootstrapper_DetectBegin(object sender, DetectBeginEventArgs e)
        {
            Log(LogLevel.Debug, $"Bootstrapper has called {nameof(Bootstrapper_DetectBegin)}");

            if (!e.Installed)
            {
                Log(LogLevel.Standard, $"Bootstrapper Detect resulted in DetectedAbsent");
                IsInstalled = false;
                BurnInstallationState = BurnInstallationState.Detected;
                return;
            }

            Log(LogLevel.Standard, $"Bootstrapper Detect resulted in DetectedPresent");

            BurnInstallationState = BurnInstallationState.Detected;
            IsInstalled = true;
        }

        public void Log(LogLevel level, string message)
        {
            Bootstrapper.Engine.Log(level, message);
        }

        public const int CancelErrorCode = 1602;

        protected void ShutDownWithCancelCode()
        {
            Bootstrapper.Engine.Quit(CancelErrorCode);
        }

        #endregion
    }
}