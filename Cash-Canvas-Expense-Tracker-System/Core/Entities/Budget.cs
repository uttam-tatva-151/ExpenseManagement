using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CashCanvas.Core.Beans.Enums;

namespace CashCanvas.Core.Entities
{
    /// <summary>
    /// Represents a period-based budget set by a user for a specific category.
    /// </summary>
    [Table("Budgets")]
    public class Budget
    {
        [Key]
        public Guid BudgetId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public BudgetPeriod Period { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [StringLength(300)]
        public string? Notes { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }

        [Required]
        public bool IsContinued { get; set; } = true;

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; } = null!;
    }
}