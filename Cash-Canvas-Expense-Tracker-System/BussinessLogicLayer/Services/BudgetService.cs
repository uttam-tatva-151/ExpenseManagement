using CashCanvas.Core.Entities;
using CashCanvas.Core.ViewModel;
using CashCanvas.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using CashCanvas.Services.Interfaces;
using CashCanvas.Common.ConstantHandler;
using CashCanvas.Core.DTOs;
using CashCanvas.Core.Beans;
using CashCanvas.Core.Beans.Enums;

namespace CashCanvas.Services.Services;

public class BudgetService(IUnitOfWork unitOfWork, ICategoryService categoryService,INotificationService notificationService) : IBudgetService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICategoryService _categoryService = categoryService;
    private readonly INotificationService _notificationService = notificationService;


    public async Task<List<BudgetViewModel>> GetBudgetListAsync(Guid userId)
    {
        try
        {
            IEnumerable<Budget> transactions = await _unitOfWork.Budgets.GetListAsync(
                                                                    t => (t.IsContinued ||
                                                                            (!t.IsContinued && t.ModifiedAt >= DateTime.UtcNow.AddDays(-30))) &&
                                                                        t.UserId == userId,
                                                                    q => q.Include(x => x.Category));
            List<BudgetViewModel> result = [.. transactions.Select(t => new BudgetViewModel()
            {
                BudgetId = t.BudgetId,
                Amount = t.Amount,
                UserId = t.UserId,
                CategoryId = t.CategoryId,
                Notes = t.Notes?? string.Empty,
                CategoryName = t.Category.CategoryName,
                Period = t.Period,
                StartDate = t.StartDate,
                CreatedAt = t.CreatedAt,
                IsContinued = t.IsContinued
            })];
            return result;
        }
        catch (Exception ex)
        {
            throw new Exception(MessageHelper.GetErrorMessageForFeatchOperation(Constants.BUDGET_LIST), ex);
        }
    }

    public async Task<ResponseResult<List<CategoryViewModel>>> AddBudgetAsync(BudgetViewModel newBudget)
    {
        ResponseResult<List<CategoryViewModel>> response = new();
        try
        {
            if (newBudget == null || newBudget.UserId == Guid.Empty)
            {
                response.Message = Messages.WARNING_INVALID_INPUT;
                response.Status = ResponseStatus.Warning;
                return response;
            }
            Budget budget = new()
            {
                Amount = newBudget.Amount,
                UserId = newBudget.UserId,
                CategoryId = newBudget.CategoryId,
                Notes = newBudget.Notes,
                Period = newBudget.Period,
                StartDate = newBudget.StartDate,
                CreatedAt = DateTime.UtcNow,
                IsContinued = newBudget.IsContinued
            };
            budget.StartDate = budget.StartDate.ToUniversalTime();
            await _unitOfWork.Budgets.AddAsync(budget);
            int result = await _unitOfWork.CompleteAsync();
            if (result > 0)
            {
                await _notificationService.SyncBudgetWithNotificationsAsync(budget.UserId, budget, Constants.DATABASE_ACTION_CREATE);
                response.Data = await GetCategoryListAsync(newBudget.UserId);
                response.Message = MessageHelper.GetSuccessMessageForAddOperation(Constants.BUDGET);
                response.Status = ResponseStatus.Success;
            }
            else
            {
                response.Status = ResponseStatus.Failed;
                response.Message = MessageHelper.GetErrorMessageForAddOperation(Constants.BUDGET);
            }
            return response;
        }
        catch (Exception ex)
        {
            throw new Exception(MessageHelper.GetErrorMessageForAddOperation(Constants.BUDGET), ex);
        }
    }

    public async Task<BudgetViewModel> GetBudgetByIdAsync(Guid budgetId)
    {
        try
        {
            Budget? budget = await _unitOfWork.Budgets.GetFirstOrDefaultAsync(t => t.BudgetId == budgetId && t.IsContinued,
                                                                              q => q.Include(x => x.Category));
            if (budget == null)
            {
                throw new Exception(MessageHelper.GetErrorMessageForFeatchOperation(Constants.BUDGET));
            }
            List<CategoryViewModel> categories = await GetCategoryListAsync(budget.UserId);
            return new BudgetViewModel()
            {
                BudgetId = budget.BudgetId,
                Amount = budget.Amount,
                UserId = budget.UserId,
                CategoryId = budget.CategoryId,
                Notes = budget.Notes ?? string.Empty,
                CategoryName = budget.Category.CategoryName,
                Period = budget.Period,
                StartDate = budget.StartDate,
                CreatedAt = budget.CreatedAt,
                IsContinued = budget.IsContinued,
                Categories = categories
            };
        }
        catch (Exception ex)
        {
            throw new Exception(MessageHelper.GetErrorMessageForFeatchOperation(Constants.BUDGET), ex);
        }
    }



    public async Task<ResponseResult<bool>> UpdateBudgetAsync(BudgetViewModel budget)
    {
        ResponseResult<bool> response = new();
        try
        {
            if (budget == null || budget.BudgetId == Guid.Empty)
            {
                response.Message = Messages.WARNING_INVALID_INPUT;
                response.Status = ResponseStatus.Warning;
                return response;
            }
            Budget? existingBudget = await _unitOfWork.Budgets.GetByIdAsync(budget.BudgetId);
            if (existingBudget == null)
            {
                response.Message = MessageHelper.GetNotFoundMessage(Constants.BUDGET);
                response.Status = ResponseStatus.Warning;
                return response;
            }
            existingBudget.Amount = budget.Amount;
            existingBudget.UserId = budget.UserId;
            existingBudget.CategoryId = budget.CategoryId;
            existingBudget.Notes = budget.Notes;
            existingBudget.Period = budget.Period;
            existingBudget.StartDate = budget.StartDate.ToUniversalTime();
            existingBudget.ModifiedAt = DateTime.UtcNow;
            existingBudget.IsContinued = budget.IsContinued;

            _unitOfWork.Budgets.Update(existingBudget);
            
            int result = await _unitOfWork.CompleteAsync();
            if (result > 0)
            {
                existingBudget.Category = await _unitOfWork.Categories.GetByIdAsync(existingBudget.CategoryId)??new();
                await _notificationService.SyncBudgetWithNotificationsAsync(budget.UserId, existingBudget, Constants.DATABASE_ACTION_UPDATE);
                response.Status = ResponseStatus.Success;
                response.Message = MessageHelper.GetSuccessMessageForUpdateOperation(Constants.BUDGET);
                response.Data = true;
            }
            else
            {
                response.Status = ResponseStatus.Failed;
                response.Message = MessageHelper.GetErrorMessageForUpdateOperation(Constants.BUDGET);
                response.Data = false;
            }
            return response;
        }
        catch (Exception ex)
        {
            throw new Exception(MessageHelper.GetErrorMessageForUpdateOperation(Constants.BUDGET), ex);
        }
    }

    public async Task<ResponseResult<bool>> DeleteBudgetAsync(Guid budgetId)
    {
        ResponseResult<bool> response = new();
        try
        {
            if (budgetId == Guid.Empty)
            {
                response.Message = Messages.WARNING_INVALID_INPUT;
                response.Status = ResponseStatus.Warning;
                return response;
            }
            Budget? budget = await _unitOfWork.Budgets.GetByIdAsync(budgetId);
            if (budget == null)
            {
                response.Message = MessageHelper.GetNotFoundMessage(Constants.BUDGET);
                response.Status = ResponseStatus.Warning;
                return response;
            }
            budget.IsContinued = false; // Mark as not continued instead of deleting
            budget.ModifiedAt = DateTime.UtcNow;

            _unitOfWork.Budgets.Update(budget);
            int result = await _unitOfWork.CompleteAsync();
            if (result > 0)
            {
                await _notificationService.SyncBudgetWithNotificationsAsync(budget.UserId, budget, Constants.DATABASE_ACTION_DELETE);
                response.Data = true;
                response.Message = MessageHelper.GetSuccessMessageForDeleteOperation(Constants.BUDGET);
                response.Status = ResponseStatus.Success;
            }
            else
            {
                response.Status = ResponseStatus.Failed;
                response.Message = MessageHelper.GetErrorMessageForDeleteOperation(Constants.BUDGET);
                response.Data = false;
            }
            return response;
        }
        catch (Exception ex)
        {
            throw new Exception(MessageHelper.GetErrorMessageForDeleteOperation(Constants.BUDGET), ex);
        }
    }

    public async Task<List<CategoryViewModel>> GetCategoryListAsync(Guid userId)
    {
        return await _categoryService.GetCategoryListAsync(userId);
    }
}