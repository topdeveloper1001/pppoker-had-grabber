//-----------------------------------------------------------------------
// <copyright file="PackageType.cs" company="Ace Poker Solutions">
// Copyright © 2018 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

namespace DriveHUD.Importers.PPPoker.Model
{
    internal enum PackageType : short
    {
        Unknown,
        
        ActionBRC,
        ActionNotifyBRC,
        ActionREQ,
        AnteBRC,
        BlindStatusBRC,
        ChipsBackBRC,
        ClubRoomREQ,
        ClubRoomRSP,
        DealerInfoRSP,
        EnterRoomREQ,
        EnterRoomRSP,
        GameComingRSP,
        GetUserMarksREQ,
        GetUserMarksRSP,
        HandCardRSP,
        HeartBeatREQ,
        HeartBeatRSP,
        LeaveRoomRSP,
        MoneyRSP,
        MttJoinREQ,
        MttJoinRSP,
        MttStatusRSP,
        NoticeBRC,
        OtherEnterRoomBRC,
        OtherLeaveRoomBRC,
        PPCoinRSP,
        PreActionREQ,
        PreActionRSP,
        RebuyBRC,
        RebuyNotifyRSP,
        RoundStartBRC,
        RoundOverBRC,
        SeatStatusBRC,
        SelUserInfoRSP,
        ShowHandRSP,
        ShowMyCardBRC,
        SitDownBRC,
        SitDownRSP,
        SngJoinREQ,
        SngJoinRSP,
        SngOverBRC,
        StandUpBRC,
        StandUpRSP,
        TableGameOverRSP,
        UserSngOverRSP,
        WinnerRSP,
    }
}
