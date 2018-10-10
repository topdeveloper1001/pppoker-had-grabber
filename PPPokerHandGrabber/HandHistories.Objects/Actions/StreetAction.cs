using System.Runtime.Serialization;
using HandHistories.Objects.Cards;

namespace HandHistories.Objects.Actions
{
    [DataContract]
    public class StreetAction : HandAction
    {
        public StreetAction(Street street) : 
            base(string.Empty, HandActionType.STREET,0, street,0)
        {
        }
    }
}