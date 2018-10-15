//-----------------------------------------------------------------------
// <copyright file="UpdateViewModel.cs" company="Ace Poker Solutions">
// Copyright © 2015 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using PPPokerCardCatcher.Common.Log;
using PPPokerCardCatcher.Common.Resources;
using PPPokerCardCatcher.Common.Wpf.Mvvm;
using PPPokerCardCatcher.Updater.Core;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Web;
using System.Windows;

namespace PPPokerCardCatcher.ViewModels
{
    /// <summary>
    /// Represents the view model of update form
    /// </summary>
    public class UpdateViewModel : PopupWindowViewModel, IUpdateViewModel
    {
        private ApplicationInfo appInfo;

        public UpdateViewModel()
        {
        }

        #region Properties

        public string Version
        {
            get
            {
                return appInfo.Version.Version;
            }
        }

        private bool isBusy;

        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref isBusy, value);
            }
        }

        private ObservableCollection<UpdateReleaseNoteViewModel> notes;

        public ObservableCollection<UpdateReleaseNoteViewModel> Notes
        {
            get
            {
                return notes;
            }
            private set
            {
                this.RaiseAndSetIfChanged(ref notes, value);
            }
        }

        private int currentProgress;

        public int CurrentProgress
        {
            get
            {
                return currentProgress;
            }
            private set
            {
                this.RaiseAndSetIfChanged(ref currentProgress, value);
            }
        }

        private string statusMessage;

        public string StatusMessage
        {
            get
            {
                return statusMessage;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref statusMessage, value);
            }
        }

        #endregion

        #region Infrastructure

        public override void Configure(object viewModelInfo)
        {
            appInfo = viewModelInfo as ApplicationInfo;

            Initialize();
            OnInitialized();
        }

        private void Initialize()
        {
            BuildNotes();
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            var canUpdate = this.WhenAny(x => x.IsBusy, x => !x.Value);

            CancelCommand = ReactiveCommand.Create(() => OnClosed(), canUpdate);
            UpdateCommand = ReactiveCommand.Create(() => Update(), canUpdate);
        }

        private void BuildNotes()
        {
            Notes = new ObservableCollection<UpdateReleaseNoteViewModel>();

            if (appInfo == null || appInfo.VersionsSinceLastUpdate == null)
            {
                return;
            }

            foreach (var versionInfo in appInfo.VersionsSinceLastUpdate)
            {
                if (versionInfo == null)
                {
                    continue;
                }

                var updateReleaseNoteViewModel = new UpdateReleaseNoteViewModel
                {
                    Notes = HttpUtility.HtmlDecode(versionInfo.ReleaseNotes),
                    Version = versionInfo.Version
                };

                Notes.Add(updateReleaseNoteViewModel);
            }
        }

        private void Update()
        {
            LogProvider.Log.Info("Updating is running");

            RunUpdater();
        }

        private async void RunUpdater()
        {
            IsBusy = true;

            var appLoader = new HttpApplicationInfoLoader();

            try
            {
                using (var appUpdater = new AppUpdater(appLoader))
                {
                    appUpdater.OperationChanged += OnOperationChanged;
                    appUpdater.ProgressChanged += OnProgressChanged;
                    appUpdater.UnzippingFileChanged += OnProcessingFileChanged;
                    appUpdater.CopyingFileChanged += OnProcessingFileChanged;

                    await appUpdater.InitializeAsync();

                    var applicationPath = Assembly.GetExecutingAssembly().Location;
                    var guid = ProgramInfo.AssemblyGuid;

                    if (!appUpdater.CheckIsUpdateAvailable(guid, applicationPath))
                    {
                        StatusMessage = CommonResourceManager.Instance.GetResourceString("Error_NoUpdate");
                        return;
                    }

                    var unpackedDirectory = await appUpdater.UpdateApplicationAsync(guid, applicationPath, true);

                    StatusMessage = CommonResourceManager.Instance.GetResourceString("Common_UpdateView_Completed");

                    var installerName = CommonResourceManager.Instance.GetResourceString("SystemSettings_Installer");

                    var installerPath = Path.Combine(unpackedDirectory.FullName, installerName);

                    RunApplication(installerPath);
                }
            }
            catch (UpdaterException ex)
            {
                HandleUpdaterError(ex);
            }
            catch (FileNotFoundException)
            {
                StatusMessage = CommonResourceManager.Instance.GetResourceString("Error_AssemblyNotFound");
            }
            catch (Exception e)
            {
                LogProvider.Log.Error(this, "Could not update app.", e);
                StatusMessage = CommonResourceManager.Instance.GetResourceString("Error_Unexpected");
            }

            IsBusy = false;
        }

        private void RunApplication(string applicationPath)
        {
            SingleInstance<App>.Cleanup();

            try
            {
                if (File.Exists(applicationPath))
                {
                    var info = new ProcessStartInfo(applicationPath);

                    var appProcess = new Process()
                    {
                        StartInfo = info
                    };

                    appProcess.Start();
                }
            }
            finally
            {
                Application.Current.Shutdown();
            }
        }

        private void HandleUpdaterError(UpdaterException ex)
        {
            StatusMessage = CommonResourceManager.Instance.GetEnumResource(ex.ErrorCode);
        }

        private void OnProcessingFileChanged(object sender, UpdateProcessingFileChangedEventArgs e)
        {
            StatusMessage = string.Format("{0} {1}", CommonResourceManager.Instance.GetEnumResource(e.Operation), e.FileName);
        }

        private void OnProgressChanged(object sender, UpdaterProgressChangedEventArgs e)
        {
            CurrentProgress = e.Progress;
        }

        private void OnOperationChanged(object sender, UpdaterOperationChangedEventArgs e)
        {
            CurrentProgress = 0;
            StatusMessage = CommonResourceManager.Instance.GetEnumResource(e.OperationStatus);
        }

        #endregion

        #region Commands

        public ReactiveCommand CancelCommand { get; private set; }

        public ReactiveCommand UpdateCommand { get; private set; }

        #endregion       
    }
}