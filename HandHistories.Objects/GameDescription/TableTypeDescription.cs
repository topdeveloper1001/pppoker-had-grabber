//-----------------------------------------------------------------------
// <copyright file="TableTypeDescription.cs" company="Ace Poker Solutions">
// Copyright © 2017 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using System;
using System.Runtime.Serialization;

namespace HandHistories.Objects.GameDescription
{
    [Flags]
    [DataContract]    
    public enum TableTypeDescription : uint
    {
        [EnumMember]
        Unknown = 0,
        [EnumMember]
        Regular = 0x1 << 0,
        [EnumMember]
        Anonymous = 0x1 << 1,
        [EnumMember]
        SuperSpeed = 0x1 << 2,
        [EnumMember]
        Deep = 0x1 << 3,
        [EnumMember]
        Ante = 0x1 << 4,
        [EnumMember]
        Cap = 0x1 << 5,
        [EnumMember]
        Speed = 0x1 << 6,
        [EnumMember]
        Jackpot = 0x1 << 7,
        [EnumMember]
        SevenDeuceGame = 0x1 << 8,
        [EnumMember]
        FiftyBigBlindsMin = 0x1 << 9,
        [EnumMember]
        Shallow = 0x1 << 10,
        [EnumMember]
        PushFold = 0x1 << 11,
        [EnumMember]
        FastFold = 0x1 << 12,
        [EnumMember]
        Strobe = 0x1 << 13,
        [EnumMember]
        Slow = 0x1 << 14,
        [EnumMember]
        ShortDeck = 0x1 << 15,
        [EnumMember]
        Any = 0x1u << 31,
        [EnumMember]
        All = 0xFFFFFFFF
    }
}