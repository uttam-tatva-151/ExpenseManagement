using CashCanvas.Core.Entities;
using CashCanvas.Core.ViewModel;
using CashCanvas.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using CashCanvas.Services.Interfaces;
using CashCanvas.Common.ConstantHandler;
using CashCanvas.Core.Beans;
using CashCanvas.Core.Beans.Enums;
using CashCanvas.Core.DTOs;


namespace CashCanvas.Services.Services;

public class TransactionService(IUnitOfWork unitOfWork,ICategoryService categoryService) : ITransactionService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICategoryService _categoryService = categoryService;
    public async Task<List<TransactionViewModel>> GetTransactionListAsync(Guid userId)
    {
        try
        {
            IEnumerable<Transaction> transactions = await _unitOfWork.Transactions.GetListAsync(
                                                                    t => (t.IsContinued ||
                                                                            (!t.IsContinued && t.ModifiedAt >= DateTime.UtcNow.AddDays(-30))) &&
                                                                        t.UserId == userId,
                                                                    q => q.Include(x => x.Category));
            List<TransactionViewModel> result = [.. transactions.Select(t => new TransactionViewModel()
            {
                TransactionId = t.TransactionId,
                Amount = t.Amount,
                TransactionType = t.TransactionType,
                PaymentMethod = t.PaymentMethod,
                Description = t.Description?? string.Empty,
                CategoryId = t.CategoryId,
                CategoryName = t.Category.CategoryName,
                CreatedAt = t.CreatedAt,
                TransactionDate = t.TransactionDate,
                IsContinued = t.IsContinued
            })];
            return result;
        }
        catch (Exception ex)
        {
            throw new Exception(MessageHelper.GetErrorMessageForFeatchOperation(Constants.TRANSACTION_LIST), ex);
        }
    }

        public async Task<List<CategoryViewModel>> GetCategoryListAsync(Guid userId)
    {
        return await _categoryService.GetCategoryListAsync(userId);
    }
