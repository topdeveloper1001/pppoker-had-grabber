//-----------------------------------------------------------------------
// <copyright file="WeakDelegatesManager.cs" company="Ace Poker Solutions">
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
using System.Collections.Generic;
using System.Linq;

namespace PPPokerHandGrabber.Common.Wpf.Events
{
    /// <summary>
	/// Represents collection of <see cref="WeakDelegateReference"/>.
	/// Original code was given from Composite Application Library (WeakDelegatesManager).
	/// </summary>
    public class WeakDelegatesManager
    {
        private readonly List<WeakDelegateReference> listeners = new List<WeakDelegateReference>();

        public void AddListener(Delegate listener)
        {
            lock (listeners)
            {
                listeners.Add(new WeakDelegateReference(listener));
            }
        }

        public void RemoveListener(Delegate listener)
        {
            lock (listeners)
            {
                listeners.RemoveAll(reference =>
                {
                    //Remove the listener, and prune collected listeners
                    var target = reference.Target;
                    return listener.Equals(target) || target == null;
                });
            }
        }

        /// <summary>
        /// Raises delegates using late binding.
        /// </summary>
        /// <param name="args">Array of delegate parameters.</param>
        public void Raise(params object[] args)
        {
            IList<Delegate> dels = GetAliveDelegates();
            foreach (Delegate handler in dels)
                handler.DynamicInvoke(args);
        }

        /// <summary>
        /// Raises delegates within action.
        /// </summary>
        /// <param name="action">Action to wrap target delegate.</param>
        public void Raise(Action<Delegate> action)
        {
            IList<Delegate> dels = GetAliveDelegates();

            foreach (Delegate handler in dels)
            {
                action(handler);
            }
        }

        private IList<Delegate> GetAliveDelegates()
        {
            List<Delegate> result = null;

            lock (listeners)
            {
                listeners.RemoveAll(listener => listener.Target == null);
                result = listeners.Select(listener => listener.Target).Where(listener => listener != null).ToList();
            }

            return result;
        }
    }
}