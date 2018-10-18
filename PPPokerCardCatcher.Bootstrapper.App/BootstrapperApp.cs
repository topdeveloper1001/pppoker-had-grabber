//-----------------------------------------------------------------------
// <copyright file="BootstrapperApp.cs" company="Ace Poker Solutions">
// Copyright © 2015 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using PPPokerCardCatcher.Bootstrapper.App.ViewModels;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace PPPokerCardCatcher.Bootstrapper.App
{
    public class BootstrapperApp : BootstrapperApplication
    {
        public static Dispatcher BootstrapperDispatcher { get; private set; }

        public static MainWindow RootView { get; private set; }

        public string BundleName => Engine.StringVariables["WixBundleName"];

        protected override void Run()
        {
            try
            {
                AppDomain.CurrentDomain.UnhandledException += (s, a) => Engine.Log(LogLevel.Error, $"Critical bootstrapper exception: {a.ExceptionObject}");

                BootstrapperDispatcher = Dispatcher.CurrentDispatcher;
                BootstrapperDispatcher.UnhandledException += (s, a) => Engine.Log(LogLevel.Error, $"Critical bootstrapper exception: {a.Exception}");

                RootView = new MainWindow(this);
                RootView.Closed += (s, a) => BootstrapperDispatcher.InvokeShutdown();

                Engine.Detect();

                if (Command.Display == Display.Passive || Command.Display == Display.Full)
                {
                    RootView.Show();
                    Dispatcher.Run();
                }

                Engine.Quit(RootView.ViewModel.Status);
            }
            catch (Exception e)
            {
                Engine.Log(LogLevel.Error, $"Critical bootstrapper exception: {e}");
                throw e;
            }
        }
    }
}