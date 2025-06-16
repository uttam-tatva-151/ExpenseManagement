using CashCanvas.Core.Beans.Enums;

namespace CashCanvas.Core.Beans;

/// <summary>
/// Represents the result of an operation, including its status, message, and optional data.
/// </summary>
/// <typeparam name="T">The type of the data returned by the operation.</typeparam>
public class ResponseResult<T>
{
    /// <summary>
    /// Gets or sets the status of the operation (e.g., Success, Failure).
    /// </summary>
    public ResponseStatus Status { get; set; }

    /// <summary>
    /// Gets or sets a message describing the result of the operation.
    /// </summary>
    public string Message { get; set; } = null!;

    /// <summary>
    /// Gets or sets the data returned by the operation, if applicable.
    /// </summary>
    public T? Data { get; set; }
}
