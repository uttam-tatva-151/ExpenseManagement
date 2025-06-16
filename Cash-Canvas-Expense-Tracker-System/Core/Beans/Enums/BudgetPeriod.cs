using System.Runtime.Serialization;

namespace CashCanvas.Core.Beans.Enums;

public enum BudgetPeriod
{
    [EnumMember(Value = "Weekly")]
    Weekly,
    [EnumMember(Value = "Monthly")]
    Monthly,
    [EnumMember(Value = "Quarterly")]
    Quarterly,
    [EnumMember(Value = "Yearly")]
    Yearly,
    [EnumMember(Value = "BiWeekly")]
    BiWeekly, // every 2 weeks
    [EnumMember(Value = "HalfYearly")]
    HalfYearly
}
