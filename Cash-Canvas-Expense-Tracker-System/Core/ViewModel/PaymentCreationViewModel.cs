using System.ComponentModel.DataAnnotations;
using CashCanvas.Core.Beans.Enums;

namespace CashCanvas.Core.ViewModel;

/// <summary>
/// Model for capturing payment details on "Mark as Paid" action.
/// </summary>
public class PaymentCreationViewModel
{
    [Required(ErrorMessage = "Bill ID is required.")]
    public Guid BillId { get; set; }
    public Guid UserId { get; set; }

    [Required(ErrorMessage = "Amount paid is required.")]
    [Range(0.01, 9999999999999999.99, ErrorMessage = "Amount must be greater than 0.")]
    public decimal AmountPaid { get; set; }

    [Required(ErrorMessage = "Payment date is required.")]
    [DataType(DataType.Date)]
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

    [Required(ErrorMessage = "Payment method is required.")]
    public PaymentMethod PaymentMethod { get; set; }

    [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
    public string? Notes { get; set; }
}
