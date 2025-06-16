using System.Security.Claims;
using CashCanvas.Common.ConstantHandler;
using CashCanvas.Common.ExceptionHandler;
using CashCanvas.Common.HttpStateHandlers;
using CashCanvas.Core.Beans.Configuration;
using CashCanvas.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CashCanvas.Web.Middlewares;

public class JWTMiddleware(RequestDelegate next, IOptions<JwtConfig> jwtConfig)
{
    private readonly RequestDelegate _next = next;
    private readonly JwtConfig _jwtConfig = jwtConfig.Value;


    public async Task Invoke(HttpContext context)
    {
        try
        {
            IJWTService jwtService = context.RequestServices.GetRequiredService<IJWTService>();
            string token = context.Request.Cookies[Constants.ACCESS_TOKEN] ?? string.Empty;
            string refreshToken = context.Request.Cookies[Constants.REFRESH_TOKEN] ?? string.Empty;

            if (!string.IsNullOrEmpty(token))
            {
                ClaimsPrincipal principal = jwtService.GetPrincipalFromExpiredToken(token) ?? throw new SecurityTokenException(Constants.INVALID_ACCESS_TOKEN);

                context.User = principal;
            }
            else if (!string.IsNullOrEmpty(refreshToken))
            {
                bool isRefreshTokenValid = await jwtService.ValidateRefreshTokenAsync(refreshToken);
                if (isRefreshTokenValid)
                {
                    string newAccessToken = await jwtService.GenerateAccessTokenByRefreshToken(refreshToken);

                    if (!string.IsNullOrEmpty(refreshToken) && !string.IsNullOrEmpty(newAccessToken))
                    {
                        CookieHelper.AppendCookie(context.Response, Constants.ACCESS_TOKEN, newAccessToken, 15); // Access token expires in 15 minutes
                        CookieHelper.AppendCookie(context.Response, Constants.REFRESH_TOKEN, refreshToken, 30 * 24 * 60); // Refresh token expires in 30 days

                        ClaimsPrincipal principal = jwtService.GetPrincipalFromExpiredToken(newAccessToken) ?? throw new SecurityTokenException(Constants.INVALID_ACCESS_TOKEN);
                        if (principal != null)
                        {
                            context.User = principal;
                        }
                    }
                }
                else
                {
                    context.Response.Cookies.Delete(Constants.ACCESS_TOKEN);
                    context.Response.Cookies.Delete(Constants.REFRESH_TOKEN);
                    context.Response.Redirect("/" + Constants.LOGIN_CONTROLLER + "/" + Constants.LOGIN_VIEW);
                    return;
                }
            }

        }
        catch (Exception ex)
        {

            context.Response.Cookies.Delete(Constants.ACCESS_TOKEN);
            context.Response.Cookies.Delete(Constants.REFRESH_TOKEN);


            int StatusCode = ex is SecurityTokenException ? StatusCodes.Status401Unauthorized : StatusCodes.Status500InternalServerError;
            string ShortErrorMessage = ex is SecurityTokenException ? Constants.INVALID_ACCESS_TOKEN : string.Empty;

            string encryptedData = DataProtectionHelper.Encode($"{StatusCode}|{ShortErrorMessage}");

            context.Response.Redirect($"/ErrorHandler/HttpStatusCodeHandler/{encryptedData}");
            return;
        }

        await _next(context);
    }

}
public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
        Claim? userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        return userIdClaim != null && Guid.TryParse(userIdClaim.Value, out Guid userId) ? userId : Guid.Empty;
    }
    public static string GetUserName(this ClaimsPrincipal user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
        Claim? userNameClaim = user.FindFirst(ClaimTypes.Name);
        return userNameClaim?.Value ?? string.Empty;
    }

    public static string GetAccountCreatedOn(this ClaimsPrincipal user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
        Claim? createdOnClaim = user.FindFirst("AccountCreatedOn");
        return createdOnClaim?.Value ?? string.Empty;
    }
}
