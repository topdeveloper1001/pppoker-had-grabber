//-----------------------------------------------------------------------
// <copyright file="PacketManagerTest.cs" company="Ace Poker Solutions">
// Copyright © 2018 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

//using DriveHUD.Importers.Adda52;
using DriveHUD.Importers.AndroidBase;
//using DriveHUD.Importers.PokerKing;
//using DriveHUD.Importers.PokerKing.Model;
//using DriveHUD.Importers.PokerMaster;
//using DriveHUD.Importers.PokerMaster.Model;
using DriveHUD.Importers.PPPoker;
using DriveHUD.Importers.PPPoker.Model;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
//using NSubstitute;
using NUnit.Framework;
using PMCatcher.Tests;
//using Prism.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DriveHUD.Tests.TcpImportersTests
{
    abstract class PacketManagerTest
    {
        protected abstract string TestDataFolder { get; }

        [OneTimeSetUp]
        public virtual void SetUp()
        {
            Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;

            var unityContainer = new UnityContainer();

            //unityContainer.RegisterType<IPackageBuilder<PokerKingPackage>, PokerKingPackageBuilder>();
            //unityContainer.RegisterType<IPackageBuilder<PokerMasterPackage>, PokerMasterPackageBuilder>();
            //unityContainer.RegisterType<IPackageBuilder<Adda52Package>, Adda52PackageBuilder>();>
            unityContainer.RegisterType<IPackageBuilder<PPPokerPackage>, PPPokerPackageBuilder>();

            //var eventAggregator = Substitute.For<IEventAggregator>();
            //unityContainer.RegisterInstance(eventAggregator);

            var locator = new UnityServiceLocator(unityContainer);

            ServiceLocator.SetLocatorProvider(() => locator);
        }

        protected List<CapturedPacket> ReadCapturedPackets(string file, string dateFormat)
        {
            file = Path.Combine(TestDataFolder, file);
            return TcpImporterTestUtils.ReadCapturedPackets(file, dateFormat);
        }

        protected byte[] ReadPacketFile(string file)
        {
            file = Path.Combine(TestDataFolder, file);

            Assert.IsTrue(File.Exists(file), $"{file} doesn't exist");

            var fileText = File.ReadAllText(file);

            Assert.IsNotEmpty(fileText, $"{file} is empty");

            var bytes = fileText.FromHexStringToBytes();
            return bytes;
        }

        protected List<T> GetPackageTypeList<T>(string file)
        {
            file = Path.Combine(TestDataFolder, file);

            FileAssert.Exists(file);

            var commands = File.ReadAllLines(file)
                .Select(x => (T)Enum.Parse(typeof(T), x))
                .ToList();

            return commands;
        }
    }
}