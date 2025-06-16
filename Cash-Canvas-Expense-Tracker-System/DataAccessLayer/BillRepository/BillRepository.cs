using CashCanvas.Core.Beans;
using CashCanvas.Core.Beans.Enums;
using CashCanvas.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CashCanvas.Data.BillRepository;

public class BillRepository(AppDbContext context) : IBillRepository
{
    private readonly AppDbContext _context = context;

    public async Task<List<Bill>> GetFilteredBillsAsync(Guid userId, PaginationDetails pagination)
    {
        IQueryable<Bill> query = _context.Bills.AsNoTracking().Where(t =>
                (t.IsContinued || (!t.IsContinued && t.ModifiedAt >= DateTime.UtcNow.AddDays(-30))) &&
                t.UserId == userId
            );

        // TIME FILTER
        if (pagination.TimeFilter != TimeFilterType.AllTime && pagination.TimeFilter != TimeFilterType.Custom)
        {
            DateTime? startDate = TimeFilterHelper.GetStartDate(pagination.TimeFilter);
            if (startDate != null)
            {
                query = query.Where(t => t.CreatedAt >= startDate);
            }
        }
        else if (pagination.TimeFilter == TimeFilterType.Custom)
        {
            query = query.Where(t =>
                t.CreatedAt >= pagination.FromDate &&
                t.CreatedAt <= pagination.ToDate
            );
        }

        // SEARCH TERM
        if (!string.IsNullOrWhiteSpace(pagination.SearchTerm))
        {
            query = query.Where(t =>
                t.Title.ToLower().Contains(pagination.SearchTerm.ToLower())
            );
        }

        // SORTING
        if (!string.IsNullOrWhiteSpace(pagination.SortBy))
        {
            switch (pagination.SortBy.ToLower())
            {
                case "name":
                    query = pagination.IsAscending ? query.OrderBy(t => t.Title) : query.OrderByDescending(t => t.Title);
                    break;
                case "amount":
                    query = pagination.IsAscending ? query.OrderBy(t => t.Amount) : query.OrderByDescending(t => t.Amount);
                    break;
                case "due":
                    query = pagination.IsAscending ? query.OrderBy(t => t.DueDate) : query.OrderByDescending(t => t.DueDate);
                    break;
                default:
                    query = query.OrderByDescending(t => t.CreatedAt); // fallback
                    break;
            }
        }
        else
        {
            query = query.OrderByDescending(t => t.CreatedAt); // default
        }
        pagination.TotalRecords = await query.CountAsync();
        // PAGINATION
        int skip = ((int)pagination.PageNumber - 1) * (int)pagination.PageSize;
        query = query.Skip(skip).Take((int)pagination.PageSize);

        return await query.ToListAsync();
    }
}
