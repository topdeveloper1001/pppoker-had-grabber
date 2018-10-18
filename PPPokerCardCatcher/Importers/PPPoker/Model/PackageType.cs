﻿//-----------------------------------------------------------------------
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

namespace PPPokerCardCatcher.Importers.PPPoker.Model
{
    internal enum PackageType : short
    {
        Unknown,
        ActionBRC,
        ActionNotifyBRC,
        ActionREQ,
        AnteBRC,
        BlindStatusBRC,
        ChangeTableRSP,
        ChipsBackBRC,
        ClubListREQ,
        ClubListRSP,
        ClubRoomREQ,
        ClubRoomRSP,
        CroupierActionBRC,
        DailyRewardREQ,
        DailyRewardRSP,
        DailyTasksREQ,
        DailyTasksRSP,
        DealerInfoRSP,
        DiamondREQ,
        DiamondRSP,
        EnterRoomREQ,
        EnterRoomRSP,
        GameComingRSP,
        GetUserMarksREQ,
        GetUserMarksRSP,
        HandCardRSP,
        HeartBeatREQ,
        HeartBeatRSP,
        ItemListREQ,
        ItemListRSP,
        JoinLobbyCashREQ,
        JoinLobbyCashRSP,
        LeaveRoomREQ,
        LeaveRoomRSP,
        LobbyListREQ,
        LobbyListRSP,
        MoneyREQ,
        MoneyRSP,
        MttInfoRSP,
        MttJoinREQ,
        MttJoinRSP,
        MttListREQ,
        MttListRSP,
        MttScoreREQ,
        MttScoreRSP,
        MttStatusRSP,
        NewMailNumREQ,
        NewMailNumRSP,
        NoticeBRC,
        OtherEnterRoomBRC,
        OtherLeaveRoomBRC,
        PPCoinRSP,
        PreActionREQ,
        PreActionRSP,
        RebateREQ,
        RebateRSP,
        RebuyBRC,
        RebuyNotifyRSP,
        RoomRebateInfoBRC,
        RoomRebateInfoREQ,
        RoomRebateInfoRSP,
        RoundStartBRC,
        RoundOverBRC,
        SeatStatusBRC,
        SelUserInfoRSP,
        SelUserVipInfoREQ,
        SelUserVipInfoRSP,
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
        TextBRC,
        UserCroupierInUsingREQ,
        UserCroupierInUsingRSP,
        UserCroupierREQ,
        UserCroupierRSP,
        UserSngOverRSP,
        UserTotalDataREQ,
        UserTotalDataRSP,
        WinnerRSP,
    }
}