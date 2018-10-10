//-----------------------------------------------------------------------
// <copyright file="CommonResourceManager.cs" company="Ace Poker Solutions">
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
using System.Linq;
using System.Globalization;

namespace PPPokerHandGrabber.Common.Resources
{
    /// <summary>
    /// Resources manager.
    /// </summary>
    public class CommonResourceManager : IResourceManager
    {
        #region Singleton implementation

        // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
        static CommonResourceManager()
        {
        }

        private CommonResourceManager()
        {
        }

        private static readonly CommonResourceManager instance = new CommonResourceManager();

        public static CommonResourceManager Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion

        private readonly List<IResourceManager> resourceManagers = new List<IResourceManager>();
        private readonly List<IResourceManager> defaultResourceManagers = new List<IResourceManager>();

        /// <summary>
        /// Register custom resources manager.
        /// </summary>
        /// <param name="resourceManager">Register custom resources manager.</param>
        public void RegisterResourceManager(IResourceManager resourceManager)
        {
            if (!resourceManagers.Contains(resourceManager))
            {
                resourceManagers.Add(resourceManager);
            }
        }

        /// <summary>
        /// Register default resources manager.
        /// </summary>
        /// <param name="resourceManager">Default resources manager</param>
        public void RegisterDefaultResourceManager(IResourceManager resourceManager)
        {
            if (!defaultResourceManagers.Contains(resourceManager))
            {
                defaultResourceManagers.Add(resourceManager);
            }
        }

        #region IResourceManager Members

        private IEnumerable<IResourceManager> GetResourceManagers(string resourceKey)
        {
            return resourceManagers
                .Where(rm => rm.Match(resourceKey))
                .Union(defaultResourceManagers);
        }

        public string GetResourceString(string resourceKey)
        {
            return GetResourceString(resourceKey, null);
        }

        public string GetResourceString(string resourceKey, CultureInfo cultureInfo)
        {
            return GetResourceManagers(resourceKey)
                .Select(rm => rm.GetResourceString(resourceKey, cultureInfo))
                .FirstOrDefault(res => res != null);
        }

        public static string GetResourceStringForCompositeKey(string prefix, params object[] parameters)
        {
            string resultKey = CompositeKeyProvider.GetKey(prefix, parameters);
            return Instance.GetResourceString(resultKey);
        }

        public object GetResourceObject(string resourceKey)
        {
            return GetResourceObject(resourceKey, null);
        }

        public object GetResourceObject(string resourceKey, CultureInfo cultureInfo)
        {
            return GetResourceManagers(resourceKey)
                .Select(rm => rm.GetResourceObject(resourceKey, cultureInfo))
                .FirstOrDefault(res => res != null);
        }

        public bool Match(string resourceKey)
        {
            var managers = GetResourceManagers(resourceKey);
            return managers != null && managers.Any();
        }

        #endregion

        public string GetEnumResource(Enum enumValue)
        {
            return GetResourceString(CompositeKeyProvider.GetEnumKey(enumValue));
        }

        private string GetBoolKey(bool boolValue)
        {
            return string.Format("Entity_Bool_{0}", boolValue);
        }

        public string GetLocalizedBoolValue(bool boolValue)
        {
            return GetResourceString(GetBoolKey(boolValue));
        }
    }
}