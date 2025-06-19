using System.Runtime.Serialization;

namespace CashCanvas.Core.Beans.Enums;

public enum NotificationSourceType
{
    [EnumMember(Value = "Bill")]
    Bill,
    [EnumMember(Value = "Reminder")]
    Reminder,
    [EnumMember(Value = "Budget")]
    Budget,
    [EnumMember(Value = "Transaction")]
    Transaction,
    [EnumMember(Value = "Custom")]
    Custom // For any other custom notifications
}
