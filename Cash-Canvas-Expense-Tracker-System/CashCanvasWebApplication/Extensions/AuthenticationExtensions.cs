using System.Text;
using CashCanvas.Common.ConstantHandler;
using CashCanvas.Core.Beans.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace CashCanvas.Web.Extensions;
/// <summary>
/// Provides extension methods for configuring JWT authentication and authorization.
/// </summary>
public static class AuthenticationExtensions
{
    /// <summary>
    /// Configures JWT authentication and authorization for the application.
    /// </summary>
    /// <param name="services">The IServiceCollection instance to add authentication services to.</param>
    /// <param name="configuration">The IConfiguration instance to retrieve JWT settings from.</param>
    /// <returns>The updated IServiceCollection instance.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the JWT configuration section is missing or contains invalid values.
    /// </exception>
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        JwtConfig? jwtConfig = configuration.GetSection(Constants.JWT_CONFIG).Get<JwtConfig>() 
            ?? throw new InvalidOperationException(Messages.ERROR_MISSING_JWT_SECTION);

        if (string.IsNullOrWhiteSpace(jwtConfig.Key) || string.IsNullOrWhiteSpace(jwtConfig.Issuer) || string.IsNullOrWhiteSpace(jwtConfig.Audience))
            throw new InvalidOperationException(Messages.ERROR_INVALID_JWT_VALUES);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtConfig.Issuer,
                ValidAudience = jwtConfig.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key))
            };
        });

        services.AddAuthorization();
        return services;
    }
}