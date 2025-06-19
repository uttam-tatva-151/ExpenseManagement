using System.Runtime.Serialization;

namespace CashCanvas.Core.Beans.Enums;

public enum NotificationType
{
    [EnumMember(Value = "Reminder")]
    Reminder,
    
    [EnumMember(Value = "Milestone")]
    Milestone,

    [EnumMember(Value = "Other")]
    Other
}
