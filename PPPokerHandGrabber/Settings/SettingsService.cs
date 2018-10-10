//-----------------------------------------------------------------------
// <copyright file="SettingsService.cs" company="Ace Poker Solutions">
// Copyright © 2017 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using PPPokerHandGrabber.Common.Extensions;
using PPPokerHandGrabber.Common.Log;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Xml;

namespace PPPokerHandGrabber.Settings
{
    internal class SettingsService : ISettingsService
    {
        private const string settingsFileName = "Settings.xml";

        private static readonly ReaderWriterLockSlim rwlock = new ReaderWriterLockSlim();

        private SettingsModel settingsModel;

        public SettingsService()
        {
            Initialize();
        }

        public SettingsModel GetSettings()
        {
            using (rwlock.Read())
            {
                return settingsModel.Clone();
            }
        }

        public void SaveSettings(SettingsModel settings)
        {
            if (settings == null)
            {
                return;
            }

            using (rwlock.Write())
            {
                settingsModel = settings.Clone();

                try
                {
                    var serializer = new DataContractSerializer(typeof(SettingsModel));

                    using (var fs = File.Open(settingsFileName, FileMode.Create))
                    {
                        using (var writer = new XmlTextWriter(fs, Encoding.UTF8))
                        {
                            writer.Formatting = Formatting.Indented;
                            serializer.WriteObject(writer, settingsModel);
                        }
                    }
                }
                catch (Exception e)
                {
                    LogProvider.Log.Error(this, e);
                }
            }
        }

        private void Initialize()
        {
            try
            {
                if (!TryLoad())
                {
                    SaveSettings(new SettingsModel());
                }
            }
            catch (Exception e)
            {
                LogProvider.Log.Error(this, "Could not initialize settings", e);
            }
        }

        private bool TryLoad()
        {
            try
            {
                using (rwlock.Write())
                {
                    if (File.Exists(settingsFileName))
                    {
                        using (var fs = File.Open(settingsFileName, FileMode.Open))
                        {
                            var serializer = new DataContractSerializer(typeof(SettingsModel));
                            settingsModel = serializer.ReadObject(fs) as SettingsModel;
                            return settingsModel != null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogProvider.Log.Error(this, "Could not load settings.", ex);
            }

            return false;
        }
    }
}