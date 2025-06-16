using CashCanvas.Core.DTOs;

namespace CashCanvas.Services.Interfaces;

public interface ICategoryService
{
    Task<List<CategoryViewModel>> GetCategoryListAsync(Guid userId);
}
