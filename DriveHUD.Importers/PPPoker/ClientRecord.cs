using DriveHUD.Importers.PPPoker.Model;
using HandHistories.Objects.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveHUD.Importers.PPPoker
{
    class ClientRecord
    {
        public int RoomID { get; set; }

        public string RoomName { get; set; }

        public int Port { get; set; }

        public long[] Pots { get; set; }

        public decimal Ante { get; set; }

        public decimal BigBlind { get; set; }

        public decimal SmallBlind { get { return BigBlind / 2; } }

        public bool IsTournament { get; set; }

        public int MaxPlayers { get; set; }

        public Dictionary<int, RoomPlayer> Players { get; set; } = new Dictionary<int, RoomPlayer>(); // SeatID => RoomPlayer

        public long HeroUid { get; set; }

        public List<PPPokerPackage> Packages { get; set; } = new List<PPPokerPackage>();

        public List<Action> DelayedActions { get; set; } = new List<Action>();
    }
}
