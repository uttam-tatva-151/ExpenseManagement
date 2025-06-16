
using System.Security.Claims;
using CashCanvas.Common.ConstantHandler;
using CashCanvas.Common.ExceptionHandler;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CashCanvas.Web.Attributes;

public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        ClaimsPrincipal user = context.HttpContext.User;

        if (user == null || !(user.Identity?.IsAuthenticated ?? false))
        {
            string encryptedData = DataProtectionHelper.Encode($"{StatusCodes.Status401Unauthorized}|{Constants.INVALID_ACCESS_TOKEN}");
            // Redirect to login page if user is not authenticated
            context.Result = new RedirectResult($"/ErrorHandler/HttpStatusCodeHandler/{encryptedData}");
        }
    }
}

