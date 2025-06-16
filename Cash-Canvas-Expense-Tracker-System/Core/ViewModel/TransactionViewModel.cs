using System.ComponentModel.DataAnnotations;
using CashCanvas.Core.Beans.Enums;
using CashCanvas.Core.DTOs;

namespace CashCanvas.Core.ViewModel;

public class TransactionViewModel
{
    [Required(ErrorMessage = "Transaction ID is required.")]
    public Guid TransactionId { get; set; } = Guid.NewGuid();

    [Required(ErrorMessage = "User ID is required.")]
    public Guid UserId { get; set; }

    [Required(ErrorMessage = "Amount is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Transaction type is required.")]
    [StringLength(50, ErrorMessage = "Transaction type must not exceed 50 characters.")]
    public string TransactionType { get; set; } = null!;

    [Required(ErrorMessage = "Payment method is required.")]
    public PaymentMethod PaymentMethod { get; set; }

    [StringLength(250, ErrorMessage = "Description can be up to 250 characters.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Category is required.")]
    public Guid CategoryId { get; set; }

    [Required(ErrorMessage = "Category name is required.")]
    [StringLength(100, ErrorMessage = "Category name must not exceed 100 characters.")]
    public string CategoryName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Creation date is required.")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required(ErrorMessage = "Transaction date is required.")]
    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

    public List<CategoryViewModel> Categories { get; set; } = [];
    public bool IsContinued { get; set; } = true;
}
