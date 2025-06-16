using CashCanvas.Core.DTOs;

namespace CashCanvas.Data.ErrorLogRepository;

public interface IErrorLogRepository
{
    Task UpsertErrorLogAsync(ErrorLogDTO errorLog);

}
