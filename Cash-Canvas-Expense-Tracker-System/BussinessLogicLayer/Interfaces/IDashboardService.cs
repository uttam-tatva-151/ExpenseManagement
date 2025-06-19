using CashCanvas.Core.Beans;
using CashCanvas.Core.ViewModel;

namespace CashCanvas.Services.Interfaces;

public interface IDashboardService
{
    Task<DashboardViewModel> GetAnalysisData(PaginationDetails pagination, Guid userId);
}
