//-----------------------------------------------------------------------
// <copyright file="UpdaterOperation.cs" company="Ace Poker Solutions">
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

namespace PPPokerHandGrabber.Updater.Core
{
    public enum OperationStatus
    {
        None,
        Getting,
        Downloading,
        Computing,
        Deleting,
        Unzipping,
        Copying
    }

    public class Operation
    {
        public Operation(Action<OperationStatus> onOperationChanged)
        {
            OnOperationChanged = onOperationChanged;
        }

        public OperationStatus OperationStatus
        {
            get;
            set;
        }

        public Action<OperationStatus> OnOperationChanged { get; set; }
    }

    internal class OperationScope : IDisposable
    {
        private readonly Operation operation;

        public OperationScope(Operation operation, OperationStatus status)
        {
            this.operation = operation;
            this.operation.OperationStatus = status;

            this.operation.OnOperationChanged?.Invoke(status);
        }

        public void Dispose()
        {
            operation.OperationStatus = OperationStatus.None;
        }
    }
}