namespace CashCanvas.Core.DTOs;

public class UserDTO
{
    public Guid UserId { get; set; }
    public string UserName { get; set; } = null!;
    public string UserEmail { get; set; } = null!;
    public string HashPassword { get; set; } = null!;
    public string AccessToken { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } 
}
