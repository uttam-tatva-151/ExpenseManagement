namespace CashCanvas.Core.DTOs;

public class LoginResult
{
    public string AccessToken { get; set; } = string.Empty; 
    public string RefreshToken { get; set; } = string.Empty; 
    public DateTime AccessTokenExpiry { get; set; } 
    public DateTime RefreshTokenExpiry { get; set; } 
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty; 
    public string Email { get; set; } = string.Empty;
}
