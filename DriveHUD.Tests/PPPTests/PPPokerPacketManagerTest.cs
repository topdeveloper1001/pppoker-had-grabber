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
    }
}
