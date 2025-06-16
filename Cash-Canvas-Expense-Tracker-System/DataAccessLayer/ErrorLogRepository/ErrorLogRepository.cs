using System.Data.Common;
using CashCanvas.Core.DTOs;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace CashCanvas.Data.ErrorLogRepository;

public class ErrorLogRepository(NpgsqlDataSource dataSource) : IErrorLogRepository
{
    private readonly NpgsqlDataSource _dataSource = dataSource;
    public async Task UpsertErrorLogAsync(ErrorLogDTO errorLog)
    {
        try
        {

            await using var connection = await _dataSource.OpenConnectionAsync();
            // NpgsqlConnection npgsqlConn = (NpgsqlConnection)connection;
            using NpgsqlCommand cmd = new("SELECT upsert_error_log(@in_log)", connection);
            NpgsqlParameter param = cmd.Parameters.AddWithValue("in_log", errorLog);
            param.DataTypeName = "error_log_dto";
            await cmd.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while upserting error log: {ex.Message}");
        }
    }

}
