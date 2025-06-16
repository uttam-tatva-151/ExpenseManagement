using CashCanvas.Common.ConstantHandler;
using Microsoft.AspNetCore.Http;

namespace CashCanvas.Common.HttpStateHandlers;

public class CookieHelper
{
    /// <summary>
        /// Appends a cookie to the HTTP response with the specified key, value, and expiration time.
        /// </summary>
        /// <param name="response">The HTTP response object.</param>
        /// <param name="key">The key for the cookie.</param>
        /// <param name="value">The value for the cookie.</param>
        /// <param name="expirationMinutes">The expiration time in minutes.</param>
        public static void AppendCookie(HttpResponse response, string key, string value, int expirationMinutes)
        {
            if (response == null) throw new ArgumentNullException(nameof(response));
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException(MessageHelper.GetNotFoundMessage(Constants.COOKIE_KEY), nameof(key));
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException(MessageHelper.GetNotFoundMessage(Constants.COOKIE_VALUE), nameof(value));

            response.Cookies.Append(key, value, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(expirationMinutes)
            });
        }

        /// <summary>
        /// Retrieves the value of a cookie from the HTTP request.
        /// </summary>
        /// <param name="request">The HTTP request object.</param>
        /// <param name="cookieName">The name of the cookie.</param>
        /// <returns>The value of the cookie, or null if the cookie does not exist.</returns>
        public static string? GetCookieValue(HttpRequest request, string cookieName)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrWhiteSpace(cookieName)) throw new ArgumentException(MessageHelper.GetNotFoundMessage(Constants.COOKIE_NAME), nameof(cookieName));

            return request.Cookies.TryGetValue(cookieName, out string? cookieValue) ? cookieValue : null;
        }
}
