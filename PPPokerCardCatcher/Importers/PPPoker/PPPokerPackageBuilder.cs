//-----------------------------------------------------------------------
// <copyright file="PPPokerPackageBuilder.cs" company="Ace Poker Solutions">
// Copyright © 2018 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using PPPokerCardCatcher.Common.Log;
using PPPokerCardCatcher.Importers.PPPoker.Model;
using PPPokerCardCatcher.Importers.TcpBased;
using System;
using System.Linq;
using System.Text;

namespace PPPokerCardCatcher.Importers.PPPoker
{
    internal class PPPokerPackageBuilder : IPackageBuilder<PPPokerPackage>
    {
        public bool TryParse(byte[] bytes, int startingPosition, out PPPokerPackage package)
        {
            try
            {
                const int PackageTypeHeaderLength = 2;
                const string PackageTypePrefix = "pb.";

                int skip = startingPosition;

                var packageTypeLengthBytes = bytes.Skip(skip).Take(PackageTypeHeaderLength).ToArray();

                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(packageTypeLengthBytes);
                }

                var packageTypeLength = BitConverter.ToUInt16(packageTypeLengthBytes, 0);
                skip += PackageTypeHeaderLength;

                var packageTypeText = Encoding.ASCII.GetString(
                    bytes.Skip(skip + PackageTypePrefix.Length)
                        .Take(packageTypeLength - PackageTypePrefix.Length)
                        .ToArray()
                );

                skip += packageTypeLength;

                if (!Enum.TryParse(packageTypeText, out PackageType packageType))
                {
                    var dump = BitConverter.ToString(bytes.Skip(skip).ToArray()).Replace("-", " ");
                    LogProvider.Log.Warn($"Unknown package type {packageTypeText}: {dump}");
                    packageType = PackageType.Unknown;
                }

                package = new PPPokerPackage
                {
                    PackageType = packageType,
                    Body = bytes.Skip(skip).ToArray(),
                    Timestamp = DateTime.Now
                };

                return true;
            }
            catch (Exception e)
            {
                LogProvider.Log.Error(this, $"Failed to parse package", e);
            }

            package = null;

            return false;
        }
    }
}