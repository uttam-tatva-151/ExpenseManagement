using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CashCanvas.Common.ConstantHandler;
using CashCanvas.Core.Beans.Configuration;
using CashCanvas.Core.DTOs;
using CashCanvas.Core.Entities;
using CashCanvas.Data.UnitOfWork;
using CashCanvas.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CashCanvas.Services.Services;

public class JWTService(IOptions<JwtConfig> jwtConfig, IUnitOfWork unitOfWork) : IJWTService
{
    private readonly JwtConfig _jwtConfig = jwtConfig.Value;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;


    public string GenerateAccessToken(UserDTO user)
    {
        if (string.IsNullOrWhiteSpace(_jwtConfig.Key))
            throw new InvalidOperationException(MessageHelper.GetNotFoundMessage(Constants.JWT_KEY));

        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_jwtConfig.Key ?? string.Empty));
        SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);
        Claim[] claims =
            [
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.UserEmail),
                new Claim("AccountCreatedOn", user.CreatedAt.ToString("dd MMM''yy"))
            ];

        JwtSecurityToken token = new(
            _jwtConfig.Issuer,
            _jwtConfig.Audience,
            claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        byte[] randomBytes = new byte[64]; // 64 bytes = 512 bits
        using (RandomNumberGenerator generator = RandomNumberGenerator.Create())
        {
            generator.GetBytes(randomBytes);
        }
        return Convert.ToBase64String(randomBytes);
    }
    public async Task<string> GenerateAccessTokenByRefreshToken(string refreshToken){
        UserDTO? user = await _unitOfWork.CustomUserRepository.GetActiveUserByRefreshTokenAsync(refreshToken);
        return user != null ? GenerateAccessToken(user) : throw new UnauthorizedAccessException();
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException(MessageHelper.GetNotFoundMessage(Constants.ACCESS_TOKEN), nameof(token));

            JwtSecurityTokenHandler tokenHandler = new();
            byte[] key = Encoding.UTF8.GetBytes(_jwtConfig.Key ?? string.Empty);

            try
            {
                TokenValidationParameters validationParameters = new()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidIssuer = _jwtConfig.Issuer,
                    ValidateAudience = false,
                    ValidAudience = _jwtConfig.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero // Strict expiration check
                };


                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                return principal;
            }
            catch
            {
                return null ?? new ClaimsPrincipal();
            }
        }
    }
    public async Task<bool> ValidateRefreshTokenAsync(string refreshToken)
    {
        return await _unitOfWork.CustomUserRepository.GetActiveUserByRefreshTokenAsync(refreshToken) != null;
    }

    public async Task<UserDTO> ValidateRefreshTokenAndGenerateAccessTokenAsync(string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            throw new ArgumentException(MessageHelper.GetNotFoundMessage(Constants.REFRESH_TOKEN), nameof(refreshToken));

        UserDTO? user = await _unitOfWork.CustomUserRepository.GetActiveUserByRefreshTokenAsync(refreshToken);
        if (user == null)
            throw new UnauthorizedAccessException(MessageHelper.GetNotFoundMessage(Constants.INVALID_REFRESH_TOKEN));

        string accessToken = GenerateAccessToken(user);
        user.AccessToken = accessToken;

        return user;
    }


    public async Task SaveRefreshTokenAsync(Guid userId, string refreshToken, string createdByIp)
    {
        try
        {
            RefreshToken token = new()
            {
                UserId = userId,
                Token = refreshToken,
                CreatedAt = DateTime.UtcNow,
                CreatedByIp = createdByIp,
                IsActive = true,
                IsRevoked = false,
                ExpirationTime = DateTime.UtcNow.AddDays(30)
            };
            await _unitOfWork.RefreshTokens.AddAsync(token);
            await _unitOfWork.CompleteAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> RevokeRefreshTokenAsync(string refreshToken, string revokedByIp)
    {
        try
        {
            // Find the refresh token in the database
            RefreshToken? token = await _unitOfWork.RefreshTokens.GetFirstOrDefaultAsync(
                                                            u => u.Token == refreshToken && 
                                                                 u.IsActive && 
                                                                !u.IsRevoked);
            if (token == null)
            {
                return false;
            }

            // Update the token's revocation details
            token.IsRevoked = true;
            token.RevokedAt = DateTime.Now;
            token.RevokedByIp = revokedByIp;
            token.IsActive = false;

            // Save changes to the database
            _unitOfWork.RefreshTokens.Update(token);
            await _unitOfWork.CompleteAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

}
