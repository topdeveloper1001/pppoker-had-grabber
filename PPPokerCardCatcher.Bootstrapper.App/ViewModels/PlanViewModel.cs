//-----------------------------------------------------------------------
// <copyright file="PlanViewModel.cs" company="Ace Poker Solutions">
// Copyright © 2015 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using PPPokerCardCatcher.Bootstrapper.App.Common;

namespace PPPokerCardCatcher.Bootstrapper.App.ViewModels
{
    public class PlanViewModel : PageViewModel
    {
        public PlanViewModel(MainWindowViewModel mainViewModel) : base(mainViewModel)
        {
        }

        public override PageType PageType
        {
            get
            {
                return PageType.PlanPage;
            }
        }
    }
}