using System.Net;

namespace PPPokerCardCatcher.Importers.PPPoker
{
    static class PPPConstants
    {
        public static IPAddress Address { get; } = IPAddress.Parse("209.200.155.113");
        public const int Port = 4000;
    }
}
