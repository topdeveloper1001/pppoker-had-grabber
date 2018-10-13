//-----------------------------------------------------------------------
// <copyright file="UpdateReleaseNoteViewModel.cs" company="Ace Poker Solutions">
// Copyright © 2015 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using PPPokerCardCatcher.Common.Wpf.Mvvm;
using ReactiveUI;

namespace PPPokerCardCatcher.ViewModels
{
    /// <summary>
    /// Represents the view model of release notes
    /// </summary>
    public class UpdateReleaseNoteViewModel : ViewModelBase
    {
        public UpdateReleaseNoteViewModel()
        {
        }

        public UpdateReleaseNoteViewModel(string version, string notes)
        {
            Version = version;
            Notes = notes;
        }

        private string version;

        public string Version
        {
            get
            {
                return version;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref version, value);
            }
        }

        private string notes;

        public string Notes
        {
            get
            {
                return notes;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref notes, value);
            }
        }
    }
}