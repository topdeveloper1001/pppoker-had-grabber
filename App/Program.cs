using DriveHUD.Importers.AndroidBase;
using DriveHUD.Importers.PPPoker;
using DriveHUD.Importers.PPPoker.Model;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var unityContainer = new UnityContainer();
                unityContainer.RegisterType<IPackageBuilder<PPPokerPackage>, PPPokerPackageBuilder>();
                unityContainer.RegisterType<IPacketManager<PPPokerPackage>, PPPokerPacketManager>();

                var locator = new UnityServiceLocator(unityContainer);
                ServiceLocator.SetLocatorProvider(() => locator);


                TcpImporter importer = new TcpImporter();

                importer.Start();

                Console.WriteLine("Press any key to stop...");
                Console.ReadKey();

                importer.Stop();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
        }
    }
}
