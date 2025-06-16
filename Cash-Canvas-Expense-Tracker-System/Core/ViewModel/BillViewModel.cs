using System.ComponentModel.DataAnnotations;
using CashCanvas.Core.Beans.Enums;
using CashCanvas.Core.Entities;

namespace CashCanvas.Core.ViewModel;

public class BillViewModel
{
    public Guid BillId { get; set; }
    public Guid UserId { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(100, ErrorMessage = "Title must be under 100 characters.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Amount is required.")]
    [Range(0.01, 10000000, ErrorMessage = "Amount must be greater than zero.")]
    [DataType(DataType.Currency)]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Due date is required.")]
    [DataType(DataType.Date)]
    public DateTime DueDate { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Please select frequency.")]
    public BillFrequency Frequency { get; set; } 

    [Required(ErrorMessage = "Payment method is required.")]
    public PaymentMethod PaymentMethod { get; set; } 

    [Required(ErrorMessage = "Reminder Day is required.")]
    [Range(0, 31, ErrorMessage = "Reminder day must be between 0 and 31.")]
    public int ReminderDay { get; set; }

    [MaxLength(500, ErrorMessage = "Notes can't exceed 500 characters.")]
    public string? Notes { get; set; }

    public bool IsContinued { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}
