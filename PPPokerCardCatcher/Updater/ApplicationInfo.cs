//-----------------------------------------------------------------------
// <copyright file="ApplicationsInfo.cs" company="Ace Poker Solutions">
// Copyright © 2015 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using System.Collections.Generic;
using System.Xml.Serialization;

namespace PPPokerCardCatcher.Updater.Core
{
    [XmlRoot("applications")]
    public class ApplicationsInfo
    {
        [XmlElement("application")]
        public ApplicationInfo[] ApplicationInfo { get; set; }
    }

    public class ApplicationInfo
    {
        [XmlAttribute("name")]
        public string ApplicationName { get; set; }

        [XmlAttribute("guid")]
        public string Guid { get; set; }

        [XmlElement("version", typeof(VersionInfo))]
        public VersionInfo Version { get; set; }

        [XmlIgnore]
        public IEnumerable<VersionInfo> VersionsSinceLastUpdate { get; set; }
    }

    public class VersionInfo
    {
        [XmlAttribute("version")]
        public string Version { get; set; }

        [XmlAttribute("path")]
        public string Path { get; set; }

        [XmlAttribute("md5hash")]
        public string Md5hash { get; set; }

        [XmlAttribute("isCritical")]
        public bool IsCritical { get; set; }

        [XmlElement("notes")]
        public string ReleaseNotes { get; set; }
    }
}