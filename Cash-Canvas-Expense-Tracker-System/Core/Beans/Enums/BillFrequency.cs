using System.Runtime.Serialization;

namespace CashCanvas.Core.Beans.Enums;

public enum BillFrequency
{
    [EnumMember(Value = "Daily")]
    Daily,
    [EnumMember(Value = "Weekly")]
    Weekly,
    [EnumMember(Value = "BiWeekly")]
    BiWeekly, // every 2
    [EnumMember(Value = "Monthly")]
    Monthly,
    [EnumMember(Value = "Quarterly")]
    Quarterly, //every 3 months
    [EnumMember(Value = "HalfYearly")]
    HalfYearly,
    [EnumMember(Value = "Yearly")]
    Yearly,
    [EnumMember(Value = "OneTime")]
    OneTime // only once
}
