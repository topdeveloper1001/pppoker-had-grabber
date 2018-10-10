//-----------------------------------------------------------------------
// <copyright file="RegistrationPopupRequestInfo.cs" company="Ace Poker Solutions">
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
    internal class RegistrationPopupRequestInfo : INotification
    {
        public RegistrationPopupRequestInfo(bool showRegister)
        {
            Title = CommonResourceManager.Instance.GetResourceString("Common_RegistrationView_Title");
            Content = showRegister;
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