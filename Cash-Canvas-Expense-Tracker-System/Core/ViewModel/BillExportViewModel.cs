namespace CashCanvas.Core.ViewModel;

public class BillExportViewModel
{
    // Bill details
    public string Title { get; set; } = null!;
    public decimal Amount { get; set; }
    public DateTime DueDate { get; set; }
    public string Frequency { get; set; } = null!;
    public string PaymentMethod { get; set; } = null!;

    // Payment summary fields
    public DateTime? LastPaidDate { get; set; }
    public decimal TotalPaid { get; set; }
    public int MissedCount { get; set; }
    public int SkippedCount { get; set; }
}
