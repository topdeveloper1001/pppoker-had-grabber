using System.Runtime.Serialization;
using HandHistories.Objects.Cards;
using System;
using System.Xml.Serialization;

namespace HandHistories.Objects.Actions
{
    [Serializable]
    public class WinningsAction : HandAction
    {
        [XmlAttribute]
        public int PotNumber { get; set; }

        public WinningsAction()
        {
        }

        public WinningsAction(string playerName, 
                              HandActionType handActionType, 
                              decimal amount,                               
                              int potNumber,
                              int actionNumber = 0) : base(playerName, handActionType, amount, Street.Summary, actionNumber)
        {
            PotNumber = potNumber;
        }

        public override string ToString()
        {
            return base.ToString() + "-Pot" + PotNumber;
        }
    }
}