using CashCanvas.Core.Beans.Enums;

namespace CashCanvas.Core.Beans;

public class PaginationDetails
{
    public int TotalRecords { get; set; } = 0; 

    // Paging

    public int PageNumber { get; set; } = 1;
    public PageSize PageSize { get; set; } = PageSize.Five;

    // Sorting
    public string? SortBy { get; set; }       // e.g., "name", "amount", "due"
    public bool IsAscending { get; set; } = true;

    // Time Filter
    public TimeFilterType TimeFilter { get; set; } = TimeFilterType.AllTime;
    public DateTime FromDate { get; set; } = DateTime.MinValue;
    public DateTime ToDate { get; set; } = DateTime.MaxValue;

    // Search
    public string SearchTerm { get; set; } = string.Empty;

    // Any future filters (extendable)
    // public Dictionary<string, string>? AdditionalFilters { get; set; }
}
