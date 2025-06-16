using CashCanvas.Core.DTOs;

namespace CashCanvas.Services.Interfaces;

public interface IErrorLogService
{
    Task SaveErrorLogAsync(ErrorLogDTO errorLog);
}
