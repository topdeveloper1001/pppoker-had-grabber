//-----------------------------------------------------------------------
// <copyright file="SubPacket.cs" company="Ace Poker Solutions">
// Copyright © 2018 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;

namespace PPPokerHandGrabber.Importers.TcpBased
{
    internal class SubPacket<T> where T : class
    {
        private SubPacket()
        {
        }

        public SubPacket(byte[] bytes, int expectedLength, DateTime createdDate, uint sequenceNumber, bool isStarting = false)
        {
            Bytes = new List<byte>(bytes);
            DateReceived = DateTime.Now;
            ExpectedLength = expectedLength;
            CreatedDate = createdDate;
            SequenceNumber = sequenceNumber;
            IsStarting = isStarting;
        }

        public List<byte> Bytes
        {
            get;
            set;
        }

        public DateTime DateReceived
        {
            get;
            set;
        }

        public DateTime CreatedDate
        {
            get;
            set;
        }

        public int ExpectedLength
        {
            get;
            set;
        }

        public uint SequenceNumber
        {
            get;
            set;
        }

        public int CurrentLength
        {
            get
            {
                return Bytes.Count;
            }
        }

        public bool IsStarting
        {
            get;
            set;
        }

        public bool IsCompleted
        {
            get
            {
                return ExpectedLength != 0 && ((ExpectedLength == CurrentLength) || (IsStarting && ExpectedLength > 0 && ExpectedLength < CurrentLength));
            }
        }

        public void Add(byte[] bytes)
        {
            if (CurrentLength + bytes.Length > ExpectedLength)
            {
                throw new ArgumentOutOfRangeException(nameof(bytes), $"Length of bytes to add is out of range: {bytes.Length}, {CurrentLength}, {ExpectedLength}");
            }

            Bytes.AddRange(bytes);
        }

        public bool CanAddSubPacket(byte[] bytes, uint partialPacketSequenceNumber)
        {
            if (partialPacketSequenceNumber < SequenceNumber &&
                (partialPacketSequenceNumber - SequenceNumber) != CurrentLength)
            {
                return false;
            }

            return bytes.Length + CurrentLength <= ExpectedLength;
        }

        public bool CanCompleteBySubPacket(byte[] partialBytes, uint partialPacketSequenceNumber)
        {
            if (partialPacketSequenceNumber < SequenceNumber &&
                (partialPacketSequenceNumber - SequenceNumber) != CurrentLength)
            {
                return false;
            }

            return partialBytes.Length + CurrentLength == ExpectedLength;
        }

        public bool IsExpired(int expirationPeriod)
        {
            return (DateTime.Now - DateReceived).TotalMilliseconds > expirationPeriod;
        }

        public bool TryParse(int startingPosition, out T package)
        {
            package = null;

            if (ExpectedLength != CurrentLength)
            {
                return false;
            }

            var packageBuilder = ServiceLocator.Current.GetInstance<IPackageBuilder<T>>();

            return packageBuilder.TryParse(Bytes.ToArray(), startingPosition, out package);
        }

        public SubPacket<T> Clone()
        {
            var clone = new SubPacket<T>()
            {
                Bytes = new List<byte>(Bytes),
                DateReceived = DateReceived,
                ExpectedLength = ExpectedLength,
                CreatedDate = CreatedDate,
                SequenceNumber = SequenceNumber,
                IsStarting = IsStarting,
            };

            return clone;
        }
    }
}