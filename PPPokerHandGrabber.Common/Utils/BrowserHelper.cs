//-----------------------------------------------------------------------
// <copyright file="BrowserHelper.cs" company="Ace Poker Solutions">
// Copyright © 2015 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using PPPokerHandGrabber.Common.Log;
using Microsoft.Win32;
using System;
using System.Diagnostics;

namespace PPPokerHandGrabber.Common.Utils
{
    /// <summary>
    /// Provides methods to work with browsers
    /// </summary>
    public class BrowserHelper
    {
        /// <summary>
        /// Gets path to the executable file of the default browser of system
        /// </summary>
        /// <returns>Path to the executable file of the default browser</returns>
        public static string GetDefaultBrowserPath()
        {
            var urlAssociation = @"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http";
            var browserPathKey = @"$BROWSER$\shell\open\command";

            RegistryKey userChoiceKey = null;
            var browserPath = string.Empty;

            try
            {
                //Read default browser path from userChoiceLKey
                userChoiceKey = Registry.CurrentUser.OpenSubKey(urlAssociation + @"\UserChoice", false);

                //If user choice was not found, try machine default
                if (userChoiceKey == null)
                {
                    //Read default browser path from Win XP registry key
                    var browserKey = Registry.ClassesRoot.OpenSubKey(@"HTTP\shell\open\command", false);

                    //If browser path wasn’t found, try Win Vista (and newer) registry key
                    if (browserKey == null)
                    {
                        browserKey = Registry.CurrentUser.OpenSubKey(urlAssociation, false);
                    }

                    var path = CleanifyBrowserPath(browserKey.GetValue(null) as string);

                    browserKey.Close();

                    return path;
                }
                else
                {
                    // user defined browser choice was found
                    var progId = (userChoiceKey.GetValue("ProgId").ToString());

                    userChoiceKey.Close();

                    // now look up the path of the executable
                    var concreteBrowserKey = browserPathKey.Replace("$BROWSER$", progId);

                    var kp = Registry.ClassesRoot.OpenSubKey(concreteBrowserKey, false);

                    browserPath = CleanifyBrowserPath(kp.GetValue(null) as string);

                    kp.Close();

                    return browserPath;
                }
            }
            catch
            {
                return "iexplorer.exe";
            }
        }

        /// <summary>
        /// Opens the specified link in the default browser
        /// </summary>
        /// <param name="link">The link to open</param>
        public static void OpenLinkInBrowser(string link)
        {
            try
            {
                Process.Start(GetDefaultBrowserPath(), link);
            }
            catch (Exception e)
            {
                LogProvider.Log.Error(typeof(BrowserHelper), $"Link {link} couldn't be opened", e);
            }
        }

        private static string CleanifyBrowserPath(string p)
        {
            var url = p.Split('"');
            var clean = url[1];
            return clean;
        }
    }
}