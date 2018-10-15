//-----------------------------------------------------------------------
// <copyright file="AsyncOperationItem.cs" company="Ace Poker Solutions">
// Copyright © 2015 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using PPPokerCardCatcher.Common.Wpf.Events;
using System;
using System.Windows;
using System.Windows.Data;

namespace PPPokerCardCatcher.Common.Wpf.Mvvm
{
    internal class AsyncOperationItem : DependencyObject, IAsyncOperationItem
    {
        private bool isOneTime = true;

        public bool IsOneTime
        {
            get
            {
                return isOneTime;
            }
            set
            {
                isOneTime = value;
            }
        }

        public IAsyncOperation Operation
        {
            get
            {
                return (IAsyncOperation)GetValue(OperationProperty);
            }
            set
            {
                SetValue(OperationProperty, value);
            }
        }

        public static DependencyProperty OperationProperty = DependencyProperty.Register(
            "Operation", typeof(IAsyncOperation), typeof(AsyncOperationItem), new PropertyMetadata(null, (s, e) =>
            {
                var instance = s as AsyncOperationItem;

                if (instance == null)
                {
                    return;
                }

                var oldOperation = e.OldValue as IAsyncOperation;

                if (oldOperation != null)
                {
                    oldOperation.Completed -= instance.OperationCompleted;
                }

                var newOperation = e.NewValue as IAsyncOperation;

                if (newOperation != null)
                {
                    newOperation.Completed += instance.OperationCompleted;
                }

                instance.OnChanged();
            }));

        private void OperationCompleted(object sender, EventArgs e)
        {
            OnChanged();
        }

        public event EventHandler Changed;

        protected virtual void OnChanged()
        {
            Changed?.Invoke(this, EventArgs.Empty);
        }

        public void Clear()
        {
            BindingOperations.ClearBinding(this, OperationProperty);
            Operation = null;
        }
    }
}