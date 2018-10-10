using System.Runtime.Serialization;

namespace HandHistories.Objects.GameDescription
{
    [DataContract]
    public enum Currency : byte
    {
        [EnumMember] PlayMoney = 0,
        [EnumMember] USD = 1,
        [EnumMember] GBP = 2,
        [EnumMember] EURO = 3,
        [EnumMember] Chips = 4,
        [EnumMember] CAD = 5,
        [EnumMember] YUAN = 6,
        [EnumMember] All = 7,
        [EnumMember] INR = 8,
    }
}