public async Task<TransactionViewModel> GetTransactionByIdAsync(Guid transactionId)
    {
        try
        {
            Transaction? transaction = await _unitOfWork.Transactions.GetFirstOrDefaultAsync(t => t.TransactionId == transactionId && t.IsContinued,
                                                                    q => q.Include(x => x.Category));

            if (transaction == null)
            {
                throw new Exception(MessageHelper.GetErrorMessageForFeatchOperation(Constants.TRANSACTION));
            }
            List<CategoryViewModel> categories = await GetCategoryListAsync(transaction.UserId);
            return new TransactionViewModel()
            {
                TransactionId = transaction.TransactionId,
                Amount = transaction.Amount,
                TransactionType = transaction.TransactionType,
                PaymentMethod = transaction.PaymentMethod,
                Description = transaction.Description ?? string.Empty,
                CategoryId = transaction.CategoryId,
                CategoryName = transaction.Category.CategoryName,
                CreatedAt = transaction.CreatedAt,
                TransactionDate = transaction.TransactionDate,
                IsContinued = transaction.IsContinued,
                Categories = categories
            };
        }
        catch (Exception ex)
        {
            throw new Exception(MessageHelper.GetErrorMessageForFeatchOperation(Constants.TRANSACTION_LIST), ex);
        }
    }

    public async Task<ResponseResult<List<CategoryViewModel>>> AddTransactionAsync(TransactionViewModel transactionViewModel)
    {
        try
        {
            ResponseResult<List<CategoryViewModel>> response = new();
            Transaction transaction = new()
            {
                TransactionId = Guid.NewGuid(),
                UserId = transactionViewModel.UserId,
                Amount = transactionViewModel.Amount,
                TransactionType = transactionViewModel.TransactionType,
                PaymentMethod = transactionViewModel.PaymentMethod,
                Description = transactionViewModel.Description,
                CategoryId = transactionViewModel.CategoryId,
                CreatedAt = DateTime.UtcNow,
                IsContinued = transactionViewModel.IsContinued
            };
            transaction.TransactionDate = DateTime.SpecifyKind(transactionViewModel.TransactionDate, DateTimeKind.Local).ToUniversalTime();
            await _unitOfWork.Transactions.AddAsync(transaction);

            int result = await _unitOfWork.CompleteAsync();
            {
                if (result > 0)
                {
                    response.Status = ResponseStatus.Success;
                    response.Message = MessageHelper.GetSuccessMessageForAddOperation(Constants.TRANSACTION);
                    response.Data = await GetCategoryListAsync(transactionViewModel.UserId);
                }
                else
                {
                    {
                        response.Status = ResponseStatus.Failed;
                        response.Message = MessageHelper.GetErrorMessageForAddOperation(Constants.TRANSACTION);
                        response.Data = await GetCategoryListAsync(transactionViewModel.UserId);
                    }
                }
                return response;

            }
        }
        catch (Exception ex)
        {
            throw new Exception(MessageHelper.GetErrorMessageForFeatchOperation(Constants.TRANSACTION_LIST), ex);
        }
    }

    public async Task<ResponseResult<bool>> UpdateTransactionAsync(TransactionViewModel transactionViewModel)
    {
        try
        {
            ResponseResult<bool> response = new();
            Transaction? transaction = await _unitOfWork.Transactions.GetFirstOrDefaultAsync(t => t.TransactionId == transactionViewModel.TransactionId);
            if (transaction == null)
            {
                response.Status = ResponseStatus.Failed;
                response.Message = MessageHelper.GetErrorMessageForFeatchOperation(Constants.TRANSACTION);
                response.Data = false;
                return response;
            }
            transaction.Amount = transactionViewModel.Amount;
            transaction.TransactionType = transactionViewModel.TransactionType;
            transaction.PaymentMethod = transactionViewModel.PaymentMethod;
            transaction.Description = transactionViewModel.Description;
            transaction.CategoryId = transactionViewModel.CategoryId;
            transaction.IsContinued = transactionViewModel.IsContinued;
            transaction.ModifiedAt = DateTime.UtcNow;
            transaction.TransactionDate = DateTime.SpecifyKind(transactionViewModel.TransactionDate, DateTimeKind.Local).ToUniversalTime();

            _unitOfWork.Transactions.Update(transaction);

            int result = await _unitOfWork.CompleteAsync();
            if (result > 0)
            {
                response.Status = ResponseStatus.Success;
                response.Message = MessageHelper.GetSuccessMessageForUpdateOperation(Constants.TRANSACTION);
                response.Data = true;
            }
            else
            {
                response.Status = ResponseStatus.Failed;
                response.Message = MessageHelper.GetErrorMessageForUpdateOperation(Constants.TRANSACTION);
                response.Data = false;
            }
            return response;

        }
        catch (Exception ex)
        {
            throw new Exception(MessageHelper.GetErrorMessageForFeatchOperation(Constants.TRANSACTION_LIST), ex);
        }
    }

    public async Task<ResponseResult<bool>> DeleteTransactionAsync(Guid transactionId)
    {
        try
        {
            ResponseResult<bool> response = new();
            Transaction? transaction = await _unitOfWork.Transactions.GetFirstOrDefaultAsync(t => t.TransactionId == transactionId);
            if (transaction == null)
            {
                response.Status = ResponseStatus.Failed;
                response.Message = MessageHelper.GetErrorMessageForFeatchOperation(Constants.TRANSACTION);
                response.Data = false;
                return response;
            }
            transaction.IsContinued = false; // Soft delete
            transaction.ModifiedAt = DateTime.UtcNow; // Update the transaction date to now
            _unitOfWork.Transactions.Update(transaction);

            int result = await _unitOfWork.CompleteAsync();
            if (result > 0)
            {
                response.Status = ResponseStatus.Success;
                response.Message = MessageHelper.GetSuccessMessageForDeleteOperation(Constants.TRANSACTION);
                response.Data = true;
            }
            else
            {
                response.Status = ResponseStatus.Failed;
                response.Message = MessageHelper.GetErrorMessageForDeleteOperation(Constants.TRANSACTION);
                response.Data = false;
            }
            return response;

        }
        catch (Exception ex)
        {
            throw new Exception(MessageHelper.GetErrorMessageForFeatchOperation(Constants.TRANSACTION_LIST), ex);
        }
    }
}


