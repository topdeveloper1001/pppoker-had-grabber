//-----------------------------------------------------------------------
// <copyright file="ShowWindowViewAction.cs" company="Ace Poker Solutions">
// Copyright © 2015 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using PPPokerCardCatcher.Common.Wpf.Controls;
using PPPokerCardCatcher.Common.Wpf.Interactivity;
using Microsoft.Practices.ServiceLocation;
using Prism.Interactivity.InteractionRequest;
using System;
using System.Windows;

namespace PPPokerCardCatcher.Common.Wpf.Actions
{
    public class ShowWindowViewAction : ShowViewActionBase<PCCWindow>
    {
        protected override void Activate(PCCWindow window)
        {
            if (window == null)
            {
                return;
            }

            if (!window.IsVisible)
            {
                window.Show();
                return;
            }

            window.Activate();
        }

        protected override void CloseWindow(PCCWindow window)
        {
            if (window != null)
            {
                window.Close();
            }
        }

        protected override PCCWindow CreateWindow(INotification context)
        {
            var window = new PCCWindow
            {
                Title = context != null ? context.Title : string.Empty
            };

            if (StartupLocation == StartupLocationOption.CenterScreen)
            {
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }

            return window;
        }

        protected override void OnClosed(PCCWindow window, Action callback)
        {
            EventHandler handler = null;

            handler = (o, e) =>
            {
                NonTopmostPopup.DisableTopMost = false;
                window.Closed -= handler;
                window.Content = null;

                if (SingleOnly)
                {
                    var windowController = ServiceLocator.Current.GetInstance<IWindowController>();
                    windowController.RemoveWindow(ViewName);
                }

                callback?.Invoke();
            };

            window.Closed += handler;
        }

        protected override void SetWindowPosition(PCCWindow window, double left, double top)
        {
            if (window == null)
            {
                return;
            }

            window.Left = left;
            window.Top = top;
        }

        protected override void Show(PCCWindow window)
        {
            NonTopmostPopup.DisableTopMost = true;

            if (SingleOnly)
            {
                var windowController = ServiceLocator.Current.GetInstance<IWindowController>();
                windowController.AddWindow(ViewName, window, () => window.Close());
            }

            if (IsModal)
            {
                window.Owner = Application.Current.MainWindow;
                window.ShowDialog();
                return;
            }

            window.Show();
        }
    }
}