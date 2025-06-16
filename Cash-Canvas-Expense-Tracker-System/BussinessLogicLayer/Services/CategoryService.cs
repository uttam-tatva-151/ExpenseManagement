using CashCanvas.Common.ConstantHandler;
using CashCanvas.Core.DTOs;
using CashCanvas.Core.Entities;
using CashCanvas.Data.UnitOfWork;
using CashCanvas.Services.Interfaces;

namespace CashCanvas.Services.Services;

public class CategoryService(IUnitOfWork unitOfWork) : ICategoryService
{

    private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<List<CategoryViewModel>> GetCategoryListAsync(Guid userId)
    {
        try
        {
            IEnumerable<Category> categories = await _unitOfWork.Categories.GetListAsync(
                                                                    t => t.IsActive && t.UserId == userId);
            List<CategoryViewModel> result = [.. categories.Select(c => new CategoryViewModel()
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                Description = c.Description,
                Type = c.Type,
                UserId = c.UserId
            })];
            return result;
        }
        catch (Exception ex)
        {
            throw new Exception(MessageHelper.GetErrorMessageForFeatchOperation(Constants.TRANSACTION_LIST), ex);
        }
    }

    
}
