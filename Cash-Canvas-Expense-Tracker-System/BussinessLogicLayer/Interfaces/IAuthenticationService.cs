using CashCanvas.Core.Beans;
using CashCanvas.Core.DTOs;
using CashCanvas.Core.ViewModel;

namespace CashCanvas.Services.Interfaces;

public interface IAuthenticationService
{
    Task<ResponseResult<UserDTO>> AuthenticateUserAsync(LoginRequest loginRequest);
    Task<ResponseResult<bool>> RegisterUserAsync(NewUserViewModel registerRequest);
    Task<ResponseResult<bool>> ResetPassword(UpdatePasswordViewModel updatePassword);

    Task<ResponseResult<bool>> SendForgotPassLink(string email);
}
