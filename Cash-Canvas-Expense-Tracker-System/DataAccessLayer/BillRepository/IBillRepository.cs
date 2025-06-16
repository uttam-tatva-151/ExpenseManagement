using CashCanvas.Core.Beans;
using CashCanvas.Core.Entities;

namespace CashCanvas.Data.BillRepository;

public interface IBillRepository
{
    Task<List<Bill>> GetFilteredBillsAsync(Guid userId, PaginationDetails pagination);
}
