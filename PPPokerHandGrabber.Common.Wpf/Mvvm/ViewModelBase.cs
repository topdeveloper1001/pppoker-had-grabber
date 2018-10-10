//-----------------------------------------------------------------------
// <copyright file="ViewModelBase.cs" company="Ace Poker Solutions">
// Copyright © 2015 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using PPPokerHandGrabber.Common.Reflection;
using ReactiveUI;
using System;
using System.Linq.Expressions;
using System.Reactive.Disposables;

namespace PPPokerHandGrabber.Common.Wpf.Mvvm
{
    /// <summary>
    /// Base view model class inherited from ReactiveUI (do not delete, probably this one will replace BaseViewModel)
    /// </summary>
    public abstract class ViewModelBase : ReactiveObject, IDisposable
    {
        protected virtual void RaisePropertyChanged<T>(Expression<Func<T>> property)
        {
            this.RaisePropertyChanged(ReflectionHelper.GetPropertyNameFromExpression(property));
        }

        #region IDisposable implementation

        private bool disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources here.
                    DisposeList.Dispose();
                    Disposing();
                }

                // Clear unmanaged resources here

                disposed = true;
            }
        }

        protected virtual void Disposing()
        {
        }

        ~ViewModelBase()
        {
            Dispose(false);
        }

        private CompositeDisposable disposeList = new CompositeDisposable();

        protected CompositeDisposable DisposeList
        {
            get { return disposeList; }
        }

        #endregion
    }
}