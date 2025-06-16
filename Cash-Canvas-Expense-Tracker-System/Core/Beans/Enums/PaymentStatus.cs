using System.Runtime.Serialization;

namespace CashCanvas.Core.Beans.Enums;

public enum PaymentStatus
{
    [EnumMember(Value = "Complete")]
    Complete,
    [EnumMember(Value = "Pending")]
    Pending,
    [EnumMember(Value = "Failed")]
    Failed
}
