//-----------------------------------------------------------------------
// <copyright file="BaseImporter.cs" company="Ace Poker Solutions">
// Copyright © 2015 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using DriveHUD.Common.Log;
using DriveHUD.Entities;
using Microsoft.Practices.ServiceLocation;
//using Model.Settings;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DriveHUD.Importers
{
    internal abstract class BaseImporter : IBackgroundProcess, IBaseImporter
    {
        protected bool isRunning;

        protected CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// Importer site (PS, iPoker, Bovada, etc.)
        /// </summary>
        protected abstract EnumPokerSites Site { get; }

        /// <summary>
        /// Importer site string (PS, iPoker, Bovada, etc.)
        /// </summary>
        public virtual string SiteString
        {
            get
            {
                return Site.ToString();
            }
        }

        public bool IsRunning
        {
            get
            {
                return isRunning;
            }
        }

        #region IBackgroundProcess implementation

        public event EventHandler ProcessStopped;

        /// <summary>
        /// Starts importing hands from poker client
        /// </summary>
        /// <param name="configuration"></param>
        public virtual void Start()
        {
            cancellationTokenSource = new CancellationTokenSource();

            if (isRunning)
            {
                return;
            }

            if (IsDisabled())
            {
                LogProvider.Log.Info(this, string.Format(CultureInfo.InvariantCulture, "\"{0}\" importer is disabled.", SiteString));
                return;
            }

            LogProvider.Log.Info(this, string.Format(CultureInfo.InvariantCulture, "Starting \"{0}\" importer", SiteString));

            // start main job
            Task.Run(() =>
            {
                isRunning = true;
                DoImport();
            }, cancellationTokenSource.Token);
        }

        /// <summary>
        /// Stop importing, send cancellation request to stop internal thread
        /// </summary>
        public virtual void Stop()
        {
            if (IsRunning)
            {
                LogProvider.Log.Info(this, string.Format(CultureInfo.InvariantCulture, "Stopping \"{0}\" importer", SiteString));
            }

            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
            }
        }

        public virtual bool IsDisabled()
        {
            //var settings = ServiceLocator.Current.GetInstance<ISettingsService>().GetSettings();
            //var siteSettings = settings.SiteSettings.SitesModelList?.FirstOrDefault(x => x.PokerSite == Site);

            //return siteSettings != null && !siteSettings.Enabled;
            return false;
        }

        #endregion

        #region Infrastructure

        /// <summary>
        /// Imports data from different sources
        /// </summary>
        protected abstract void DoImport();

        /// <summary>
        /// Raise process stopped event
        /// </summary>
        protected void RaiseProcessStopped()
        {
            isRunning = false;

            ProcessStopped?.Invoke(this, EventArgs.Empty);

            LogProvider.Log.Info(this, $"\"{SiteString}\" importer has been stopped");
        }
      
        #endregion

        #region IDisposable implementation

        private bool disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources here.                    
                    Disposing();
                }

                // Clear unmanaged resources here
                disposed = true;
            }
        }

        protected virtual void Disposing()
        {
        }

        ~BaseImporter()
        {
            Dispose(false);
        }

        #endregion
    }
}
