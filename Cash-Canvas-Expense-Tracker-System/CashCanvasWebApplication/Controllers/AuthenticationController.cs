using CashCanvas.Common.ConstantHandler;
using CashCanvas.Common.ExceptionHandler;
using CashCanvas.Common.HttpStateHandlers;
using CashCanvas.Common.TosterHandlers;
using CashCanvas.Core.Beans;
using CashCanvas.Core.Beans.Enums;
using CashCanvas.Core.DTOs;
using CashCanvas.Core.ViewModel;
using CashCanvas.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashCanvas.Web.Controllers;

public class AuthenticationController(IAuthenticationService authService, IJWTService jwtService) : Controller
{
    private readonly IAuthenticationService _authService = authService;
    private readonly IJWTService _jwtService = jwtService;
   
   [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        try
        {
            string userToken = CookieHelper.GetCookieValue(Request, Constants.REFRESH_TOKEN) ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(userToken))
            {
                UserDTO user = await _jwtService.ValidateRefreshTokenAndGenerateAccessTokenAsync(userToken);
                SetAuthData(Response, HttpContext, user.AccessToken, user.UserEmail, user.UserName);
                return RedirectToAction(Constants.DASHBOARD_VIEW, Constants.HOME_CONTROLLER);
            }
        }
        catch (Exception)
        {
            throw;
        }

        TempData[Constants.LAYOUT_VARIABLE_NAME] = Constants.LOGIN_LAYOUT;
        return View();
    }
    
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        ResponseResult<UserDTO> result = new();
        try
        {
            result = await _authService.AuthenticateUserAsync(loginRequest);
            if (result.Status == ResponseStatus.Success && result.Data is UserDTO user)
            {
                string accessToken = _jwtService.GenerateAccessToken(result.Data);
                if (loginRequest.IsRememberMe)
                {
                    // If we running locally, we'll often see 127.0.0.1 or ::1. That's normal.
                    string createdByIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? Constants.UNKNOWN_IP;
                    string refreshToken = _jwtService.GenerateRefreshToken();
                    await _jwtService.SaveRefreshTokenAsync(user.UserId, refreshToken, createdByIp);
                    CookieHelper.AppendCookie(Response, Constants.REFRESH_TOKEN, refreshToken, 30 * 24 * 60); // 1 month
                }

                SetAuthData(Response, HttpContext, accessToken, user.UserEmail, user.UserName);

                ToasterHelper.SetToastMessage(TempData, result.Message, result.Status);
                return RedirectToAction(Constants.DASHBOARD_VIEW, Constants.HOME_CONTROLLER);
            }
        }
        catch (Exception ex)
        {
            result.Status = ResponseStatus.Failed;
            result.Message = ExceptionHelper.GetErrorMessage(ex);
        }

        ToasterHelper.SetToastMessage(TempData, result.Message, result.Status);
        return RedirectToAction(Constants.LOGIN_VIEW, Constants.LOGIN_CONTROLLER);
    }
    public IActionResult ForgotPassword()
    {
        TempData[Constants.LAYOUT_VARIABLE_NAME] = Constants.LOGIN_LAYOUT;
        return View();
    }
    public IActionResult Register()
    {
        TempData[Constants.LAYOUT_VARIABLE_NAME] = Constants.LOGIN_LAYOUT;
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterUser(NewUserViewModel registerRequest)
    {
        ResponseResult<bool> result = new();
        try
        {
            result = await _authService.RegisterUserAsync(registerRequest);
        }
        catch (Exception ex)
        {
            result.Status = ResponseStatus.Failed;
            result.Message = ExceptionHelper.GetErrorMessage(ex);
        }
        ToasterHelper.SetToastMessage(TempData, result.Message, result.Status);
        return RedirectToAction(Constants.LOGIN_VIEW, Constants.LOGIN_CONTROLLER);
    }


    [HttpPost]
    [AllowAnonymous]

    public async Task<IActionResult> SendEmailLink(string email)
    {
        ResponseResult<bool> result = new();
        try
        {
            result = await _authService.SendForgotPassLink(email);
        }
        catch (Exception ex)
        {
            result.Status = ResponseStatus.Failed;
            result.Message = ExceptionHelper.GetErrorMessage(ex);
        }
        ToasterHelper.SetToastMessage(TempData, result.Message, result.Status);
        return RedirectToAction(Constants.LOGIN_VIEW, Constants.LOGIN_CONTROLLER);
    }
    public IActionResult ResetPassword(string token)
    {
        TempData[Constants.LAYOUT_VARIABLE_NAME] = Constants.LOGIN_LAYOUT;
        ViewBag.Token = token;
        return View();
    }

    [HttpPost]
       [AllowAnonymous]

    public async Task<IActionResult> ResetPassword(UpdatePasswordViewModel updatePassword)
    {
        ResponseResult<bool> result = new();
        try
        {
            result = await _authService.ResetPassword(updatePassword);
        }
        catch (Exception ex)
        {
            result.Status = ResponseStatus.Failed;
            result.Message = ExceptionHelper.GetErrorMessage(ex);
        }
        ToasterHelper.SetToastMessage(TempData, result.Message, result.Status);
        return RedirectToAction(Constants.LOGIN_VIEW, Constants.LOGIN_CONTROLLER);
    }

    public IActionResult Logout()
    {
        if (Request.Cookies.TryGetValue(Constants.REFRESH_TOKEN, out string? refreshToken) && refreshToken != null)
        {
            string revokedByIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown IP";
            _jwtService.RevokeRefreshTokenAsync(refreshToken, revokedByIp);
        }

        Response.Cookies.Delete(Constants.ACCESS_TOKEN);
        Response.Cookies.Delete(Constants.REFRESH_TOKEN);
        ToasterHelper.SetToastMessage(TempData, Messages.SUCCESS_LOGOUT, ResponseStatus.Success);
        return RedirectToAction(Constants.LOGIN_VIEW, Constants.LOGIN_CONTROLLER);
    }
    private static void SetAuthData(HttpResponse response, HttpContext context, string accessToken, string email, string userName)
    {
        // Set access token cookie
        CookieHelper.AppendCookie(response, Constants.ACCESS_TOKEN, accessToken, 15);

        // Set session values
        // SessionHelper.SetString(context, Constants.SESSION_EMAIL, email);
        // SessionHelper.SetString(context, Constants.SESSION_USERNAME, userName);
    }
}