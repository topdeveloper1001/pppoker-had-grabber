//-----------------------------------------------------------------------
// <copyright file="IOperationViewModel.cs" company="Ace Poker Solutions">
// Copyright © 2015 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using PPPokerHandGrabber.Common.Wpf.Events;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PPPokerHandGrabber.Common.Wpf.Mvvm
{
    public interface IOperationViewModel
    {
        IEnumerable<IAsyncOperation> Operations { get; }

        bool IsBusy { get; }
     
        void AddOperation(IAsyncOperation operation);        

        void SetOperationBinding<T>(T source, Expression<Func<T, object>> expression);

        void Clear();
    }
}