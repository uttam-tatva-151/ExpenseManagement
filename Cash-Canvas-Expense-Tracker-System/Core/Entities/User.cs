using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashCanvas.Core.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        public Guid UserId { get; set; } // Primary Key

        [Required]
        [StringLength(50, MinimumLength = 3)]
        [RegularExpression(@"^(?=(?:[^a-zA-Z_]*[a-zA-Z_]){3,})[a-zA-Z0-9_@]{3,20}$")]
        public string UserName { get; set; } = null!; // Unique username    

        [Required]
        [EmailAddress]
        [StringLength(100, MinimumLength = 5)]
        public string Email { get; set; } = null!; // Unique email address

        [Required]
        [StringLength(256, MinimumLength = 20)]
        public string HashPassword { get; set; } = null!; // Hashed password

        public DateTime? LastLoginAt { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedAt { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;
        public ICollection<Notifications> Notifications { get; set; } = new List<Notifications>(); // Navigation property for related notifications
    }
}