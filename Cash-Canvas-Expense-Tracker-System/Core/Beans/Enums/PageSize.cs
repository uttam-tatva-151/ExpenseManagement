using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CashCanvas.Core.Beans.Enums;

public enum PageSize
{
    [Display(Name = "3 per page")]
    Three = 3,

    [Display(Name = "5 per page")]
    Five = 5,

    [Display(Name = "10 per page")]
    Ten = 10,

    [Display(Name = "20 per page")]
    Twenty = 20,

    [Display(Name = "50 per page")]
    Fifty = 50
}
