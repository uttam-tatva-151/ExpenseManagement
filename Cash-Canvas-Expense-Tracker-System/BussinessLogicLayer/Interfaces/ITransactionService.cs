using CashCanvas.Core.Beans;
using CashCanvas.Core.DTOs;
using CashCanvas.Core.ViewModel;

namespace CashCanvas.Services.Interfaces;

public interface ITransactionService
{
    Task<List<TransactionViewModel>> GetTransactionListAsync(Guid UserId);
    Task<TransactionViewModel> GetTransactionByIdAsync(Guid transactionId);
    Task<ResponseResult<List<CategoryViewModel>>> AddTransactionAsync(TransactionViewModel transactionViewModel);
    Task<ResponseResult<bool>> UpdateTransactionAsync(TransactionViewModel transactionViewModel);
    Task<ResponseResult<bool>> DeleteTransactionAsync(Guid transactionId);
    Task<List<CategoryViewModel>> GetCategoryListAsync(Guid userId);
    Task<List<TransactionExportViewModel>> GetExortTransactionAsync(Guid userId);
    Task<byte[]> ExportTransactionsToExcel(Guid userID);
    Task<byte[]> ExportTransactionsToCsv(Guid userID);
    Task<ResponseResult<bool>> ImportTransactionsFromCsvAsync(Stream csvStream, Guid userId);
}
