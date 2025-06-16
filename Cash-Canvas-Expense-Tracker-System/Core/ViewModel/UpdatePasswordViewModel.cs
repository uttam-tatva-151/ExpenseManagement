using System.ComponentModel.DataAnnotations;

namespace CashCanvas.Core.ViewModel;

public class UpdatePasswordViewModel
{
    public string Token { get; set; } = string.Empty;

    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{4,}$",
    ErrorMessage = "Password must include at least one uppercase letter, lowercase letter, number, and special character (such as !, @, #, $, %, etc.).")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    [MaxLength(16, ErrorMessage = "Password cannot be longer than 16 characters.")]
    [Required(ErrorMessage = "New password is required.")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Confirm password is required.")]
    [MaxLength(16, ErrorMessage = "Password cannot be longer than 16 characters.")]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; } = null!;
}
