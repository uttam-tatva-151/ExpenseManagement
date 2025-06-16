using System.Security.Claims;
using CashCanvas.Core.Beans;
using CashCanvas.Core.DTOs;

namespace CashCanvas.Services.Interfaces;

public interface IJWTService
{
    string GenerateAccessToken(UserDTO user);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    Task SaveRefreshTokenAsync(Guid userId, string refreshToken, string createdByIp);
    Task<bool> ValidateRefreshTokenAsync(string refreshToken);
    Task<UserDTO> ValidateRefreshTokenAndGenerateAccessTokenAsync(string refreshToken);
    Task<string> GenerateAccessTokenByRefreshToken(string refreshToken);
    Task<bool> RevokeRefreshTokenAsync(string refreshToken, string revokedByIp);
}
