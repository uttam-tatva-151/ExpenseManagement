
using Microsoft.AspNetCore.Mvc.Controllers;
using CashCanvas.Core.DTOs;
using CashCanvas.Common.ExceptionHandler;
using CashCanvas.Services.Interfaces;
using CashCanvas.Common.ConstantHandler;
using System.Text.Json;
using System.Text;

namespace CashCanvas.Web.Middlewares
{
    /// <summary>
    /// Middleware for handling exceptions globally in the application.
    /// Logs the error and renders a user-friendly error page with status code and a short message.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ExceptionMiddleware"/> class.
    /// </remarks>
    /// <param name="next">The next middleware in the pipeline.</param>
    public class ExceptionMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        private readonly RequestDelegate _next = next ?? throw new ArgumentNullException(nameof(next));
        private readonly string _filePath = configuration.GetSection(Constants.LOGGING_PATH_URL).Value ?? throw new InvalidOperationException("Logging:PathUrl is not set in configuration.");

        /// <summary>
        /// Invokes the middleware to handle exceptions.
        /// </summary>
        /// <param name="context">The current HTTP context.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await RenderCustomErrorViewAsync(context, ex);
            }
        }

        /// <summary>
        /// Retrieves the name of the controller handling the current request.
        /// </summary>
        /// <param name="context">The current HTTP context.</param>
        /// <returns>The name of the controller, or "Unknown" if not available.</returns>
        private static string GetControllerName(HttpContext context)
        {
            Endpoint? endpoint = context.GetEndpoint();
            return endpoint?.Metadata?.GetMetadata<ControllerActionDescriptor>()?.ControllerName ?? Constants.UNKNOWN_CONTROLLER_NAME;
        }

        /// <summary>
        /// Retrieves the name of the action handling the current request.
        /// </summary>
        /// <param name="context">The current HTTP context.</param>
        /// <returns>The name of the action, or "Unknown" if not available.</returns>
        private static string GetActionName(HttpContext context)
        {
            Endpoint? endpoint = context.GetEndpoint();
            return endpoint?.Metadata?.GetMetadata<ControllerActionDescriptor>()?.ActionName ?? Constants.UNKNOWN_ACTION_NAME;
        }

        /// <summary>
        /// Handles exceptions and prepares a response for the client.
        /// </summary>
        /// <param name="context">The current HTTP context.</param>
        /// <param name="exception">The exception to handle.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task RenderCustomErrorViewAsync(HttpContext context, Exception exception)
        {
            IErrorLogService errorLogService = context.RequestServices.GetRequiredService<IErrorLogService>();
            (string ShortErrorMessage, int StatusCode) = ExceptionHelper.GetErrorShortModel(exception);

            ErrorLogDTO errorDetails = new()
            {
                ErrorMessage = exception.Message,
                StackTrace = exception.StackTrace,
                ExceptionType = exception.GetType().Name,
                ControllerName = GetControllerName(context),
                ActionName = GetActionName(context),
                StatusCode = StatusCode.ToString()
            };

            // Save error details to the error log database 
            await errorLogService.SaveErrorLogAsync(errorDetails);
            string encryptedData = DataProtectionHelper.Encode($"{StatusCode}|{ShortErrorMessage}");
            await LogErrorToFileAsync(errorDetails);
            // Prepare the response
            context.Response.Clear();
            context.Response.StatusCode = StatusCode;
            context.Response.Redirect($"/ErrorHandler/HttpStatusCodeHandler/{encryptedData}");
        }

        private async Task LogErrorToFileAsync(ErrorLogDTO errorLog)
        {
            string json = JsonSerializer.Serialize(errorLog, new JsonSerializerOptions
            {
                WriteIndented = true // pretty print
            });
            // Optionally add timestamp and separator
            string logEntry = FormatErrorLog(errorLog);

            await File.AppendAllTextAsync(_filePath, logEntry);
        }
        public static string FormatErrorLog(ErrorLogDTO errorLog)
        {
            StringBuilder builder = new ();

            builder.AppendLine("┌─────────────────────────────── ERROR LOG ───────────────────────────────┐");
            builder.AppendLine($"│ Timestamp       : {DateTime.UtcNow:yyyy-MM-ddTHH:mm:ss.fffZ}".PadRight(75) + "│");
            builder.AppendLine($"│ Status Code     : {errorLog.StatusCode}".PadRight(75) + "│");
            builder.AppendLine($"│ Exception Type  : {errorLog.ExceptionType}".PadRight(75) + "│");
            builder.AppendLine($"│ Controller Name : {errorLog.ControllerName ?? "N/A"}".PadRight(75) + "│");
            builder.AppendLine($"│ Action Name     : {errorLog.ActionName ?? "N/A"}".PadRight(75) + "│");
            builder.AppendLine($"│ Error Message   : {errorLog.ErrorMessage}".PadRight(75) + "│");

            builder.AppendLine("├────────────────────────────── Stack Trace ─────────────────────────────┤");

            if (!string.IsNullOrWhiteSpace(errorLog.StackTrace))
            {
                foreach (string line in errorLog.StackTrace.Split('\n', StringSplitOptions.RemoveEmptyEntries))
                {
                    builder.AppendLine($"│ {line.Trim()}".PadRight(75) + "│");
                }
            }
            else
            {
                builder.AppendLine("│ N/A".PadRight(75) + "│");
            }

            builder.AppendLine("└────────────────────────────────────────────────────────────────────────┘");
            builder.AppendLine();

            return builder.ToString();
        }
    }
}

