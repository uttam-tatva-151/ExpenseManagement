using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CashCanvas.Core.Beans.Enums;


public enum TimeFilterType
{
    [EnumMember(Value = "All Time")]
    [Display(Name = "All Time")]
    AllTime,

    [EnumMember(Value = "Today")]
    [Display(Name = "Today")]
    Today,

    [EnumMember(Value = "Yesterday")]
    [Display(Name = "Yesterday")]
    Yesterday,

    [EnumMember(Value = "Last 3 Days")]
    [Display(Name = "Last 3 Days")]
    Last3Days,

    [EnumMember(Value = "Last 7 Days")]
    [Display(Name = "Last 7 Days")]
    Last7Days,

    [EnumMember(Value = "Last 15 Days")]
    [Display(Name = "Last 15 Days")]
    Last15Days,

    [EnumMember(Value = "This Month")]
    [Display(Name = "This Month")]
    ThisMonth,

    [EnumMember(Value = "Last Month")]
    [Display(Name = "Last Month")]
    LastMonth,

    [EnumMember(Value = "Custom Range")]
    [Display(Name = "Custom Range")]
    Custom
}
public static class TimeFilterHelper
{
    public static DateTime? GetStartDate(TimeFilterType filterType)
    {
        var now = DateTime.UtcNow.Date;

        return filterType switch
        {
            TimeFilterType.Today => now,
            TimeFilterType.Yesterday => now.AddDays(-1),
            TimeFilterType.Last3Days => now.AddDays(-3),
            TimeFilterType.Last7Days => now.AddDays(-7),
            TimeFilterType.Last15Days => now.AddDays(-15),
            TimeFilterType.ThisMonth => new DateTime(now.Year, now.Month, 1),
            TimeFilterType.LastMonth => new DateTime(now.Year, now.Month, 1).AddMonths(-1),
            _ => null
        };
    }
}

