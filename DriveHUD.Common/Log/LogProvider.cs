//-----------------------------------------------------------------------
// <copyright file="LogProvider.cs" company="Ace Poker Solutions">
// Copyright © 2015 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using log4net;
using log4net.Config;
using log4net.Util;
using System;
using System.IO;

namespace DriveHUD.Common.Log
{
    /// <summary>
    /// Log provider
    /// </summary>
    public static class LogProvider
    {
        private static ILog mainLog;

        private static IDHLog internalLog = new InternalLog();

        static LogProvider()
        {
            GlobalContext.Properties["log.ApplicationDataFolder"] = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            var appConfigPath = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            var appSetting = SystemInfo.GetAppSetting("log4net.Config");

            LogConfigPath = Path.Combine(Path.GetDirectoryName(appConfigPath), appSetting ?? string.Empty);

            if (!File.Exists(LogConfigPath))
            {
                LogConfigPath = appConfigPath;
            }

            XmlConfigurator.ConfigureAndWatch(new FileInfo(LogConfigPath));

            mainLog = LogManager.GetLogger("MainLog");           
        }
     
        /// <summary>
        /// Log class
        /// </summary>
        public static IDHLog Log
        {
            get
            {
                return internalLog;
            }
        }

        /// <summary>
        /// Path to log configurations
        /// </summary>
        public static string LogConfigPath
        {
            get;
            private set;
        }

        /// <summary>
        /// Set default logger
        /// </summary>
        /// <param name="alias">Name of default logger</param>
        public static void SetDefaultLogger(string alias)
        {
            mainLog = LogManager.GetLogger(alias);
        }

        /// <summary>
        /// Set custom logger
        /// </summary>
        /// <param name="customLog">Custom logger</param>
        public static void SetCustomLogger(IDHLog customLog)
        {
            internalLog = customLog;
        }

        /// <summary>
        /// IDHLog implementation via log4net methods
        /// </summary>
        private class InternalLog : IDHLog
        {
            public void Log(Type senderType, object message, LogMessageType logMessageType)
            {
                ILog log = senderType == null ? mainLog : LogManager.GetLogger(senderType);

                switch (logMessageType)
                {
                    case LogMessageType.Debug:
                        log.Debug(message);
                        break;
                    case LogMessageType.Info:
                        log.Info(message);
                        break;
                    case LogMessageType.Warning:
                        log.Warn(message);
                        break;
                    case LogMessageType.Error:
                        log.Error(message);
                        break;
                    case LogMessageType.FatalError:
                        log.Fatal(message);
                        break;
                }
            }

            public void Log(string loggerName, object message, LogMessageType logMessageType)
            {
                ILog log = string.IsNullOrEmpty(loggerName) ? mainLog : LogManager.GetLogger(loggerName);

                switch (logMessageType)
                {
                    case LogMessageType.Debug:
                        log.Debug(message);
                        break;
                    case LogMessageType.Info:
                        log.Info(message);
                        break;
                    case LogMessageType.Warning:
                        log.Warn(message);
                        break;
                    case LogMessageType.Error:
                        log.Error(message);
                        break;
                    case LogMessageType.FatalError:
                        log.Fatal(message);
                        break;
                }
            }

            public void Log(Type senderType, object message, Exception exception, LogMessageType logMessageType)
            {
                ILog log = senderType == null ? mainLog : LogManager.GetLogger(senderType);

                switch (logMessageType)
                {
                    case LogMessageType.Debug:
                        log.Debug(message, exception);
                        break;
                    case LogMessageType.Info:
                        log.Info(message, exception);
                        break;
                    case LogMessageType.Warning:
                        log.Warn(message, exception);
                        break;
                    case LogMessageType.Error:
                        log.Error(message, exception);
                        break;
                    case LogMessageType.FatalError:
                        log.Fatal(message, exception);
                        break;
                }
            }

            public void Log(string loggerName, object message, Exception exception, LogMessageType logMessageType)
            {
                ILog log = string.IsNullOrEmpty(loggerName) ? mainLog : LogManager.GetLogger(loggerName);

                switch (logMessageType)
                {
                    case LogMessageType.Debug:
                        log.Debug(message, exception);
                        break;
                    case LogMessageType.Info:
                        log.Info(message, exception);
                        break;
                    case LogMessageType.Warning:
                        log.Warn(message, exception);
                        break;
                    case LogMessageType.Error:
                        log.Error(message, exception);
                        break;
                    case LogMessageType.FatalError:
                        log.Fatal(message, exception);
                        break;
                }
            }
        }
    }
}