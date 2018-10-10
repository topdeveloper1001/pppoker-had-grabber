using DriveHUD.Tests.TcpImportersTests;
using DriveHUD.Common.Extensions;
using DriveHUD.Importers.AndroidBase;
using DriveHUD.Importers.PPPoker;
using DriveHUD.Importers.PPPoker.Model;
//using Microsoft.QualityTools.Testing.Fakes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NUnit.Framework;
using System;
using System.Collections.Generic;
//using System.Fakes;
using System.IO;
using System.Net;
using Microsoft.QualityTools.Testing.Fakes;
using System.Fakes;

namespace DriveHUD.Tests.PPPTests
{
    [TestFixture]
    class PPPokerPacketManagerTest : PacketManagerTest
    {
        protected override string TestDataFolder => "PPPTests\\TestData";

        [TestCase(@"Packets\SngJoinREQ.txt", true)]
        public void PacketIsStartingPacketTest(string file, bool expected)
        {
            var packetManager = new PPPokerPacketManager();

            var bytes = ReadPacketFile(file);
            var actual = packetManager.IsStartingPacket(bytes);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase(@"Packets\SngJoinREQ.txt", 1)]
        [TestCase(@"Packets\MultiplePackets.txt", 2)]
        public void TryParsePacketTest(string file, int expected)
        {
            var bytes = ReadPacketFile(file);
            var packetManager = new PPPokerPacketManager();

            var capturedPacket = new CapturedPacket
            {
                Bytes = bytes,
                CreatedTimeStamp = DateTime.Parse("08/02/2018 12:28:28"),
                Destination = new IPEndPoint(IPAddress.Parse("192.168.0.105"), 27633),
                Source = new IPEndPoint(IPAddress.Parse("47.52.92.161"), 4000),
                SequenceNumber = 1962805251
            };

            var result = packetManager.TryParse(capturedPacket, out IList<PPPokerPackage> actualPackages);

            Assert.IsTrue(result);
            Assert.IsNotNull(actualPackages);
            Assert.That(actualPackages.Count, Is.EqualTo(expected));
        }

        [TestCase(@"Packets\EnterRoomRSP.txt", @"Packets\EnterRoomRSP.json")]
        [TestCase(@"Packets\SitDownBRC.txt", @"Packets\SitDownBRC.json")]
        [TestCase(@"Packets\DealerInfoRSP.txt", @"Packets\DealerInfoRSP.json")]
        [TestCase(@"Packets\ActionBRC.txt", @"Packets\ActionBRC.json")]
        [TestCase(@"Packets\WinnerRSP.txt", @"Packets\WinnerRSP.json")]
        public void DeserializationTest(string file, string jsonFile)
        {
            var bytes = ReadPacketFile(file);
            var packetManager = new PPPokerPacketManager();

            var capturedPacket = new CapturedPacket
            {
                Bytes = bytes,
                CreatedTimeStamp = DateTime.Parse("08/02/2018 12:28:28"),
                Destination = new IPEndPoint(IPAddress.Parse("192.168.0.105"), 27633),
                Source = new IPEndPoint(IPAddress.Parse("47.52.92.161"), 4000),
                SequenceNumber = 1962805251
            };

            var result = packetManager.TryParse(capturedPacket, out IList<PPPokerPackage> packages);

            foreach (var package in packages)
            {
                object actual = null;

                switch (package.PackageType)
                {
                    case PackageType.EnterRoomRSP:
                        actual = SerializationHelper.Deserialize<EnterRoomRSP>(package.Body);
                        break;
                    case PackageType.SitDownBRC:
                        actual = SerializationHelper.Deserialize<SitDownBRC>(package.Body);
                        break;
                    case PackageType.DealerInfoRSP:
                        actual = SerializationHelper.Deserialize<DealerInfoRSP>(package.Body);
                        break;
                    case PackageType.ActionBRC:
                        actual = SerializationHelper.Deserialize<ActionBRC>(package.Body);
                        break;
                    case PackageType.WinnerRSP:
                        actual = SerializationHelper.Deserialize<WinnerRSP>(package.Body);
                        break;
                }

                var jsonExpected = File.ReadAllText(Path.Combine(TestDataFolder, jsonFile));
                var jsonActual = JsonConvert.SerializeObject(actual, Formatting.Indented, new StringEnumConverter());

                Assert.That(jsonActual, Is.EqualTo(jsonExpected));
            }
        }

