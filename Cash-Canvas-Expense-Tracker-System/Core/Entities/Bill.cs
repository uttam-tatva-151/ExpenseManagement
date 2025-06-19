using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CashCanvas.Core.Beans.Enums;

namespace CashCanvas.Core.Entities
{
    /// <summary>
    /// Represents a recurring bill for a user (e.g., rent, utility), with reminders and payment tracking.
    /// </summary>
    [Table("Bills")]
    public class Bill
    {
        [Key]
        public Guid BillId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public BillFrequency Frequency { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        /// <summary>
        /// For monthly/yearly bills: 1-31, for weekly: 0-6 (0=Sunday), or 0 if no reminder.
        /// </summary>
        [Required]
        public int ReminderDay { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }

        [Required]
        public bool IsContinued { get; set; } = true;
        public DateTime? NextDueDate { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

       public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}