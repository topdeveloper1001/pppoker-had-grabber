//-----------------------------------------------------------------------
// <copyright file="EnumPokerSites.cs" company="Ace Poker Solutions">
// Copyright © 2018 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using System.Runtime.Serialization;

namespace PPPokerHandGrabber.Entities
{
    [DataContract(Namespace = "")]
    public enum EnumPokerSites : short
    {
        [EnumMember]
        Unknown = 0,
        [EnumMember]
        Ignition = 1,
        [EnumMember]
        IPoker = 2,
        [EnumMember]
        Bovada = 3,
        [EnumMember]
        Bodog = 4,
        [EnumMember]
        BetOnline = 5,
        [EnumMember]
        SportsBetting = 6,
        [EnumMember]
        PokerStars = 7,
        [EnumMember]
        Poker888 = 8,
        [EnumMember]
        TigerGaming = 9,
        [EnumMember]
        AmericasCardroom = 10,
        [EnumMember]
        BlackChipPoker = 11,
        [EnumMember]
        WinningPokerNetwork = 12,
        [EnumMember]
        TruePoker = 13,
        [EnumMember]
        YaPoker = 14,
        [EnumMember]
        PartyPoker = 15,
        [EnumMember]
        GGN = 16,
        [EnumMember]
        PokerMaster = 17,
        [EnumMember]
        Horizon = 18,
        [EnumMember]
        Winamax = 19,
        [EnumMember]
        PokerKing = 20,
        [EnumMember]
        Adda52 = 21,
        [EnumMember]
        SpartanPoker = 22,
        [EnumMember]
        PPPoker = 23
    }
}