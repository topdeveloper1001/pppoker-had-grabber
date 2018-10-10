//-----------------------------------------------------------------------
// <copyright file="ProcessExtensions.cs" company="Ace Poker Solutions">
// Copyright © 2018 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using PPPokerHandGrabber.Common.WinApi;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Management;

namespace PPPokerHandGrabber.Common.Extensions
{
    public static class ProcessExtensions
    {
        /// <summary>
        /// Defines an extension method for type System.Process that returns the command line via WMI.
        /// </summary>
        /// <param name="process">Process to get command line</param>
        /// <returns>Command line</returns>     
        /// <exception cref="NotSupportedException">
        ///  You are trying to get command line for
        ///  a process that is running on a remote computer. This property is available only
        ///  for processes that are running on the local computer.
        /// </exception>
        /// <exception cref="System.ComponentModel.Win32Exception">A 32-bit process is trying to access the modules of a 64-bit process.</exception>
        /// <exception cref="PlatformNotSupportedException">
        /// The platform is Windows 98 or Windows Millennium Edition (Windows Me); set System.Diagnostics.ProcessStartInfo.UseShellExecute
        /// to false to access this property on Windows 98 and Windows Me.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The process System.Diagnostics.Process.Id is not available. -or- 
        /// The process has exited
        /// </exception>     
        public static string GetCommandLine(this Process process)
        {
            string cmdLine = null;

            using (var searcher = new ManagementObjectSearcher($"SELECT CommandLine FROM Win32_Process WHERE ProcessId = {process.Id}"))
            {
                // By definition, the query returns at most 1 match, because the process 
                // is looked up by ID (which is unique by definition).
                var matchEnum = searcher.Get().GetEnumerator();

                if (matchEnum.MoveNext()) // Move to the 1st item.
                {
                    cmdLine = matchEnum.Current["CommandLine"]?.ToString();
                }
            }

            if (cmdLine == null)
            {
                // Not having found a command line implies 1 of 2 exceptions, which the
                // WMI query masked:
                // An "Access denied" exception due to lack of privileges.
                // A "Cannot process request because the process (<pid>) has exited."
                // exception due to the process having terminated.
                // We provoke the same exception again simply by accessing process.MainModule.
                var dummy = process.MainModule; // Provoke exception.
            }

            return cmdLine;
        }

        /// <summary>
        /// Gets the parent process of the current process.
        /// </summary>
        /// <returns>An instance of the Process class.</returns>
        public static Process GetParentProcess(this Process process)
        {
            if (process == null)
            {
                throw new ArgumentNullException(nameof(process));
            }

            return GetParentProcess(process.Handle);
        }

        /// <summary>
        /// Gets the parent process of a specified process.
        /// </summary>
        /// <param name="handle">The process handle.</param>
        /// <returns>An instance of the Process class.</returns>
        public static Process GetParentProcess(IntPtr handle)
        {
            var pbi = new ProcessBasicInformation();

            var status = WinApi.WinApi.NtQueryInformationProcess(handle, 0, ref pbi, pbi.Size, out int returnLength);

            if (status != 0)
            {                
                throw new Win32Exception(status);
            }

            try
            {
                return Process.GetProcessById(pbi.InheritedFromUniqueProcessId.ToInt32());
            }
            catch (ArgumentException)
            {
                // not found
                return null;
            }
        }
    }
}