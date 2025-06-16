using CashCanvas.Core.DTOs;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace CashCanvas.Web.Middlewares;

public class LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<LoggingMiddleware> _logger = logger;

    /// <summary>
    /// Invokes the middleware to log API request and response details.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        DateTime startTime = DateTime.UtcNow;
        await _next(context);
        double durationMs = (DateTime.UtcNow - startTime).TotalMilliseconds;

        LogEntryDTO logEntry = CreateLogEntry(context, durationMs, startTime);
        _logger.LogInformation(FormatLogEntry(logEntry));
    }

    /// <summary>
    /// Creates a structured log entry with all required fields in uppercase.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    /// <param name="durationMs">The request execution time in milliseconds.</param>
    /// <param name="startTime">The request start time.</param>
    /// <returns>An <see cref="LogEntryDTO"/> object.</returns>
    private static LogEntryDTO CreateLogEntry(HttpContext context, double durationMs, DateTime startTime)
    {
        return new LogEntryDTO
        {
            TIMESTAMP = startTime.ToString("yyyy-MM-dd HH:mm:ss"),
            USERNAME = GetUserName(context),
            CONTROLLER = GetControllerName(context),
            ACTION = GetActionName(context),
            METHOD = context.Request.Method.ToUpperInvariant(),
            PATH = context.Request.Path.HasValue ? context.Request.Path.Value.ToUpperInvariant() : string.Empty,
            STATUS = context.Response.StatusCode,
            DURATION_MS = durationMs,
            SUMMARY = BuildSummary(context)
        };
    }

    /// <summary>
    /// Builds a plain-language summary for business and technical users.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    /// <returns>A summary string.</returns>
    private static string BuildSummary(HttpContext context)
    {
        string controller = GetControllerName(context);
        string action = GetActionName(context);
        string method = context.Request.Method.ToUpperInvariant();
        string path = context.Request.Path.HasValue ? context.Request.Path.Value.ToUpperInvariant() : string.Empty;
        int status = context.Response.StatusCode;

        if (status >= 200 && status < 300)
            return $"SUCCESS: {controller}/{action} handled {method} {path}";
        else if (status >= 400 && status < 500)
            return $"CLIENT ERROR: {controller}/{action} returned {status} for {method} {path}";
        else if (status >= 500)
            return $"SERVER ERROR: {controller}/{action} returned {status} for {method} {path}";
        else
            return $"{controller}/{action} processed {method} {path} with status {status}";
    }

    /// <summary>
    /// Extracts the controller name in uppercase or returns "UNKNOWN".
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    /// <returns>The controller name in uppercase.</returns>
    private static string GetControllerName(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        var name = endpoint?.Metadata?.GetMetadata<ControllerActionDescriptor>()?.ControllerName;
        return string.IsNullOrEmpty(name) ? "UNKNOWN" : name.ToUpperInvariant();
    }

    /// <summary>
    /// Extracts the action name in uppercase or returns "UNKNOWN".
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    /// <returns>The action name in uppercase.</returns>
    private static string GetActionName(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        var name = endpoint?.Metadata?.GetMetadata<ControllerActionDescriptor>()?.ActionName;
        return string.IsNullOrEmpty(name) ? "UNKNOWN" : name.ToUpperInvariant();
    }

    /// <summary>
    /// Gets the authenticated user's username or "ANONYMOUS".
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    /// <returns>The user's username in uppercase.</returns>
    private static string GetUserName(HttpContext context)
    {
        var userName = context.User?.Identity?.IsAuthenticated == true
            ? context.User.Identity.Name
            : "ANONYMOUS";
        return string.IsNullOrWhiteSpace(userName) ? "ANONYMOUS" : userName.ToUpperInvariant();
    }

    /// <summary>
    /// Formats a log entry into a readable multi-line string for logging.
    /// </summary>
    /// <param name="entry">The log entry to format.</param>
    /// <returns>A formatted log string.</returns>
    private static string FormatLogEntry(LogEntryDTO entry)
    {
        return $@"
======== Backend REQUEST LOG ========
TIMESTAMP   : {entry.TIMESTAMP}
USERNAME    : {entry.USERNAME}
CONTROLLER  : {entry.CONTROLLER}
ACTION      : {entry.ACTION}
METHOD      : {entry.METHOD}
PATH        : {entry.PATH}
STATUS      : {entry.STATUS}
DURATION_MS : {entry.DURATION_MS:F2}
SUMMARY     : {entry.SUMMARY}
===============================";
    }
}
