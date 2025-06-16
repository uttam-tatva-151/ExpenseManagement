using CashCanvas.Core.DTOs;
using Npgsql;

namespace CashCanvas.Web.Extensions;

/// <summary>
/// Provides extension methods for registering PostgreSQL composite types.
/// </summary>
public static class PostgreSqlCompositeTypeExtensions
{
    /// <summary>
    /// Registers all PostgreSQL composite types needed for the application.
    /// </summary>
    /// <param name="builder">The NpgsqlDataSourceBuilder instance to register composite types with.</param>
    /// <returns>The updated NpgsqlDataSourceBuilder instance.</returns>
    /// <remarks>
    /// This method maps application-specific composite types to PostgreSQL types.
    /// Ensure that the composite types are properly defined in the database schema.
    /// </remarks>
    public static NpgsqlDataSourceBuilder RegisterAppComposites(this NpgsqlDataSourceBuilder builder)
    {
        builder.MapComposite<ErrorLogDTO>("error_log_dto");
        return builder;
    }
}
