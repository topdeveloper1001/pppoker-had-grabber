//-----------------------------------------------------------------------
// <copyright file="WindowViewModelBase.cs" company="Ace Poker Solutions">
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
using ReactiveUI;
using System;
using System.Reactive.Linq;
using System.Windows.Threading;

namespace PPPokerHandGrabber.Common.Wpf.Mvvm
{
    public abstract class WindowViewModelBase : ViewModelBase
    {      
        private ReactiveOperation currentOperation;

        public ReactiveOperation CurrentOperation
        {
            get
            {
                return currentOperation;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref currentOperation, value);
            }
        }

        private OperationViewModel operationInfo;

        public IOperationViewModel OperationInfo
        {
            get
            {
                if (operationInfo == null)
                {
                    operationInfo = new OperationViewModel();
                    ConfigureOperationViewModel(operationInfo);
                }
                return operationInfo;
            }
        }

        protected virtual void ConfigureOperationViewModel(OperationViewModel operationViewModel)
        {
            operationViewModel.SetOperationBinding(this, vm => vm.CurrentOperation);
        }

        protected ReactiveOperation StartAsyncOperation(Action action, Action callback)
        {
            var asyncOperation = new ReactiveOperation();

            CurrentOperation = asyncOperation;

            Observable
                .Start(action)
                .ObserveOnDispatcher(DispatcherPriority.ContextIdle)
                .Subscribe(x => callback(), asyncOperation.CompleteFailed, asyncOperation.CompleteSuccess);

            return asyncOperation;
        }

        protected ReactiveOperation StartAsyncOperation(Action action, Action<Exception> callback)
        {
            var asyncOperation = new ReactiveOperation();

            CurrentOperation = asyncOperation;

            Observable
                .Start(action)
                .ObserveOnDispatcher(DispatcherPriority.ContextIdle)
                .Subscribe(x => callback(null), e => { callback(e); asyncOperation.CompleteFailed(null); }, asyncOperation.CompleteSuccess);

            return asyncOperation;
        }

        protected ReactiveOperation StartAsyncOperation<T>(Func<T> func, Action<T> callback)
        {
            var asyncOperation = new ReactiveOperation();

            CurrentOperation = asyncOperation;

            Observable
                .Start(func)
                .ObserveOnDispatcher(DispatcherPriority.ContextIdle)
                .Subscribe(callback, asyncOperation.CompleteFailed, asyncOperation.CompleteSuccess);

            return asyncOperation;
        }
    }
}