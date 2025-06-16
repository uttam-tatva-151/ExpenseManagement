using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashCanvas.Core.Entities
{
    [Table("PasswordRecoveryTokens")]
    public class PasswordRecoveryToken
    {
        [Key]
        public Guid TokenId { get; set; } // Primary Key

        [Required]
        public Guid UserId { get; set; } // Foreign Key to Users

        [Required]
        [StringLength(256)]
        public string Token { get; set; } = null!;// Recovery token (hashed or random string)

        [Required]
        public DateTime ExpirationTime { get; set; } // When token expires

        [Required]
        public bool IsUsed { get; set; } = false; // Has the token been used

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Creation time

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;// Navigation property
    }
}