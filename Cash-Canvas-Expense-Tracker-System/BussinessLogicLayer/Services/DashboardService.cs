using CashCanvas.Common.ConstantHandler;
using CashCanvas.Core.Beans;
using CashCanvas.Core.Beans.Enums;
using CashCanvas.Core.Entities;
using CashCanvas.Core.ViewModel;
using CashCanvas.Data.UnitOfWork;
using CashCanvas.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace CashCanvas.Services.Services;

public class DashboardService(IUnitOfWork unitOfWork) : IDashboardService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<DashboardViewModel> GetAnalysisData(PaginationDetails pagination, Guid userId)
    {
        try
        {
            List<Transaction> transactions = await _unitOfWork.Transactions.GetListAsync(t => t.UserId == userId && t.IsContinued,
                                                                                            q => q.Include(x => x.Category));
            List<Category> categories = await _unitOfWork.Categories.GetListAsync(t => t.UserId == userId && t.IsActive);
            List<Bill> bills = await _unitOfWork.Bills.GetListAsync(t => t.UserId == userId && t.IsContinued,
                                                                        q => q.Include(x => x.Payments));

            List<string> labels = GetChartLabels(pagination);
            List<string> categoryLabels = GetExpenseCategoryLabels(categories);

            DashboardViewModel analysis = new()
            {
                TotalIncome = GetTotalIncome(transactions),
                TotalExpense = GetTotalExpense(transactions),
                Labels = labels,
                IncomeData = GetIncomeData(labels, transactions, pagination),
                ExpenseData = GetExpenseData(labels, transactions, pagination),
                ExpenseCategoryLabels = categoryLabels,
                ExpenseCategoryData = GetExpenseCategoryData(categoryLabels, transactions),
                RecentExpenses = GetRecentExpenses(3, transactions),
                UpcomingBills = GetUpcomingBills(3, bills),
                OverDueBills = GetOverdueBills(bills)
            };
            analysis.OverdueBillsCount = analysis.OverDueBills.Count;

            return analysis;
        }
        catch (Exception ex)
        {
            throw new Exception(Messages.GENERAL_ERROR, ex);
        }
    }
    // Financial Summary
    private static decimal GetTotalIncome(List<Transaction> transactions)
    {
        return transactions.Where(t => t.TransactionType == Constants.TRANSACTION_TYPE_INCOME).Sum(t => t.Amount);
    }
    private static decimal GetTotalExpense(List<Transaction> transactions)
    {
        return transactions.Where(t => t.TransactionType == Constants.TRANSACTION_TYPE_EXPENSE).Sum(t => t.Amount);
    }

    // Chart Data
    private static List<string> GetChartLabels(PaginationDetails pagination)
    {
        List<string> labels = [];
        DateTime fromDate, toDate;

        switch (pagination.TimeFilter)
        {
            case TimeFilterType.Today:
            case TimeFilterType.Yesterday:
                fromDate = TimeFilterHelper.GetStartDate(pagination.TimeFilter) ?? DateTime.UtcNow.Date;
                toDate = pagination.TimeFilter == TimeFilterType.Today
                    ? fromDate
                    : DateTime.UtcNow.Date;

                // For today/yesterday, generate labels in 3-hour intervals
                int startHour = 0;
                int endHour = 24;
                DateTime date = fromDate;

                while (date <= toDate)
                {
                    for (int h = startHour; h < endHour; h += 3)
                    {
                        string label = $"{date:MM-dd} {h:00}:00";
                        labels.Add(label);
                    }
                    date = date.AddDays(1);
                }
                break;

            case TimeFilterType.Last3Days:
            case TimeFilterType.Last7Days:
            case TimeFilterType.Last15Days:
                fromDate = TimeFilterHelper.GetStartDate(pagination.TimeFilter) ?? DateTime.UtcNow.Date;
                toDate = DateTime.UtcNow.Date;
                for (DateTime dt = fromDate; dt <= toDate; dt = dt.AddDays(1))
                    labels.Add(dt.ToString("dd-MM"));
                break;

            case TimeFilterType.ThisMonth:
            case TimeFilterType.LastMonth:
                fromDate = TimeFilterHelper.GetStartDate(pagination.TimeFilter) ?? new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
                toDate = fromDate.AddMonths(1).AddDays(-1);
                for (DateTime dt = fromDate; dt <= toDate; dt = dt.AddDays(1))
                    labels.Add(dt.ToString("dd-MM-yyyy"));
                break;

            case TimeFilterType.Custom:
                fromDate = pagination.FromDate.Date;
                toDate = pagination.ToDate.Date;
                for (DateTime dt = fromDate; dt <= toDate; dt = dt.AddDays(1))
                    labels.Add(dt.ToString("dd-MM-yyyy"));
                break;

            case TimeFilterType.AllTime:
            default:
                DateTime now = DateTime.UtcNow;
                for (int i = 0; i < 12; i++)
                {
                    DateTime month = now.AddMonths(-i);
                    labels.Add(month.ToString("MM-yyyy"));
                }
                labels.Reverse();
                break;
        }

        return labels;
    }
    private static List<decimal> GetIncomeData(List<string> labels, List<Transaction> transactions, PaginationDetails pagination)
    {
        return GetChartData(labels, transactions, Constants.TRANSACTION_TYPE_INCOME, pagination);
    }
    private static List<decimal> GetExpenseData(List<string> labels, List<Transaction> transactions, PaginationDetails pagination)
    {
        return GetChartData(labels, transactions, Constants.TRANSACTION_TYPE_EXPENSE, pagination);
    }
    private static List<decimal> GetChartData(List<string> labels, List<Transaction> transactions, string type, PaginationDetails pagination)
    {
        return [.. labels.Select(label =>
    {
        switch (pagination.TimeFilter)
        {
            case TimeFilterType.Today:
            case TimeFilterType.Yesterday:
                // label: "MM-dd hh:00"
                string datePart = label[..5]; // "MM-dd"
                int hourPart = int.Parse(label.Substring(6, 2)); // "hh"
                return transactions
                    .Where(t => t.TransactionType == type &&
                                t.TransactionDate.ToString("MM-dd") == datePart &&
                                t.TransactionDate.Hour >= hourPart && t.TransactionDate.Hour < hourPart + 3)
                    .Sum(t => t.Amount);

            case TimeFilterType.Last3Days:
            case TimeFilterType.Last7Days:
            case TimeFilterType.Last15Days:
                // label: "dd-MM"
                return transactions
                    .Where(t => t.TransactionType == type &&
                                t.TransactionDate.ToString("dd-MM") == label)
                    .Sum(t => t.Amount);

            case TimeFilterType.ThisMonth:
            case TimeFilterType.LastMonth:
            case TimeFilterType.Custom:
                // label: "dd-MM-yy"
                return transactions
                    .Where(t => t.TransactionType == type &&
                                t.TransactionDate.ToString("dd-MM-yyyy") == label)
                    .Sum(t => t.Amount);

            case TimeFilterType.AllTime:
            default:
                // label: "MM-yy"
                return transactions
                    .Where(t => t.TransactionType == type &&
                                t.TransactionDate.ToString("MM-yyyy") == label)
                    .Sum(t => t.Amount);
        }
    })];
    }

    // Pie Chart Data
    private static List<string> GetExpenseCategoryLabels(List<Category> categories)
    {
        return categories?.Select(c => c.CategoryName).ToList() ?? [];
    }
    private static List<decimal> GetExpenseCategoryData(List<string> categoryLabels, List<Transaction> transactions)
    {
        return [.. categoryLabels.Select(label =>
                                                transactions
                                                    .Where(t => t.Category?.CategoryName == label)
                                                    .Sum(t => t.Amount)
                                            )];
    }

    // Lists
    private static List<ExpenseItem> GetRecentExpenses(int count, List<Transaction> transactions)
    {
        return [.. transactions
                        .Where(t => t.TransactionType == Constants.TRANSACTION_TYPE_EXPENSE)
                        .OrderByDescending(t => t.TransactionDate)
                        .Take(count)
                        .Select(t => new ExpenseItem
                        {
                            Category = t.Category?.CategoryName ?? string.Empty,
                            Amount = t.Amount,
                            Date = t.TransactionDate.ToString("dd-MM-yyyy"),
                            Description = t.Description ?? string.Empty
                        })];
    }
    private static List<BillItem> GetUpcomingBills(int count, List<Bill> bills)
    {
        DateTime now = DateTime.UtcNow.Date;
        return [.. bills
                        .Where(b =>
                            b.NextDueDate >= now &&
                            !b.Payments.Any(p => p.Status == PaymentStatus.Complete && p.PaymentDate.Date == b.NextDueDate)
                        )
                        .OrderBy(b => b.NextDueDate)
                        .Take(count)
                        .Select(b => new BillItem
                        {
                            Name = b.Title,
                            Amount = b.Amount,
                            DueDate = b.NextDueDate?.ToString("dd-MM-yyyy")??b.DueDate.ToString("dd-MM-yyyy"),
                            Description = b.Notes ?? string.Empty
                        })];
    }
    private static List<BillItem> GetOverdueBills(List<Bill> bills)
    {
        DateTime now = DateTime.UtcNow.Date;
        return bills
            .Where(b =>
                b.NextDueDate < now && // Overdue
                !b.Payments.Any(p => p.Status == PaymentStatus.Complete && p.PaymentDate.Date == b.NextDueDate) // Not paid for this due date
            )
            .OrderBy(b => b.NextDueDate)
            .Select(b => new BillItem
            {
                Name = b.Title,
                Amount = b.Amount,
                DueDate = b.NextDueDate?.ToString("yy-MM-dd") ?? b.DueDate.ToString("yy-MM-dd"),
                Description = b.Notes ?? string.Empty
            })
            .ToList();
    }

}
