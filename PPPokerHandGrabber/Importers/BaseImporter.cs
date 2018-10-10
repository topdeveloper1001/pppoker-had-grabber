//-----------------------------------------------------------------------
// <copyright file="BaseImporter.cs" company="Ace Poker Solutions">
// Copyright © 2018 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using PPPokerHandGrabber.Common.Log;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PPPokerHandGrabber.Importers
{
    internal abstract class BaseImporter : IBaseImporter
    {
        protected bool isRunning;

        protected CancellationTokenSource cancellationTokenSource;

        public bool IsRunning
        {
            get
            {
                return isRunning;
            }
        }

        public abstract string ImporterName { get; }

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

            LogProvider.Log.Info(this, $"Starting \"{ImporterName}\" importer");

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
                LogProvider.Log.Info(this, $"Stopping \"{ImporterName}\" importer");
            }

            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
            }
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

            LogProvider.Log.Info(this, $"\"{ImporterName}\" importer has been stopped");
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