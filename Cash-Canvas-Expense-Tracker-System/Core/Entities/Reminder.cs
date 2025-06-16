using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashCanvas.Core.Entities
{
    /// <summary>
    /// Represents an extra reminder for a bill, set by a user.
    /// </summary>
    [Table("Reminders")]
    public class Reminder
    {
        [Key]
        public Guid ReminderId { get; set; }

        [Required]
        public Guid BillId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public DateTime ReminderDate { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }

        [Required]
        public bool IsContinued { get; set; } = true;

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        [ForeignKey(nameof(BillId))]
        public virtual Bill Bill { get; set; } = null!;
    }
}