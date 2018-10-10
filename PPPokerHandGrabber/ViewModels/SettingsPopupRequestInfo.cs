//-----------------------------------------------------------------------
// <copyright file="SettingsPopupRequestInfo.cs" company="Ace Poker Solutions">
// Copyright © 2017 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using PPPokerHandGrabber.Common.Resources;
using Prism.Interactivity.InteractionRequest;

namespace PPPokerHandGrabber.ViewModels
{
    internal class SettingsPopupRequestInfo : INotification
    {
        public SettingsPopupRequestInfo()
        {
            Title = CommonResourceManager.Instance.GetResourceString("Common_Settings_Title");
        }

        public object Content
        {
            get;
            set;
        }

        public string Title
        {
            get; set;
        }
    }
}