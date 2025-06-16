using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using CashCanvas.Common.ConstantHandler;
using CashCanvas.Core.Beans;
using CashCanvas.Core.Beans.Configuration;
using CashCanvas.Core.Beans.Enums;
using CashCanvas.Core.DTOs;
using CashCanvas.Core.Entities;
using CashCanvas.Core.ViewModel;
using CashCanvas.Data.UnitOfWork;
using CashCanvas.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace CashCanvas.Services.Services;

public class AuthenticationService(IJWTService jwtService, IOptions<EmailSettings> emailSettings,
                                     IConfiguration configuration,
                                     IUnitOfWork unitOfWork) : IAuthenticationService
{
    private readonly IJWTService _jwtService = jwtService;
    private readonly EmailSettings _emailSettings = emailSettings.Value;
    private readonly IConfiguration _configuration = configuration;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ResponseResult<UserDTO>> AuthenticateUserAsync(LoginRequest loginRequest)
    {
        try
        {
            ResponseResult<UserDTO> result = new();
            UserDTO? user = await _unitOfWork.Users.GetFirstOrDefaultAsync(
                                u => u.Email.ToLower() == loginRequest.Email.ToLower() && u.IsActive,
                                u => new UserDTO
                                {
                                    UserId = u.UserId,
                                    UserEmail = u.Email,
                                    UserName = u.UserName,
                                    HashPassword = u.HashPassword,
                                    CreatedAt = u.CreatedAt
                                }
                            );
            if (user == null)
            {
                result.Message = Messages.WARNING_EMAIL_NOT_FOUND;
                result.Status = ResponseStatus.NotFound;
                return result;
            }
            else
            {
                if (BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.HashPassword))
                {
                    user.AccessToken = _jwtService.GenerateAccessToken(user);
                    result.Data = user;
                    result.Message = Messages.SUCCESS_LOGIN;
                    result.Status = ResponseStatus.Success;
                    await UpdateLastLoginAsync(user.UserId);
                }
                else
                {
                    result.Message = Messages.ERROR_PASSWORD_MISMATCH;
                    result.Status = ResponseStatus.Failed;
                }
            }
            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }

    private async Task UpdateLastLoginAsync(Guid userId)
    {
        User? userEntity = await _unitOfWork.Users.GetByIdAsync(userId);
        if (userEntity != null)
        {
            userEntity.LastLoginAt = DateTime.UtcNow;
            _unitOfWork.Users.Update(userEntity);
            await _unitOfWork.CompleteAsync();
        }
    }


    public async Task<ResponseResult<bool>> SendForgotPassLink(string EmailId)
    {
        ResponseResult<bool> result = new();
        try
        {
            bool isUserExist = await _unitOfWork.Users.ExistsAsync(
                u => u.Email.ToLower() == EmailId.ToLower() && u.IsActive
            );
            if (!isUserExist)
            {
                result.Message = Messages.WARNING_EMAIL_NOT_FOUND;
                result.Status = ResponseStatus.NotFound;
            }
            else
            {
                if (await HelperToSend(EmailId))
                {
                    result.Message = Messages.SUCCESS_FORGOT_PASSWORD;
                    result.Status = ResponseStatus.Success;
                }
                else
                {
                    result.Message = Messages.ERROR_FORGOT_PASSWORD;
                    result.Status = ResponseStatus.Failed;
                }
            }
        }
        catch (Exception ex)
        {
            result.Message = ex.Message;
            result.Status = ResponseStatus.Failed;
        }
        return result;
    }
    private async Task<bool> HelperToSend(string emailId)
    {
        string token = await GenerateResetToken(emailId);
        if (string.IsNullOrEmpty(token))
        {
            return false; // Token generation failed
        }

        string resetLink = $"{_configuration[Constants.APP_BASE_URL]}/Authentication/ResetPassword?token={token}";
        string emailBody = await GetEmailBodyAsync(Constants.FORGOT_PASSWORD_FILE);
        emailBody = emailBody.Replace("{{reset_link}}", resetLink);
        string subject = Constants.EMAIL_SUBJECT_FORGOT_PASSWORD;
        return await SendEmailAsync(emailId, subject, emailBody);
    }
    private async Task<string> GenerateResetToken(string emailId)
    {
        string token;
        using (RandomNumberGenerator randomnumbergenerator = RandomNumberGenerator.Create())
        {
            byte[] tokenBytes = new byte[32];
            randomnumbergenerator.GetBytes(tokenBytes);
            token = Convert.ToBase64String(tokenBytes)
                        .Replace("+", "-") // URL-safe Base64
                        .Replace("/", "_")
                        .Replace("=", ""); // Remove padding
        }

        if (token != null)
        {
            bool isSaved = await IsSaveResetToken(emailId, token);
            if (isSaved)
            {
                return token;
            }
            else
            {
                return string.Empty;
            }

        }
        return string.Empty;
    }

    private async Task<bool> IsSaveResetToken(string emailId, string token)
    {
        try
        {
            UserDTO? user = await _unitOfWork.Users.GetFirstOrDefaultAsync(
                                u => u.Email.ToLower() == emailId.ToLower() && u.IsActive,
                                u => new UserDTO
                                {
                                    UserId = u.UserId,
                                    UserEmail = u.Email,
                                    UserName = u.UserName,
                                    HashPassword = u.HashPassword,
                                    CreatedAt = u.CreatedAt
                                }
                            );
            if (user != null)
            {
                PasswordRecoveryToken newToken = new()
                {
                    Token = token,
                    UserId = user.UserId,
                    ExpirationTime = DateTime.UtcNow.AddHours(24),// Token Valid for 24 hours
                    IsUsed = false,
                    CreatedAt = DateTime.UtcNow,
                };
                _unitOfWork.PasswordRecoveryTokens.Update(newToken);
                int affectedRows = await _unitOfWork.CompleteAsync();
                if (affectedRows > 0)
                {
                    return true; // Token saved successfully
                }
                else
                {
                    return false; // Failed to save token
                }
            }
            return false;
        }
        catch
        {
            return false;
        }
    }

    public async Task<string> GetEmailBodyAsync(string templateName)
    {
        string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Templates", templateName);
        return await File.ReadAllTextAsync(templatePath);
    }
    public async Task<bool> SendEmailAsync(string emailId, string subject, string emailBody)
    {

        try
        {
            MailMessage mail = new()
            {
                From = new MailAddress(_emailSettings.SenderEmail),
                Subject = subject,
                Body = emailBody,
                IsBodyHtml = true
            };

            SmtpClient client = new(_emailSettings.SmtpServer)
            {
                Port = _emailSettings.SmtpPort,
                Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.SenderPassword),
                EnableSsl = true,
                UseDefaultCredentials = false
            };

            mail.To.Add(emailId);
            await client.SendMailAsync(mail);

            return true; // Email sent successfully
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Email sending failed: {ex.Message}");
            return false; // Email failed to send
        }
    }

    public async Task<ResponseResult<bool>> ResetPassword(UpdatePasswordViewModel updatePassword)
    {
        ResponseResult<bool> result = new();
        try
        {
            PasswordRecoveryToken? tokenEntity = await _unitOfWork.PasswordRecoveryTokens
                                                                .GetFirstOrDefaultAsync(
                                                                    t => t.Token == updatePassword.Token
                                                                    && t.ExpirationTime > DateTime.UtcNow
                                                                    && !t.IsUsed);
            if (tokenEntity == null)
            {
                result.Message = Messages.WARNING_RESET_TOKEN_EXPIRED;
                result.Status = ResponseStatus.NotFound;
                return result;
            }
            User? userEntity = await _unitOfWork.Users.GetByIdAsync(tokenEntity.UserId);
            if (userEntity == null)
            {
                result.Message = MessageHelper.GetInfoMessageForNoRecordsFound(Constants.USER);
                result.Status = ResponseStatus.Failed;
                return result;
            }
            userEntity.HashPassword = BCrypt.Net.BCrypt.HashPassword(updatePassword.ConfirmPassword);
            tokenEntity.IsUsed = true;
            _unitOfWork.Users.Update(userEntity);
            _unitOfWork.PasswordRecoveryTokens.Update(tokenEntity);
            int affectedRows = await _unitOfWork.CompleteAsync();
            if (affectedRows > 0)
            {
                result.Message = MessageHelper.GetSuccessMessageForUpdateOperation(Constants.PASSWORD);
                result.Status = ResponseStatus.Success;
                result.Data = true;
            }
            else
            {
                result.Message = MessageHelper.GetErrorMessageForUpdateOperation(Constants.PASSWORD);
                result.Status = ResponseStatus.Failed;
                result.Data = false;
            }
            return result;
        }
        catch
        {
            throw;
        }
    }

    public async Task<ResponseResult<bool>> RegisterUserAsync(NewUserViewModel registerRequest)
    {
        try
        {
            ResponseResult<bool> result = new();
            if (await _unitOfWork.Users.ExistsAsync(u => u.Email.ToLower() == registerRequest.EmailId.ToLower()))
            {
                result.Message = Messages.WARNING_EMAIL_ALL_READY_EXISTS;
                result.Status = ResponseStatus.Warning;
                return result;
            }
            User newUser = new()
            {
                UserName = registerRequest.UserName,
                Email = registerRequest.EmailId,
                HashPassword = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password),
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
            await _unitOfWork.Users.AddAsync(newUser);
            int affectedRows = await _unitOfWork.CompleteAsync();
            if (affectedRows > 0)
            {
                result.Message = MessageHelper.GetSuccessMessageForAddOperation(Constants.USER);
                result.Status = ResponseStatus.Success;
                result.Data = true;
            }
            else
            {
                result.Message = MessageHelper.GetErrorMessageForAddOperation(Constants.USER);
                result.Status = ResponseStatus.Failed;
                result.Data = false;
            }
            return result;
        }
        catch (Exception ex)
        {
            throw new Exception(MessageHelper.GetErrorMessageForAddOperation(Constants.USER), ex);
        }
    }
}

