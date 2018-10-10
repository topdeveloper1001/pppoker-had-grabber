//-----------------------------------------------------------------------
// <copyright file="WeakDelegateReference.cs" company="Ace Poker Solutions">
// Copyright © 2015 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using System;
using System.Reflection;

namespace PPPokerHandGrabber.Common.Wpf.Events
{
    /// <summary>
    /// Represents a reference to a <see cref="Delegate"/>.
    /// Original code was given from Composite Application Library (DelegateReference).
    /// This class is tested to be Silverlight compatible.
    /// </summary>
    public interface IWeakDelegateReference
    {
        /// <summary>
        /// Gets the referenced <see cref="Delegate" /> object.
        /// </summary>
        /// <value>A <see cref="Delegate"/> instance if the target is valid; otherwise <see langword="null"/>.</value>
        Delegate Target { get; }
    }

    /// <summary>
    /// Represents a reference to a <see cref="Delegate"/> that contain a
    /// <see cref="WeakReference"/> to the target.
    /// </summary>
    public class WeakDelegateReference : IWeakDelegateReference
    {
        private readonly WeakReference _weakReference;
        private readonly MethodInfo _method;
        private readonly Type _delegateType;

        /// <summary>
        /// Initializes a new instance of <see cref="DelegateReference"/>.
        /// </summary>
        /// <param name="delegate">The original <see cref="Delegate"/> to create a reference for.</param>
        /// <exception cref="ArgumentNullException">If the passed <paramref name="delegate"/> is not assignable to <see cref="Delegate"/>.</exception>
        public WeakDelegateReference(Delegate @delegate)
        {
            if (@delegate == null)
            {
                throw new ArgumentNullException("delegate");
            }

            _weakReference = new WeakReference(@delegate.Target);
            _method = @delegate.Method;
            _delegateType = @delegate.GetType();
        }

        /// <summary>
        /// Gets the <see cref="Delegate" /> (the target) referenced by the current <see cref="DelegateReference"/> object.
        /// </summary>
        /// <value><see langword="null"/> if the object referenced by the current <see cref="DelegateReference"/> object has been garbage collected; otherwise, a reference to the <see cref="Delegate"/> referenced by the current <see cref="DelegateReference"/> object.</value>
        public Delegate Target
        {
            get { return TryGetDelegate(); }
        }

        private Delegate TryGetDelegate()
        {
            if (_method.IsStatic)
            {
                return Delegate.CreateDelegate(_delegateType, null, _method);
            }

            object target = _weakReference.Target;

            if (target != null)
            {
                return Delegate.CreateDelegate(_delegateType, target, _method);
            }

            return null;
        }
    }
}