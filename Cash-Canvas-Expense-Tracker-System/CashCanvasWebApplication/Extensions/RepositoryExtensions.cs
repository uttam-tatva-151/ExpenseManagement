using CashCanvas.Data.BaseRepository;
using CashCanvas.Data.UnitOfWork;
using CashCanvas.Data.ErrorLogRepository;
using CashCanvas.Data.UserRepository;
using CashCanvas.Data.BillRepository;

namespace CashCanvas.Web.Extensions;
/// <summary>
/// Provides extension methods for registering repository services.
/// </summary>
public static class RepositoryExtensions
{
    /// <summary>
    /// Registers repository services used in the application.
    /// </summary>
    /// <param name="services">The IServiceCollection instance to add repository services to.</param>
    /// <returns>The updated IServiceCollection instance.</returns>
    /// <remarks>
    /// This method registers the following repositories:
    /// - IErrorLogRepository with ErrorLogRepository
    /// - IUserRepository with UserRepository
    /// Ensure that these repositories are properly implemented and follow the application's data access patterns.
    /// </remarks>
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IErrorLogRepository, ErrorLogRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IBillRepository, BillRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        return services;
    }
}