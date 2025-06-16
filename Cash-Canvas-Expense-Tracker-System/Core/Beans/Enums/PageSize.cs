using System.Runtime.Serialization;

namespace CashCanvas.Core.Beans.Enums;

public enum PageSize
{
    [EnumMember(Value = "3 per page")]
    Three = 3,

    [EnumMember(Value = "5 per page")]
    Five = 5,

    [EnumMember(Value = "10 per page")]
    Ten = 10,

    [EnumMember(Value = "20 per page")]
    Twenty = 20,

    [EnumMember(Value = "50 per page")]
    Fifty = 50
}
