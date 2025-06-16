namespace CashCanvas.Core.DTOs;

public class ResetPasswordTokenDTO
{
    public DateTime ExpirationTime { get; set; } 
    public Guid TokenId { get; set; }
    public string ResetToken { get; set; } = null!;
    public Guid UserId { get; set; }

}
