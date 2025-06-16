using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CashCanvas.Core.Beans.Enums;

namespace CashCanvas.Core.Entities
{
    /// <summary>
    /// Represents a financial transaction (income or expense) associated with a user and category.
    /// </summary>
    [Table("Transactions")]
    public class Transaction
    {
        [Key]
        public Guid TransactionId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        /// <summary>
        /// "Expense" or "Income"
        /// </summary>
        [Required]
        [StringLength(10)]
        public string TransactionType { get; set; } = null!;

        [Required]
        public PaymentMethod PaymentMethod { get; set; } 

        [StringLength(300)]
        public string? Description { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        public bool IsContinued { get; set; } = true;

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } =null!;

        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; } =null!;
    }
}