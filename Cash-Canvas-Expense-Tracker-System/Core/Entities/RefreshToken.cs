using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashCanvas.Core.Entities
{
    /// <summary>
    /// Stores refresh tokens for JWT authentication, enabling secure session management, token rotation, auditing, and device-level tracking.
    /// </summary>
    [Table("RefreshTokens")]
    public class RefreshToken
    {
        [Key]
        public Guid TokenId { get; set; } // PK, DB-generated

        [Required]
        public Guid UserId { get; set; } // FK to User

        [Required]
        [StringLength(256)]
        public string Token { get; set; } = null!;// Secure, random string

        [Required]
        public DateTime ExpirationTime { get; set; } // Expiry timestamp

        [Required]
        public DateTime CreatedAt { get; set; } // Creation timestamp

        [Required]
        [StringLength(50)]
        public string CreatedByIp { get; set; } = null!;// IP at creation

        [Required]
        public bool IsRevoked { get; set; } = false; // Revocation status

        public DateTime? RevokedAt { get; set; } // When revoked

        [StringLength(50)]
        public string? RevokedByIp { get; set; } // IP at revocation

        [Required]
        public bool IsActive { get; set; } = true; // Is current/valid

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;
    }
}