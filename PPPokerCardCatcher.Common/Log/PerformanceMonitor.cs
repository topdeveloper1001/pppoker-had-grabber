//-----------------------------------------------------------------------
// <copyright file="PerformanceMonitor.cs" company="Ace Poker Solutions">
// Copyright © 2018 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using System;
using System.Diagnostics;

namespace PPPokerCardCatcher.Common.Log
{
    public class PerformanceMonitor : IDisposable
    {
        private readonly Stopwatch stopwatch = new Stopwatch();

        private readonly string message;

        private readonly long initialMemory;

        private readonly bool isEnabled;

        private readonly object logger;

        private readonly bool checkMemory;

        public PerformanceMonitor(string message) : this(message, true, null, true)
        {
        }

        public PerformanceMonitor(string message, bool isEnabled, object logger, bool checkMemory = false)
        {
            this.isEnabled = isEnabled;
            this.logger = logger;
            this.checkMemory = checkMemory;

            if (!isEnabled)
            {
                return;
            }

            if (checkMemory)
            {
                initialMemory = GC.GetTotalMemory(false);
            }

            this.message = message;
            stopwatch.Start();
        }

        public void Dispose()
        {
            if (!isEnabled)
            {
                return;
            }

            if (stopwatch.IsRunning)
            {
                stopwatch.Stop();
            }

            var durationMessage = $"{message} (Duration): {stopwatch.ElapsedMilliseconds}ms";
#if DEBUG
            Console.WriteLine(durationMessage);
#endif
            LogProvider.Log.Info(logger, durationMessage);

            if (!checkMemory)
            {
                return;
            }

            var memoryMessage = $"{message} (Memory): {initialMemory}/{GC.GetTotalMemory(false)}";
#if DEBUG
            Console.WriteLine(memoryMessage);
#endif
            LogProvider.Log.Info(logger, memoryMessage);
        }
    }
}