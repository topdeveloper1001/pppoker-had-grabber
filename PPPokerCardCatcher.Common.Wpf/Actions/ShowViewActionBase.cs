//-----------------------------------------------------------------------
// <copyright file="ShowViewActionBase.cs" company="Ace Poker Solutions">
// Copyright © 2017 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using PPPokerCardCatcher.Common.Log;
using PPPokerCardCatcher.Common.Wpf.Interactivity;
using PPPokerCardCatcher.Common.Wpf.Mvvm;
using Microsoft.Practices.ServiceLocation;
using Prism.Interactivity.InteractionRequest;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace PPPokerCardCatcher.Common.Wpf.Actions
{
    public abstract class ShowViewActionBase<T> : TriggerAction<FrameworkElement>
        where T : ContentControl
    {
        #region Dependency Properties

        /// <summary>
        /// The name of view of the child window to display
        /// </summary>
        public static readonly DependencyProperty ViewNameProperty = DependencyProperty.Register(
            "ViewName",
            typeof(string),
            typeof(ShowViewActionBase<T>),
            new UIPropertyMetadata(null));

        /// <summary>
        /// Determines if the content should be shown in a modal window or not.
        /// </summary>
        public static readonly DependencyProperty IsModalProperty = DependencyProperty.Register(
                "IsModal",
                typeof(bool),
                typeof(ShowViewActionBase<T>),
                new PropertyMetadata(null));

        /// <summary>
        /// Determines if only one instance of window is allowed
        /// </summary>
        public static readonly DependencyProperty SingleOnlyProperty = DependencyProperty.Register(
                "SingleOnly",
                typeof(bool),
                typeof(ShowViewActionBase<T>),
                new PropertyMetadata(null));

        // Style of the window
        public static readonly DependencyProperty WindowStyleProperty =
            DependencyProperty.Register("WindowStyle",
                typeof(Style),
                typeof(ShowViewActionBase<T>),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the name of view of the content of the window
        /// </summary>
        public string ViewName
        {
            get { return (string)GetValue(ViewNameProperty); }
            set { SetValue(ViewNameProperty, value); }
        }

        /// <summary>
        /// Gets or sets if the window will be modal or not.
        /// </summary>
        public bool IsModal
        {
            get { return (bool)GetValue(IsModalProperty); }
            set { SetValue(IsModalProperty, value); }
        }

        /// <summary>
        /// Gets or sets if the only one instance of window is allowed
        /// </summary>
        public bool SingleOnly
        {
            get { return (bool)GetValue(SingleOnlyProperty); }
            set { SetValue(SingleOnlyProperty, value); }
        }

        /// <summary>
        /// Gets or sets style of the window
        /// </summary>
        public Style WindowStyle
        {
            get { return (Style)GetValue(WindowStyleProperty); }
            set { SetValue(WindowStyleProperty, value); }
        }

        /// <summary>
        ///  Gets or sets the startup location of window
        /// </summary>
        public StartupLocationOption StartupLocation
        {
            get;
            set;
        }

        #endregion

        #region Trigger action infrastructure

        /// <summary>
        /// Invokes action
        /// </summary>
        /// <param name="parameter"></param>
        protected override void Invoke(object parameter)
        {
            if (string.IsNullOrEmpty(ViewName))
            {
                return;
            }

            var args = parameter as InteractionRequestedEventArgs;

            if (args == null)
            {
                return;
            }

            // activate window if it has been created already
            if (SingleOnly)
            {
                var windowController = ServiceLocator.Current.GetInstance<IWindowController>();
                var existingWindow = windowController.GetWindow(ViewName) as T;

                if (existingWindow != null)
                {
                    Activate(existingWindow);
                    return;
                }
            }

            var view = ServiceLocator.Current.GetInstance<IViewModelContainer>(ViewName);

            if (view == null)
            {
                return;
            }

            var window = CreateWindow(args.Context);

            if (WindowStyle != null)
            {
                window.Style = WindowStyle;
            }

            window.Content = view;

            var viewModel = ViewModelContainerHelper.GetViewModelAs<ICloseableViewModel>(view);

            if (viewModel != null)
            {
                viewModel.Closed += (s, e) => CloseWindow(window);

                if (args.Context != null && viewModel is IConfigurableViewModel)
                {
                    ((IConfigurableViewModel)viewModel).Configure(args.Context.Content);
                }
            }

            var callback = args.Callback;

            OnClosed(window, callback);

            if (StartupLocation == StartupLocationOption.CenterAssosiated &&
                AssociatedObject != null)
            {
                // If we should center the pop-up over the parent window we subscribe to the SizeChanged event
                // so we can change its position after the dimensions are set.
                SizeChangedEventHandler sizeHandler = null;

                sizeHandler = (o, e) =>
                {
                    window.SizeChanged -= sizeHandler;

                    try
                    {
                        var position = AssociatedObject.PointToScreen(new Point(0, 0));

                        var top = position.Y + ((AssociatedObject.ActualHeight - window.ActualHeight) / 2);
                        var left = position.X + ((AssociatedObject.ActualWidth - window.ActualWidth) / 2);

                        SetWindowPosition(window, left, top);
                    }
                    catch (Exception ex)
                    {
                        LogProvider.Log.Error(this, $"Window couldn't be centered", ex);
                    }
                };

                window.SizeChanged += sizeHandler;
            }

            Show(window);
        }

        #endregion

        protected abstract T CreateWindow(INotification context);

        protected abstract void Activate(T window);

        protected abstract void CloseWindow(T window);

        protected abstract void Show(T window);

        protected abstract void SetWindowPosition(T window, double left, double top);

        protected abstract void OnClosed(T window, Action callback);
    }
}