namespace CashCanvas.Core.ViewModel;

public class PaymentHistoryViewModel
{
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public bool IsPaid { get; set; }
    public bool IsSkipped { get; set; }
    public decimal? PaidAmount { get; set; }
    public DateTime? PaidDate { get; set; }
    public Guid? PaymentId { get; set; }
    public Guid BillId { get; set; }
}
