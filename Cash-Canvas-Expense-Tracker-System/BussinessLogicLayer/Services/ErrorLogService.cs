using CashCanvas.Common.ConstantHandler;
using CashCanvas.Core.DTOs;
using CashCanvas.Data.ErrorLogRepository;

using CashCanvas.Services.Interfaces;

namespace CashCanvas.Services.Services;

public class ErrorLogService : IErrorLogService
{
    private readonly IErrorLogRepository _errorLogRepository;
    public ErrorLogService(IErrorLogRepository errorLogRepository)
    {
        _errorLogRepository = errorLogRepository;
    }
    public async Task SaveErrorLogAsync(ErrorLogDTO errorLog)
    {
        if (string.IsNullOrWhiteSpace(errorLog.ExceptionType) ||
            string.IsNullOrWhiteSpace(errorLog.StatusCode) ||
            (errorLog == null))
        {
            throw new ArgumentException(Messages.EXCEPTION_TYPE_AND_STATUS_CODE_REQUIRED);
        }
        await _errorLogRepository.UpsertErrorLogAsync(errorLog);
    }
}
