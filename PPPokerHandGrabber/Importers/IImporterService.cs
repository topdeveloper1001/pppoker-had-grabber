//-----------------------------------------------------------------------
// <copyright file="IImporterService.cs" company="Ace Poker Solutions">
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

namespace PPPokerHandGrabber.Importers
{
    /// <summary>
    /// Importer service is service responsible for importing
    /// </summary>
    internal interface IImporterService
    {
        /// <summary>
        /// Importing has been stopped
        /// </summary>
        event EventHandler ImportingStopped;

        /// <summary>
        /// Service status
        /// </summary>
        bool IsStarted { get; }

        /// <summary>
        /// Start import
        /// </summary>
        void StartImport();

        /// <summary>
        /// Stop import
        /// </summary>
        void StopImport();

        /// <summary>
        /// Gets importer
        /// </summary>
        /// <typeparam name="T">Importer interface</typeparam>
        /// <returns>Registered importer or null</returns>
        T GetImporter<T>();

        /// <summary>
        /// Gets running importer
        /// </summary>
        /// <typeparam name="T">Importer interface</typeparam>
        /// <returns>Registered importer or null</returns>
        T GetRunningImporter<T>();

        /// <summary>
        /// Register importer
        /// </summary>
        /// <typeparam name="T">Importer interface</typeparam>
        IImporterService Register<T>() where T : IBackgroundProcess;
    }
}