//-----------------------------------------------------------------------
// <copyright file="CaptureDevice.cs" company="Ace Poker Solutions">
// Copyright © 2018 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using SharpPcap;
using System;

namespace PPPokerCardCatcher.Importers.TcpBased
{
    internal class CaptureDevice
    {
        private readonly ICaptureDevice captureDevice;

        public CaptureDevice(ICaptureDevice captureDevice)
        {
            this.captureDevice = captureDevice ?? throw new ArgumentNullException(nameof(captureDevice));
        }

        public ICaptureDevice Device
        {
            get
            {
                return captureDevice;
            }
        }

        public bool IsOpened
        {
            get;
            set;
        }
    }
}