using System.ComponentModel.DataAnnotations;

namespace CashCanvas.Core.ViewModel;

public class ChangePasswordViewModel
{
    [Required]
    [DataType(DataType.Password)]
    public string CurrentPassword { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "The new password and confirmation do not match.")]
    public string ConfirmNewPassword { get; set; } = null!;
} 
