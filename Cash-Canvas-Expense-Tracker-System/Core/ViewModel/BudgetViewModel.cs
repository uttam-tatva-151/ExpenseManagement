using CashCanvas.Core.Beans.Enums;
using CashCanvas.Core.DTOs;

namespace CashCanvas.Core.ViewModel;

public class BudgetViewModel
{
    public Guid BudgetId { get; set; }
    public Guid UserId { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;
    public decimal Amount { get; set; }
    public BudgetPeriod Period { get; set; } = BudgetPeriod.Monthly;
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public string? Notes { get; set; }
    public bool IsContinued { get; set; } = true;

    public DateTime CreatedAt { get; set; }

    public List<CategoryViewModel> Categories { get; set; } = [];
}
