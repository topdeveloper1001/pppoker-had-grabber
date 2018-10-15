//-----------------------------------------------------------------------
// <copyright file="UpdatePopupRequestInfo.cs" company="Ace Poker Solutions">
// Copyright © 2018 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using PPPokerCardCatcher.Common.Resources;
using PPPokerCardCatcher.Updater.Core;
using Prism.Interactivity.InteractionRequest;

namespace PPPokerCardCatcher.ViewModels
{
    internal class UpdatePopupRequestInfo : INotification
    {
        public UpdatePopupRequestInfo(ApplicationInfo applicationInfo)
        {
            Title = CommonResourceManager.Instance.GetResourceString("Common_UpdateView_Title");
            Content = applicationInfo;
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