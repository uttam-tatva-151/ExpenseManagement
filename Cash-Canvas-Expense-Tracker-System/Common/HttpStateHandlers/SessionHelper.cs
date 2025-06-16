using CashCanvas.Common.ConstantHandler;
using Microsoft.AspNetCore.Http;

namespace CashCanvas.Common.HttpStateHandlers;

public class SessionHelper
{/// <summary>
 /// Sets a string value in the session with the specified key.
 /// </summary>
 /// <param name="httpContext">The HTTP context object.</param>
 /// <param name="key">The key for the session value.</param>
 /// <param name="value">The value to store in the session.</param>
    public static void SetString(HttpContext httpContext, string key, string value)
    {
        if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException(MessageHelper.GetNotFoundMessage(Constants.SESSION_KEY), nameof(key));

        httpContext.Session.SetString(key, value);
    }

    /// <summary>
    /// Retrieves a string value from the session using the specified key.
    /// </summary>
    /// <param name="httpContext">The HTTP context object.</param>
    /// <param name="key">The key for the session value.</param>
    /// <returns>The value from the session, or null if the key does not exist.</returns>
    public static string? GetString(HttpContext httpContext, string key)
    {
        if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException(MessageHelper.GetNotFoundMessage(Constants.SESSION_KEY), nameof(key));

        return httpContext.Session.GetString(key);
    }

    /// <summary>
    /// Removes a value from the session using the specified key.
    /// </summary>
    /// <param name="httpContext">The HTTP context object.</param>
    /// <param name="key">The key for the session value to remove.</param>
    public static void Remove(HttpContext httpContext, string key)
    {
        if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException(MessageHelper.GetNotFoundMessage(Constants.SESSION_KEY), nameof(key));

        httpContext.Session.Remove(key);
    }

    /// <summary>
    /// Checks if a key exists in the session.
    /// </summary>
    /// <param name="httpContext">The HTTP context object.</param>
    /// <param name="key">The key to check in the session.</param>
    /// <returns>True if the key exists, otherwise false.</returns>
    public static bool Contains(HttpContext httpContext, string key)
    {
        if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException(MessageHelper.GetNotFoundMessage(Constants.SESSION_KEY), nameof(key));

        return httpContext.Session.GetString(key) != null;
    }

}
