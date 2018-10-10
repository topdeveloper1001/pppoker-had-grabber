//-----------------------------------------------------------------------
// <copyright file="ViewModelContainerHelper.cs" company="Ace Poker Solutions">
// Copyright © 2017 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using PPPokerHandGrabber.Common.Wpf.Actions;
using System.Linq;

namespace PPPokerHandGrabber.Common.Wpf.Mvvm
{
    public static class ViewModelContainerHelper
    {
        public static object GetViewModel(object view)
        {            
            object result = null;

            var vmContainerType = view.GetType().GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition().IsAssignableFrom(typeof(IViewModelContainer<>)));

            if (vmContainerType != null)
            {
                result = vmContainerType.GetProperty(nameof(IViewModelContainer<ViewModelBase>.ViewModel)).GetValue(view, null);
            }

            return result;
        }

        public static T GetViewModelAs<T>(object view) where T : class
        {
            return GetViewModel(view) as T;
        }
    }
}