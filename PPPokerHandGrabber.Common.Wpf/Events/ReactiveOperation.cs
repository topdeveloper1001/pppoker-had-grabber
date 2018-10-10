//-----------------------------------------------------------------------
// <copyright file="ReactiveOperation.cs" company="Ace Poker Solutions">
// Copyright © 2015 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using Prism.Mvvm;
using System;

namespace PPPokerHandGrabber.Common.Wpf.Events
{
    public class ReactiveOperation : BindableBase, IAsyncOperation
    {
        private readonly WeakDelegatesManager completedListners = new WeakDelegatesManager();

        public event EventHandler Completed
        {
            add { completedListners.AddListener(value); }
            remove { completedListners.RemoveListener(value); }
        }

        public bool IsCompleted { get; private set; }

        private void CheckCompleted()
        {
            if (IsCompleted)
            {
                throw new InvalidOperationException("DH: Operation was already completed");
            }
        }

        private void InternalComplete()
        {
            IsCompleted = true;
            OnCompleted();
            OnPropertyChanged(() => IsCompleted);
        }

        protected virtual void OnCompleted()
        {
            completedListners.Raise(
                d =>
                {
                    ((EventHandler)d)?.Invoke(this, EventArgs.Empty);
                });
        }

        public void CompleteSuccess()
        {
            CheckCompleted();
            InternalComplete();
        }

        public void CompleteFailed(Exception error)
        {
            CheckCompleted();
            InternalComplete();

            if (error != null)
            {
                throw error;
            }
        }
    }
}