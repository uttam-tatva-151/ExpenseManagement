using CashCanvas.Common.ConstantHandler;
using CashCanvas.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace CashCanvas.Web.Extensions;

/// <summary>
/// Provides extension methods for configuring database-related services.
/// </summary>
public static class DatabaseExtensions
{
    /// <summary>
    /// Configures the application's database services, including PostgreSQL connection and DbContext.
    /// </summary>
    /// <param name="services">The IServiceCollection instance to add database services to.</param>
    /// <param name="configuration">The IConfiguration instance to retrieve connection string settings from.</param>
    /// <returns>The updated IServiceCollection instance.</returns>
    /// <remarks>
    /// This method performs the following:
    /// - Retrieves the default database connection string from the configuration.
    /// - Registers PostgreSQL composite types using NpgsqlDataSourceBuilder.
    /// - Configures the application's DbContext to use PostgreSQL.
    /// Ensure that the connection string is properly defined in the appsettings.json file.
    /// </remarks>
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        string? databaseConnectionString = configuration.GetConnectionString(Constants.DATABASE_DEFAULT_CONNECTION);
        if (string.IsNullOrWhiteSpace(databaseConnectionString))
            throw new InvalidOperationException(Messages.ERROR_MISSING_DB_CONNECTION);

        NpgsqlDataSourceBuilder dataSourceBuilder = new(databaseConnectionString);
        dataSourceBuilder.RegisterAppComposites();
        NpgsqlDataSource dataSource = dataSourceBuilder.Build();

        services.AddSingleton<NpgsqlDataSource>(dataSource);
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(databaseConnectionString));

        return services;
    }
}