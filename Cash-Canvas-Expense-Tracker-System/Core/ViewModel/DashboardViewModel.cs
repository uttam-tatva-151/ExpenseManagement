namespace CashCanvas.Core.ViewModel;

public class DashboardViewModel
{
    // For Financial Summary
    public decimal TotalIncome { get; set; }
    public decimal TotalExpense { get; set; }
    public decimal Balance => TotalIncome - TotalExpense;
    public int OverdueBillsCount { get; set; }

    // For Income/Expense chart
    public List<string> Labels { get; set; } = [];
    public List<decimal> IncomeData { get; set; }  = [];
    public List<decimal> ExpenseData { get; set; }  = [];

    // For Category Pie Chart
    public List<string> ExpenseCategoryLabels { get; set; } = [];
    public List<decimal> ExpenseCategoryData { get; set; } = [];

    // For Recent Expenses List
    public List<ExpenseItem> RecentExpenses { get; set; } = [];

    // For Upcoming Bills List
    public List<BillItem> UpcomingBills { get; set; } = [];
    public List<BillItem> OverDueBills { get; set; } = [];
}

public class ExpenseItem
{
    public string Category { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Date { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class BillItem
{
    public string Name { get; set; } = string.Empty;
    public decimal Amount { get; set; } 
    public string DueDate { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}