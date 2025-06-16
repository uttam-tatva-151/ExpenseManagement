using CashCanvas.Core.Beans;
using CashCanvas.Core.DTOs;
using CashCanvas.Core.ViewModel;

namespace CashCanvas.Services.Interfaces;

public interface IBudgetService
{
    Task<List<BudgetViewModel>> GetBudgetListAsync(Guid userId);
    Task<List<CategoryViewModel>> GetCategoryListAsync(Guid userId);
    Task<ResponseResult<List<CategoryViewModel>>> AddBudgetAsync(BudgetViewModel newBudget);
    Task<BudgetViewModel> GetBudgetByIdAsync(Guid budgetId);
    Task<ResponseResult<bool>> UpdateBudgetAsync(BudgetViewModel budget);
    Task<ResponseResult<bool>> DeleteBudgetAsync(Guid budgetId);
}
