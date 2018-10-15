//-----------------------------------------------------------------------
// <copyright file="WindowController.cs" company="Ace Poker Solutions">
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
using System.Threading;

namespace PPPokerCardCatcher.Common.Wpf.Interactivity
{
    public class WindowController : IWindowController
    {
        private readonly Dictionary<string, InternalWindow> windows = new Dictionary<string, InternalWindow>();

        private readonly ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();

        public void AddWindow(string viewName, object window, Action close)
        {
            try
            {
                rwLock.EnterWriteLock();

                if (!windows.ContainsKey(viewName))
                {
                    var internalWindow = new InternalWindow
                    {
                        Window = window,
                        Close = close
                    };

                    windows.Add(viewName, internalWindow);
                }
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public object GetWindow(string viewName)
        {
            try
            {
                rwLock.EnterReadLock();

                if (windows.ContainsKey(viewName))
                {
                    return windows[viewName].Window;
                }
            }
            finally
            {
                rwLock.ExitReadLock();
            }

            return null;
        }

        public void CloseAllWindows()
        {
            foreach (var window in windows.Values)
            {
                window?.Close();
            }
        }

        public void RemoveWindow(string viewName)
        {
            try
            {
                rwLock.EnterWriteLock();

                if (windows.ContainsKey(viewName))
                {
                    var window = windows[viewName];
                    window?.Close();

                    windows.Remove(viewName);
                }
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        private class InternalWindow
        {
            public object Window { get; set; }

            public Action Close { get; set; }
        }
    }
}