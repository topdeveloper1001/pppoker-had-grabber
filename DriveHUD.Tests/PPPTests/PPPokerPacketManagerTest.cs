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
    }
}
