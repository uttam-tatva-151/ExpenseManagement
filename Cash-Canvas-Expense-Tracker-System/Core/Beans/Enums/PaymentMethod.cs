using System.Runtime.Serialization;

namespace CashCanvas.Core.Beans.Enums;

public enum PaymentMethod
{
    [EnumMember(Value = "Cash")]
    Cash,
    [EnumMember(Value = "Card")]
    Card,
    [EnumMember(Value = "UPI")]
    UPI,
    [EnumMember(Value = "NetBanking")]
    NetBanking,
    [EnumMember(Value = "Cheque")]
    Cheque,
    [EnumMember(Value = "Wallet")]
    Wallet
}
