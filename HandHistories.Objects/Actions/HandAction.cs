//-----------------------------------------------------------------------
// <copyright file="HandAction.cs" company="Ace Poker Solutions">
// Copyright © 2015 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using HandHistories.Objects.Cards;
using System;
using System.Xml.Serialization;

namespace HandHistories.Objects.Actions
{
    [Serializable]
    [XmlInclude(typeof(WinningsAction))]
    [XmlInclude(typeof(AllInAction))]
    public class HandAction
    {
        [XmlAttribute]
        public string PlayerName { get; set; }

        [XmlAttribute]
        public HandActionType HandActionType { get; set; }

        [XmlAttribute]
        public decimal Amount { get; set; }

        [XmlAttribute]
        public Street Street { get; set; }

        [XmlIgnore]
        public int ActionNumber { get; set; }

        [XmlAttribute]
        public bool IsAllIn { get; set; }

        [XmlIgnore]
        public bool IsCheck
        {
            get
            {
                return HandActionType == HandActionType.CHECK;
            }
        }

        [XmlIgnore]
        public bool IsFold
        {
            get
            {
                return HandActionType == HandActionType.FOLD;
            }
        }

        public HandAction()
        {
        }

        public HandAction(string playerName,
            HandActionType handActionType,
            decimal amount,
            Street street,
            int actionNumber)
            : this(playerName, handActionType, amount, street, false, actionNumber)
        {
        }

        public HandAction(string playerName,
                          HandActionType handActionType,
                          Street street,
                          bool AllInAction = false,
                          int actionNumber = 0)
        {
            Street = street;
            HandActionType = handActionType;
            PlayerName = playerName;
            Amount = 0m;
            ActionNumber = actionNumber;
            IsAllIn = AllInAction;
        }

        public HandAction(string playerName,
                          HandActionType handActionType,
                          decimal amount,
                          Street street,
                          bool AllInAction = false,
                          int actionNumber = 0)
        {
            Street = street;
            HandActionType = handActionType;
            PlayerName = playerName;
            Amount = GetAdjustedAmount(amount, handActionType);
            ActionNumber = actionNumber;
            IsAllIn = AllInAction;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            HandAction handAction = obj as HandAction;
            if (handAction == null) return false;

            return handAction.ToString().Equals(ToString());
        }

        public override string ToString()
        {
            string format = "{0} does {1} for {2} on street {3}{4}";

            return string.Format(format,
                PlayerName,
                HandActionType,
                Amount.ToString("N2"),
                Street,
                IsAllIn ? " and is All-In" : "");
        }

        public void DecreaseAmount(decimal value)
        {
            Amount = Math.Abs(Amount) - Math.Abs(value);
            Amount = GetAdjustedAmount(Amount, HandActionType);
        }

        /// <summary>
        /// Actions like calling, betting, raising should be negative amounts.
        /// Actions such as winning should be positive.
        /// Actions such as chatting should be 0 and can cause false positives if people say certain things.
        /// </summary>
        /// <param name="amount">The amount in the action.</param>
        /// <param name="type">The type of the action.</param>
        /// <returns></returns>
        public static decimal GetAdjustedAmount(decimal amount, HandActionType type)
        {
            if (amount == 0m)
            {
                return 0m;
            }

            amount = Math.Abs(amount);

            switch (type)
            {
                case HandActionType.WINS:
                    return amount;
                case HandActionType.WINS_SIDE_POT:
                    return amount;
                case HandActionType.TIES:
                    return amount;
                case HandActionType.ANTE:
                case HandActionType.ALL_IN:
                case HandActionType.BET:
                case HandActionType.BIG_BLIND:
                case HandActionType.CALL:
                case HandActionType.POSTS:
                case HandActionType.RAISE:
                case HandActionType.SMALL_BLIND:
                case HandActionType.STRADDLE:
                    return amount * -1;
                case HandActionType.UNCALLED_BET:
                    return amount;
                case HandActionType.WINS_THE_LOW:
                    return amount;
                case HandActionType.ADDS:
                    return 0.0M; // when someone adds to their stack it doesn't effect their winnings in the hand
                case HandActionType.CHAT:
                    return 0.0M; // overwrite any $ talk in the chat
                case HandActionType.JACKPOTCONTRIBUTION:
                    return 0.0M; // does not affect pot, as it goes to a jackpot
            }

            throw new ArgumentException("GetAdjustedAmount: Unknown action " + type + " to have amount " + amount);
        }       
     
        [XmlIgnore]
        public bool IsAllInAction
        {
            get { return HandActionType == HandActionType.ALL_IN; }
        }

        [XmlIgnore]
        public bool IsWinningsAction
        {
            get
            {
                return HandActionType == HandActionType.WINS ||
                       HandActionType == HandActionType.WINS_SIDE_POT ||
                       HandActionType == HandActionType.TIES ||
                       HandActionType == HandActionType.TIES_SIDE_POT ||
                       HandActionType == HandActionType.WINS_THE_LOW;
            }
        }

        [XmlIgnore]
        public bool IsAggressiveAction
        {
            get
            {
                return HandActionType == HandActionType.RAISE ||
                       HandActionType == HandActionType.BET ||
                       IsAllInAction;
            }
        }

        [XmlIgnore]
        public virtual bool IsBlinds
        {
            get
            {
                return HandActionType == HandActionType.SMALL_BLIND ||
                       HandActionType == HandActionType.BIG_BLIND ||
                       HandActionType == HandActionType.POSTS ||
                       HandActionType == HandActionType.STRADDLE ||
                       HandActionType == HandActionType.ANTE;
            }
        }

        [XmlIgnore]
        public bool IsGameAction
        {
            get
            {
                return HandActionType == HandActionType.SMALL_BLIND ||
                    HandActionType == HandActionType.BIG_BLIND ||
                    HandActionType == HandActionType.ANTE ||
                    HandActionType == HandActionType.POSTS ||
                    HandActionType == HandActionType.STRADDLE ||
                    HandActionType == HandActionType.BET ||
                    HandActionType == HandActionType.CHECK ||
                    HandActionType == HandActionType.FOLD ||
                    HandActionType == HandActionType.ALL_IN ||
                    HandActionType == HandActionType.CALL ||
                    HandActionType == HandActionType.RAISE;
            }
        }

        [XmlIgnore]
        public virtual bool IsCall
        {
            get
            {
                return HandActionType == HandActionType.CALL;
            }
        }

    }
}