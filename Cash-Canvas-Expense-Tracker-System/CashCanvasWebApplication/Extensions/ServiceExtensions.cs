using CashCanvas.Core.Beans.Events;
using CashCanvas.Services.Interfaces;
using CashCanvas.Services.Jobs;
using CashCanvas.Services.Services;
using CashCanvas.Web.Events;
using MediatR;

namespace CashCanvas.Web.Extensions;

/// <summary>
/// Provides extension methods for registering application services.
/// </summary>
public static class ServiceExtensions
{
    /// <summary>
    /// Registers application services used in the business logic layer.
    /// </summary>
    /// <param name="services">The IServiceCollection instance to add application services to.</param>
    /// <returns>The updated IServiceCollection instance.</returns>
    /// <remarks>
    /// This method registers the following services:
    /// - IErrorLogService with ErrorLogService
    /// - IUserService with UserService
    /// - IJWTService with JWTService
    /// - ILoginService with LoginService
    /// Ensure that these services are properly implemented and follow the application's business logic patterns.
    /// </remarks>
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddHostedService<ReminderNotificationJob>();
        services.AddScoped<IErrorLogService, ErrorLogService>();
        services.AddScoped<IJWTService, JWTService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<IBillService, BillService>();
        services.AddScoped<IBudgetService, BudgetService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<INotificationService, NotificationService>();
        // Register MediatR for all relevant assemblies
        services.AddMediatR(cfg =>
        {
            // Register handlers from any assembly containing these types
            cfg.RegisterServicesFromAssemblyContaining<NotificationCreatedHandler>();
            cfg.RegisterServicesFromAssemblyContaining<NotificationCreatedEvent>();
            cfg.RegisterServicesFromAssemblyContaining<NotificationService>();
        });
        return services;
    }
}