using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CashCanvas.Core.Beans.Enums;

namespace CashCanvas.Core.Entities
{
    /// <summary>
    /// Represents a payment made for a bill by a user.
    /// </summary>
    [Table("Payments")]
    public class Payment
    {
        [Key]
        public Guid PaymentId { get; set; }

        [Required]
        public Guid BillId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountPaid { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        [Required]
        public PaymentStatus Status { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }

        [Required]
        public bool IsContinued { get; set; } = true;

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        public  Bill Bill { get; set; } = null!;
    }
}