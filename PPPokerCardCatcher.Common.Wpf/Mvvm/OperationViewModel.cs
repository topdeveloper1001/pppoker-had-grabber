//-----------------------------------------------------------------------
// <copyright file="OperationViewModel.cs" company="Ace Poker Solutions">
// Copyright © 2015 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using PPPokerCardCatcher.Common.Reflection;
using PPPokerCardCatcher.Common.Wpf.Events;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Data;

namespace PPPokerCardCatcher.Common.Wpf.Mvvm
{
    public class OperationViewModel : ReactiveObject, INotifyPropertyChanged, IOperationViewModel
    {
        public IEnumerable<IAsyncOperation> Operations
        {
            get
            {
                return operations.Where(e => e.Operation != null).Select(e => e.Operation);
            }
        }

        public bool IsBusy
        {
            get
            {
                return Operations.Any(e => !e.IsCompleted);
            }
        }

        private void OnChanged()
        {
            ClearCompletedItems();
            this.RaisePropertyChanged(nameof(IsBusy));
        }

        private void ItemChanged(object sender, EventArgs e)
        {
            OnChanged();
        }

        #region [ Items ]

        private readonly object operationsLock = new object();

        private readonly List<IAsyncOperationItem> operations = new List<IAsyncOperationItem>();

        public void SetOperationBinding<T>(T source, Expression<Func<T, object>> expression)
        {
            SetOperationBinding(source, ReflectionHelper.GetPath(expression));
        }

        public void SetOperationBinding(object source, string path)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            var binding = new Binding(path)
            {
                Mode = BindingMode.OneWay,
                Source = source
            };

            var item = new AsyncOperationItem { IsOneTime = false };

            BindingOperations.SetBinding(item, AsyncOperationItem.OperationProperty, binding);

            AddItem(item);
        }

        public void AddOperation(IAsyncOperation operation)
        {
            var item = new AsyncOperationItem
            {
                Operation = operation,
                IsOneTime = true
            };

            AddItem(item);
        }

        private void ClearCompletedItems()
        {
            lock (operationsLock)
            {
                for (int i = operations.Count - 1; i >= 0; i--)
                {
                    if (operations[i].IsOneTime && (operations[i].Operation == null || operations[i].Operation.IsCompleted))
                    {
                        RemoveItemAt(i);
                    }
                }
            }
        }

        private void AddItem(IAsyncOperationItem item)
        {
            item.Changed += ItemChanged;

            lock (operationsLock)
            {
                operations.Add(item);
            }

            OnChanged();
        }

        public void Clear()
        {
            while (operations.Count > 0)
            {
                RemoveItemAt(0);
            }
        }

        private void RemoveItemAt(int i)
        {
            if (i < 0 || i >= operations.Count)
            {
                return;
            }

            var item = operations[i];
            item.Changed -= ItemChanged;
            item.Clear();
            operations.RemoveAt(i);
        }

        #endregion       
    }       
}