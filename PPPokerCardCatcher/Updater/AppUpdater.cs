//-----------------------------------------------------------------------
// <copyright file="AppUpdater.cs" company="Ace Poker Solutions">
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
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PPPokerCardCatcher.Updater.Core
{
    internal interface IAppUpdater : IDisposable
    {
        bool CheckIsUpdateAvailable(string guid, string applicationPath);

        ApplicationInfo CheckIsUpdateAvailable(string guid, string applicationPath, out bool isUpdateAvailable);

        Task<DirectoryInfo> UpdateApplicationAsync(string guid, string applicationPath, bool doNotMoveData);

        Task InitializeAsync(string pathToUpdatingData = null, X509Certificate2 assemblyCertificate = null);

        bool IsInitialized { get; }

        event EventHandler<UpdaterProgressChangedEventArgs> ProgressChanged;
        event EventHandler<UpdaterOperationChangedEventArgs> OperationChanged;
        event EventHandler<UpdateProcessingFileChangedEventArgs> UnzippingFileChanged;
        event EventHandler<UpdateProcessingFileChangedEventArgs> CopyingFileChanged;

        Operation CurrentOperation { get; }
    }

    /// <summary>
    /// Application updater
    /// </summary>
    internal class AppUpdater : IAppUpdater
    {
        private readonly IApplicationInfoLoader loader;
        private X509Certificate2 assemblyCertificate;

        public AppUpdater(IApplicationInfoLoader loader)
        {
            this.loader = loader;
            this.currentOperation = new Operation(OnOperationChanged);
        }

        #region Events

        public event EventHandler<UpdaterProgressChangedEventArgs> ProgressChanged;
        public event EventHandler<UpdaterOperationChangedEventArgs> OperationChanged;
        public event EventHandler<UpdateProcessingFileChangedEventArgs> UnzippingFileChanged;
        public event EventHandler<UpdateProcessingFileChangedEventArgs> CopyingFileChanged;

        #endregion

        private ApplicationInfo[] applicationInfo;

        private bool isInitialized;
        public bool IsInitialized
        {
            get
            {
                return isInitialized;
            }
        }

        private readonly Operation currentOperation;
        public Operation CurrentOperation
        {
            get
            {
                return currentOperation;
            }
        }

        #region Public

        public async virtual Task InitializeAsync(string pathToUpdatingData = null, X509Certificate2 assemblyCertificate = null)
        {
            this.assemblyCertificate = assemblyCertificate;

            // clear old data 
            Clear();

            if (string.IsNullOrWhiteSpace(pathToUpdatingData))
                pathToUpdatingData = UpdaterPaths.GetUpdaterDataPath();
            else
                pathToUpdatingData = UpdaterPaths.GetUpdatePath(pathToUpdatingData);

            using (var operationScope = new OperationScope(currentOperation, OperationStatus.Getting))
            {
                var response = await loader.LoadApplicationInfoAsync(pathToUpdatingData);

                applicationInfo = response.ToArray();
                isInitialized = true;
            }
        }

        public virtual bool CheckIsUpdateAvailable(string guid, string applicationPath)
        {
            bool isUpdateAvailable;

            CheckIsUpdateAvailable(guid, applicationPath, out isUpdateAvailable);

            return isUpdateAvailable;
        }

        public virtual ApplicationInfo CheckIsUpdateAvailable(string guid, string applicationPath, out bool isUpdateAvailable)
        {
            isUpdateAvailable = false;

            if (!isInitialized)
            {
                return null;
            }

            var assemblyName = AssemblyName.GetAssemblyName(applicationPath);

            var appInfo = GetApplicationUpdateInfo(guid, assemblyName.Version);

            isUpdateAvailable = appInfo != null;

            return appInfo;
        }

        public async virtual Task<DirectoryInfo> UpdateApplicationAsync(string guid, string applicationPath, bool doNotMoveData)
        {
            if (!isInitialized)
            {
                return null;
            }

            var assemblyName = AssemblyName.GetAssemblyName(applicationPath);

            // Get update info
            var appInfo = GetApplicationUpdateInfo(guid, assemblyName.Version);

            if (appInfo == null)
            {
                return null;
            }

            // Download update
            var updatePath = await DownloadUpdateAsync(appInfo);

            // Compute and compare hashes
            string md5hash = await GetMD5HashFromFile(updatePath);

            if (appInfo.Version.Md5hash != md5hash)
                throw new UpdaterException(UpdaterError.InvalidHash);

            var unpackedDir = await Unzip(updatePath);

            if (!doNotMoveData)
            {
                // move data
                await Move(unpackedDir);
            }

            return unpackedDir;
        }

        public void Dispose()
        {
            Clear();
        }

        #endregion

        #region Infrastructure

        protected virtual void ProgressReport(int progress)
        {
            ProgressChanged?.Invoke(this, new UpdaterProgressChangedEventArgs(progress, currentOperation.OperationStatus));
        }

        protected virtual void OnOperationChanged(OperationStatus operationStatus)
        {
            OperationChanged?.Invoke(this, new UpdaterOperationChangedEventArgs(operationStatus));
        }

        protected virtual void OnUnzippingFileChanged(string fileName)
        {
            UnzippingFileChanged?.Invoke(this, new UpdateProcessingFileChangedEventArgs(fileName, OperationStatus.Unzipping));
        }

        protected virtual void OnCopyingFileChanged(string fileName)
        {
            CopyingFileChanged?.Invoke(this, new UpdateProcessingFileChangedEventArgs(fileName, OperationStatus.Copying));
        }

        protected virtual ApplicationInfo GetApplicationUpdateInfo(string guid, Version appVersion)
        {
            Version appInfoVersion;

            var newerAppInfos = applicationInfo
                .Where(x => x.Guid.Equals(guid, StringComparison.InvariantCultureIgnoreCase) &&
                    Version.TryParse(x.Version.Version, out appInfoVersion) &&
                    appInfoVersion > appVersion)
                .OrderByDescending(x => x.Version.Version)
                .ToArray();

            var appInfo = newerAppInfos.FirstOrDefault();

            if (appInfo != null)
            {
                appInfo.VersionsSinceLastUpdate = newerAppInfos.Select(x => x.Version).ToArray();
            }

            return appInfo;
        }

        protected async virtual Task<string> DownloadUpdateAsync(ApplicationInfo appInfo)
        {
            if (appInfo == null)
            {
                throw new ArgumentNullException("appInfo");
            }

            DirectoryInfo dir = new DirectoryInfo(UpdaterPaths.GetTempUpdatesFolder());

            if (!dir.Exists)
            {
                dir.Create();
            }

            string pathToCopyUpdate = Path.Combine(dir.FullName, appInfo.Version.Md5hash);

            using (var operationScope = new OperationScope(currentOperation, OperationStatus.Downloading))
            {
                try
                {
                    using (var webClient = new WebClient())
                    {
                        using (var stream = await webClient.OpenReadTaskAsync(UpdaterPaths.GetUpdatePath(appInfo.Version.Path)))
                        {
                            int bytesTotal = Convert.ToInt32(webClient.ResponseHeaders["Content-Length"]);

                            using (FileStream fs = new FileStream(pathToCopyUpdate, FileMode.Create))
                            {
                                int bytesRead = 0;
                                int bytesLoaded = 0;
                                byte[] buffer = new byte[4096];

                                do
                                {
                                    bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                                    fs.Write(buffer, 0, bytesRead);

                                    bytesLoaded += bytesRead;

                                    double progress = (double)bytesLoaded / bytesTotal * 100;
                                    ProgressReport((int)progress);

                                }
                                while (bytesRead > 0);
                            }
                        }
                    }

                    return pathToCopyUpdate;
                }
                catch
                {
                    throw new UpdaterException(UpdaterError.DownloadingFailed);
                }
            }
        }

        protected virtual Task<string> GetMD5HashFromFile(string fileName)
        {
            return Task.Run(() =>
            {
                try
                {
                    using (FileStream file = new FileStream(fileName, FileMode.Open))
                    {
                        MD5 md5 = new MD5CryptoServiceProvider();
                        byte[] retVal = md5.ComputeHash(file);

                        StringBuilder sb = new StringBuilder();

                        for (int i = 0; i < retVal.Length; i++)
                        {
                            sb.Append(retVal[i].ToString("x2"));
                        }

                        return sb.ToString();
                    }
                }
                catch
                {
                    throw new UpdaterException(UpdaterError.ComputingHashingFailed);
                }
            });
        }

        private async Task<DirectoryInfo> Unzip(string zipFile)
        {
            using (var operationScope = new OperationScope(currentOperation, OperationStatus.Unzipping))
            {
                try
                {
                    DirectoryInfo dir = new DirectoryInfo(UpdaterPaths.GetTempUnpackedFolder());

                    if (!dir.Exists)
                        dir.Create();

                    using (var zipFileStream = File.OpenRead(zipFile))
                    {
                        using (var archive = new ZipArchive(zipFileStream))
                        {
                            foreach (ZipArchiveEntry entry in archive.Entries)
                            {
                                if (string.IsNullOrWhiteSpace(entry.Name))
                                    continue;

                                OnUnzippingFileChanged(entry.Name);

                                var directoryPath = entry.FullName.Remove(entry.FullName.IndexOf(entry.Name));

                                if (!string.IsNullOrWhiteSpace(directoryPath))
                                    Directory.CreateDirectory(Path.Combine(dir.FullName, directoryPath));

                                string fileName = Path.Combine(dir.FullName, entry.FullName.Replace("/", @"\"));

                                using (var newFileStream = File.Create(fileName))
                                {
                                    Stream fileData = entry.Open();

                                    int bytesRead = 0;
                                    int bytesLoaded = 0;
                                    byte[] buffer = new byte[4096];

                                    do
                                    {
                                        bytesRead = await fileData.ReadAsync(buffer, 0, buffer.Length);
                                        await newFileStream.WriteAsync(buffer, 0, bytesRead);

                                        bytesLoaded += bytesRead;

                                        double progress = (double)bytesLoaded / entry.Length * 100;
                                        ProgressReport((int)progress);

                                    }
                                    while (bytesRead > 0);

                                    await newFileStream.FlushAsync();
                                }
                            }
                        }
                    }

                    return dir;
                }
                catch
                {
                    throw new UpdaterException(UpdaterError.UnzippingFailed);
                }
            }
        }

        protected virtual async Task Move(DirectoryInfo unpackedDir)
        {
            using (var operationScope = new OperationScope(currentOperation, OperationStatus.Copying))
            {
                foreach (var file in unpackedDir.GetFiles())
                    await MoveFileAsync(file);

                foreach (var directory in unpackedDir.GetDirectories())
                    await Move(directory);
            }
        }

        protected async Task MoveFileAsync(FileInfo file)
        {
            try
            {
                var tempPath = UpdaterPaths.GetTempUnpackedFolder() + "\\";
                var newFilePath = file.FullName.Remove(file.FullName.IndexOf(tempPath), tempPath.Length);
                var directoryPath = newFilePath.Remove(newFilePath.IndexOf(file.Name));

                if (!Directory.Exists(directoryPath) && !string.IsNullOrWhiteSpace(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                using (var oldFileStream = file.OpenRead())
                {
                    using (var newFileStream = File.Create(newFilePath))
                    {
                        OnCopyingFileChanged(file.Name);

                        int bytesRead = 0;
                        int bytesLoaded = 0;
                        byte[] buffer = new byte[4096];

                        do
                        {
                            bytesRead = await oldFileStream.ReadAsync(buffer, 0, buffer.Length);
                            await newFileStream.WriteAsync(buffer, 0, bytesRead);

                            bytesLoaded += bytesRead;

                            double progress = (double)bytesLoaded / (double)file.Length * 100;
                            ProgressReport((int)progress);

                        }
                        while (bytesRead > 0);

                        await newFileStream.FlushAsync();
                    }
                }
            }
            catch
            {
                throw new UpdaterException(UpdaterError.CopyingFailed);
            }
        }

        private void Clear()
        {
            DirectoryInfo dir = new DirectoryInfo(UpdaterPaths.GetTempUpdatesFolder());

            try
            {
                if (dir.Exists)
                {
                    dir.Delete(true);
                }
            }
            catch
            {
            }
        }

        #endregion
    }
}