using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashCanvas.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateErrorLogCompositeType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE TYPE error_log_dto AS (
                ErrorMessage TEXT,
                StackTrace TEXT,
                ExceptionType TEXT,
                StatusCode TEXT,
                ControllerName TEXT,
                ActionName TEXT
            );
        ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TYPE IF EXISTS error_log_dto;");
        }
    }
}
