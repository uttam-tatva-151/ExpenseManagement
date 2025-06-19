namespace CashCanvas.Core.ViewModel;

public class TransactionExportViewModel
{
    public string TransactionType { get; set; } = null!;  // "Expense" or "Income"
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = null!;
    public string? Description { get; set; }
    public string CategoryName { get; set; } = null!;
    public DateTime TransactionDate { get; set; }
    public DateTime CreatedAt { get; set; }
}
