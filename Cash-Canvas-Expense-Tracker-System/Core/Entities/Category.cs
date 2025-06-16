using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashCanvas.Core.Entities
{
    /// <summary>
    /// Represents a transaction category (e.g., Groceries, Salary, Rent) for a user.
    /// </summary>
    [Table("Categories")]
    public class Category
    {
        [Key]
        public Guid CategoryId { get; set; }
        
        [Required]
        public Guid UserId { get; set; } // FK to User

        [Required]
        [StringLength(50)]
        public string CategoryName { get; set; } = null!;// Category Name
        // [Required]
        // [StringLength(50)]
        // public string CategoryIcon { get; set; } = null!;// Category Icon as bootstrp icon class (e.g., "bi bi-bag-fill")

        [StringLength(200)]
        public string? Description { get; set; }

        [Required]
        [StringLength(20)]
        public string Type { get; set; } = null!;// "Income" or "Expense"
        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }= null!; 
    }
}