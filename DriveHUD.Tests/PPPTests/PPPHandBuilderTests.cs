//-----------------------------------------------------------------------
// <copyright file="PPPHandBuilderTests.cs" company="Ace Poker Solutions">
// Copyright © 2018 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using DriveHUD.Common.Extensions;
using DriveHUD.Common.Log;
using DriveHUD.Importers;
using DriveHUD.Importers.AndroidBase;
using DriveHUD.Importers.PPPoker;
using DriveHUD.Importers.PPPoker.Model;
using HandHistories.Objects.Hand;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NUnit.Framework;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DriveHUD.Tests.PPPTests
{
    [TestFixture]
    class PPPHandBuilderTests : PacketManagerTest
    {
        private const string SourceJsonFile = "Source.json";
        private const string ExpectedResultFile = "Result.xml";
        private const int destinationPort = 31001;

        private const int identifier = 7777;

        private IDHLog testLogger;

        private Dictionary<PackageType, MethodInfo> BuildPackageMapping { get; } = new Dictionary<PackageType, MethodInfo>();

        public override void SetUp()
        {
            base.SetUp();

            testLogger = new TestLogger();
            LogProvider.SetCustomLogger(testLogger);

            PackageTypeEnumerator.ForEach((PackageType enumValue, Type packageObjectType) => {
                Func<PPPTestSourcePacket, PPPokerPackage> func = BuildPackage<HeartBeatREQ>;
                MethodInfo method = func.Method.GetGenericMethodDefinition();
                MethodInfo generic = method.MakeGenericMethod(packageObjectType);

                BuildPackageMapping[enumValue] = generic;
            });
        }

        protected override string TestDataFolder => "PPPTests\\TestData\\HandsRawData";

        [TestCase("plo-6max-hero")]
        //[TestCase("6-max-short-dbl-ante-no-hero")]
        //[TestCase("8-max-regular-hero")]
        //[TestCase("6-max-short-all-in")]
        public void TryBuildTest(string testFolder)
        {
            var packages = ReadPackages(testFolder);

            CollectionAssert.IsNotEmpty(packages, $"Packages collection must be not empty for {testFolder}");

            var handBuilder = new PPPHandBuilder();

            HandHistory actual = null;

            foreach (var package in packages)
            {
                if (handBuilder.TryBuild(package, out actual))
                {
                    break;
                }
            }

            Assert.IsNotNull(actual, $"Actual HandHistory must be not null for {testFolder}");

            var expected = ReadExpectedHandHistory(testFolder);

            Assert.IsNotNull(expected, $"Expected HandHistory must be not null for {testFolder}");

            AssertionUtils.AssertHandHistory(actual, expected);
        }

        //[TestCase("multiple-accounts-raw-1")]
        //[TestCase("multiple-accounts-raw-2")]
        //[TestCase("multiple-accounts-raw-3")]
        //public void MultipleTryBuildTest(string folder)
        //{
        //    var testFolder = Path.Combine(TestDataFolder, folder);

        //    DirectoryAssert.Exists(testFolder);

        //    var logFiles = Directory.GetFiles(testFolder, "*.txt.");

        //    var capturedPackets = new List<CapturedPacket>();

        //    foreach (var logFile in logFiles)
        //    {
        //        capturedPackets.AddRange(TcpImporterTestUtils.ReadCapturedPackets(logFile, null));
        //    }

        //    capturedPackets = capturedPackets.OrderBy(x => x.CreatedTimeStamp).ToList();

        //    var handHistories = new List<HandHistory>();

        //    var packetManager = new PPPokerPacketManager();
        //    var handBuilder = new PPPHandBuilder();
        //    var debugPPPImporter = new DebugPPPImporter();

        //    foreach (var capturedPacket in capturedPackets)
        //    {
        //        if (!packetManager.TryParse(capturedPacket, out IList<PPPokerPackage> packages))
        //        {
        //            continue;
        //        }

        //        foreach (var package in packages)
        //        {                    
        //            if (!PPPImporterStub.IsAllowedPackage(package))
        //            {
        //                continue;
        //            }

        //            package.Timestamp = capturedPacket.CreatedTimeStamp;

        //            debugPPPImporter.LogPackage(capturedPacket, package);

        //            if (package.PackageType == PackageType.RequestJoinRoom)
        //            {
        //                debugPPPImporter.ParsePackage<RequestJoinRoom>(package,
        //                  body => LogProvider.Log.Info($"User {package.UserId} entered room {body.RoomId}."),
        //                  () => LogProvider.Log.Info($"User {package.UserId} entered room."));

        //                continue;
        //            }

        //            var port = capturedPacket.Destination.Port != destinationPort ? capturedPacket.Destination.Port : capturedPacket.Source.Port;

        //            if (package.PackageType == PackageType.RequestLeaveRoom)
        //            {
        //                debugPPPImporter.ParsePackage<RequestLeaveRoom>(package,
        //                    body =>
        //                    {
        //                        LogProvider.Log.Info($"User {package.UserId} left room {body.RoomId}.");
        //                        handBuilder.CleanRoom(port, body.RoomId);
        //                    },
        //                    () => LogProvider.Log.Info($"User {package.UserId} left room {package.RoomId}."));

        //                continue;
        //            }

        //            if (handBuilder.TryBuild(package, port, out HandHistory handHistory))
        //            {
        //                handHistories.Add(handHistory);
        //                LogProvider.Log.Info($"Hand #{handHistory.HandId} has been sent. [{package.UserId}]");
        //            }
        //        }
        //    }

        //    WriteHandHistoriesToFile(handHistories);

        //    Assert.IsTrue(handHistories.Count > 0);
        //}

        //private void WriteHandHistoriesToFile(IEnumerable<HandHistory> handHistories)
        //{
        //    var groupedHandHistories = handHistories.GroupBy(x => x.GameDescription.Identifier).ToDictionary(x => x.Key, x => x.ToArray());

        //    foreach (var handHistoriesByIdentifier in groupedHandHistories)
        //    {
        //        var file = $"{handHistoriesByIdentifier.Key}.xml";

        //        var xml = SerializationHelper.SerializeObject(handHistoriesByIdentifier.Value);

        //        File.WriteAllText(file, xml);
        //    }
        //}

        private HandHistory ReadExpectedHandHistory(string folder)
        {
            var xmlFile = Path.Combine(TestDataFolder, folder, ExpectedResultFile);

            FileAssert.Exists(xmlFile);

            try
            {
                var handHistoryText = File.ReadAllText(xmlFile);
                return SerializationHelper.DeserializeObject<HandHistory>(handHistoryText);
            }
            catch (Exception e)
            {
                Assert.Fail($"{ExpectedResultFile} in {folder} has not been deserialized: {e}");
            }

            return null;
        }

        private IEnumerable<PPPokerPackage> ReadPackages(string folder)
        {
            var packages = new List<PPPokerPackage>();

            var jsonFile = Path.Combine(TestDataFolder, folder, SourceJsonFile);

            FileAssert.Exists(jsonFile);

            PPPTestSourceObject testSourceObject = null;

            try
            {
                var jsonFileContent = File.ReadAllText(jsonFile);
                testSourceObject = JsonConvert.DeserializeObject<PPPTestSourceObject>(jsonFileContent, new StringEnumConverter());
            }
            catch (Exception e)
            {
                Assert.Fail($"{ExpectedResultFile} in {folder} has not been deserialized: {e}");
            }

            Assert.IsNotNull(testSourceObject);

            foreach (var packet in testSourceObject.Packets)
            {
                PPPokerPackage package = null;

                if (BuildPackageMapping.ContainsKey(packet.PackageType))
                {
                    package = (PPPokerPackage)BuildPackageMapping[packet.PackageType].Invoke(this, new object[] { packet });
                }

                Assert.IsNotNull(package);

                packages.Add(package);
            }

            return packages;
        }

        private PPPokerPackage BuildPackage<T>(PPPTestSourcePacket packet)
        {
            var contentObject = JsonConvert.DeserializeObject<T>(packet.Content.ToString(), new StringEnumConverter());

            Assert.IsNotNull(contentObject, $"Content object of {typeof(T)} must be not null.");

            using (var ms = new MemoryStream())
            {
                Serializer.Serialize(ms, contentObject);
                var bytes = ms.ToArray();

                var package = new PPPokerPackage
                {
                    PackageType = packet.PackageType,
                    Body = bytes,
                    Timestamp = packet.Time
                };

                return package;
            }
        }

        private class PPPTestSourceObject
        {
            public IEnumerable<PPPTestSourcePacket> Packets { get; set; }
        }

        private class PPPTestSourcePacket
        {
            public PackageType PackageType { get; set; }

            public PackageDirection Direction { get; set; }

            public int ClientPort { get; set; }

            public DateTime Time { get; set; }

            public object Content { get; set; }
        }

        private class PPPImporterStub : PPPImporter
        {
            public new static bool IsAllowedPackage(PPPokerPackage package)
            {
                return PPPImporter.IsAllowedPackage(package);
            }
        }

        private class TestLogger : IDHLog
        {
            public void Log(Type senderType, object message, LogMessageType logMessageType)
            {
            }

            public void Log(Type senderType, object message, Exception exception, LogMessageType logMessageType)
            {
            }

            public void Log(string loggerName, object message, LogMessageType logMessageType)
            {
                Console.WriteLine(message);
            }

            public void Log(string loggerName, object message, Exception exception, LogMessageType logMessageType)
            {
                Console.WriteLine(message);
            }
        }

        private class DebugPPPImporter : PPPImporter
        {
            private Dictionary<int, DebugPPPLogger> loggers = new Dictionary<int, DebugPPPLogger>();

            public DebugPPPImporter()
            {
            }

            public void LogPackage(CapturedPacket capturedPacket, PPPokerPackage package)
            {
                var port = capturedPacket.Destination.Port != destinationPort ? capturedPacket.Destination.Port : capturedPacket.Source.Port;

                if (!loggers.TryGetValue(port, out DebugPPPLogger logger))
                {
                    logger = new DebugPPPLogger(port);
                    loggers.Add(port, logger);
                }

                //protectedLogger = logger;

                base.LogPackage(package);
            }

            public new void ParsePackage<T>(PPPokerPackage package, Action<T> onSuccess, Action onFail)
            {
                base.ParsePackage(package, onSuccess, onFail);
            }

            private class DebugPPPLogger // : IPokerClientEncryptedLogger
            {
                private const string logFilePattern = "Json-raw-{0}.json";

                private string logFile;

                public DebugPPPLogger(int port)
                {
                    logFile = string.Format(logFilePattern, port);

                    if (File.Exists(logFile))
                    {
                        File.Delete(logFile);
                    }
                }

                public void CleanLogs()
                {
                }

                public void Initialize() //(PokerClientLoggerConfiguration configuration)
                {
                }

                public void Log(string message)
                {
                    File.AppendAllText(logFile, message);
                }

                public void StartLogging()
                {
                }

                public void StopLogging()
                {
                }
            }
        }
    }
}