        [TestCase(@"Packets\192.168.88.15.60473-209.200.155.113.4000.txt", @"Packets\192.168.88.15.60473-209.200.155.113.4000-pkgt.txt")]
        [TestCase(@"Packets\209.200.155.113.4000-192.168.88.15.60473.txt", @"Packets\209.200.155.113.4000-192.168.88.15.60473-pkgt.txt")]
        public void TryParseTest(string file, string expectedPackageTypesFile)
        {
            var packets = ReadCapturedPackets(file, null);

            var expectedPackageTypes = !string.IsNullOrEmpty(expectedPackageTypesFile) ?
                    GetPackageTypeList<PackageType>(expectedPackageTypesFile) :
                    new List<PackageType>();

            var packetManager = new PPPokerPacketManager();

            var expectedCommandsIndex = 0;

            using (ShimsContext.Create())
            {
                foreach (var packet in packets)
                {
                    ShimDateTime.NowGet = () => packet.CreatedTimeStamp;

                    if (packetManager.TryParse(packet, out IList<PPPokerPackage> packages))
                    {
                        foreach (var package in packages)
                        {
                            if (expectedPackageTypes.Count > 0)
                            {
                                Assert.That(package.PackageType, Is.EqualTo(expectedPackageTypes[expectedCommandsIndex++]));
                                AssertPackage(package, packet);
                            }
                        }
                    }
                }
            }
        }

        private void AssertPackage(PPPokerPackage package, CapturedPacket capturedPacket)
        {
            switch (package.PackageType)
            {
                case PackageType.GetUserMarksREQ:
                    AssertPackage<GetUserMarksREQ>(package, capturedPacket);
                    break;
                case PackageType.GetUserMarksRSP:
                    AssertPackage<GetUserMarksRSP>(package, capturedPacket);
                    break;
                case PackageType.SelUserInfoRSP:
                    AssertPackage<SelUserInfoRSP>(package, capturedPacket);
                    break;
                case PackageType.EnterRoomRSP:
                    AssertPackage<EnterRoomRSP>(package, capturedPacket);
                    break;
                case PackageType.SitDownRSP:
                    AssertPackage<SitDownRSP>(package, capturedPacket);
                    break;
                case PackageType.SitDownBRC:
                    AssertPackage<SitDownBRC>(package, capturedPacket);
                    break;
                case PackageType.StandUpBRC:
                    AssertPackage<StandUpBRC>(package, capturedPacket);
                    break;
                case PackageType.BlindStatusBRC:
                    AssertPackage<BlindStatusBRC>(package, capturedPacket);
                    break;
                case PackageType.DealerInfoRSP:
                    AssertPackage<DealerInfoRSP>(package, capturedPacket);
                    break;
                case PackageType.RoundStartBRC:
                    AssertPackage<RoundStartBRC>(package, capturedPacket);
                    break;
                case PackageType.RoundOverBRC:
                    AssertPackage<RoundOverBRC>(package, capturedPacket);
                    break;
                case PackageType.ActionBRC:
                    AssertPackage<ActionBRC>(package, capturedPacket);
                    break;
                case PackageType.HandCardRSP:
                    AssertPackage<HandCardRSP>(package, capturedPacket);
                    break;
                case PackageType.ShowHandRSP:
                    AssertPackage<ShowHandRSP>(package, capturedPacket);
                    break;
                case PackageType.WinnerRSP:
                    AssertPackage<WinnerRSP>(package, capturedPacket);
                    break;
                case PackageType.ShowMyCardBRC:
                    AssertPackage<ShowMyCardBRC>(package, capturedPacket);
                    break;
                case PackageType.UserSngOverRSP:
                    AssertPackage<UserSngOverRSP>(package, capturedPacket);
                    break;
                case PackageType.TableGameOverRSP:
                    AssertPackage<TableGameOverRSP>(package, capturedPacket);
                    break;
            }
        }

        private void AssertPackage<T>(PPPokerPackage package, CapturedPacket capturedPacket)
        {
            Assert.IsTrue(
                SerializationHelper.TryDeserialize(package.Body, out T packageContent),
                $"Failed to deserialize {typeof(T)} package [ticks={capturedPacket.CreatedTimeStamp.Ticks}, userid={package.ClientPort}]"
            );
        }
    }
}
