using NpgsqlTypes;

namespace CashCanvas.Core.DTOs;

[PgName("error_log_dto")]
public class ErrorLogDTO
{
    [PgName("ErrorMessage")]
    public string ErrorMessage { get; set; } = string.Empty;

    [PgName("StackTrace")]
    public string? StackTrace { get; set; }

    [PgName("ExceptionType")]
    public string ExceptionType { get; set; } = string.Empty;

    [PgName("StatusCode")]
    public string StatusCode { get; set; } = string.Empty;

    [PgName("ControllerName")]
    public string? ControllerName { get; set; }

    [PgName("ActionName")]
    public string? ActionName { get; set; }
}
