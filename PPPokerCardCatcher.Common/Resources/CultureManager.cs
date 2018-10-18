//-----------------------------------------------------------------------
// <copyright file="CultureManager.cs" company="Ace Poker Solutions">
// Copyright © 2018 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading;

namespace PPPokerCardCatcher.Common.Resources
{
    public class CultureManager : INotifyPropertyChanged
    {
        private CultureManager()
        {
        }

        static CultureManager()
        {
        }

        private static readonly CultureManager instance = new CultureManager();

        public static CultureManager Instance
        {
            get
            {
                return instance;
            }
        }

        private CultureInfo uiCulture;

        private bool synchronizeThreadCulture = true;

        public event EventHandler UICultureChanged;

        public CultureInfo UICulture
        {
            get
            {
                return uiCulture ?? (uiCulture = Thread.CurrentThread.CurrentUICulture);
            }
            set
            {
                if (value == UICulture)
                {
                    return;
                }

                uiCulture = value;

                Thread.CurrentThread.CurrentUICulture = value;

                if (SynchronizeThreadCulture)
                {
                    SetThreadCulture(value);
                }

                if (UICultureChanged != null)
                {
                    UICultureChanged(null, EventArgs.Empty);
                }

                OnPropertyChanged("UICulture");
            }
        }

        public bool SynchronizeThreadCulture
        {
            get
            {
                return synchronizeThreadCulture;
            }
            set
            {
                if (synchronizeThreadCulture == value)
                {
                    return;
                }

                synchronizeThreadCulture = value;

                if (value)
                {
                    SetThreadCulture(UICulture);
                }

                OnPropertyChanged("SynchronizeThreadCulture");
            }
        }

        private void SetThreadCulture(CultureInfo value)
        {
            Thread.CurrentThread.CurrentCulture = value.IsNeutralCulture ? CultureInfo.CreateSpecificCulture(value.Name) : value;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}