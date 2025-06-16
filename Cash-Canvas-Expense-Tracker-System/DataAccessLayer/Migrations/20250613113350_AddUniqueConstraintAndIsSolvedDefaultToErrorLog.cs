using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashCanvas.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraintAndIsSolvedDefaultToErrorLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsSolved",
                table: "ErrorLog",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.CreateIndex(
                name: "unique_error_pattern",
                table: "ErrorLog",
                columns: new[] { "ExceptionType", "StatusCode", "ControllerName", "ActionName" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "unique_error_pattern",
                table: "ErrorLog");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSolved",
                table: "ErrorLog",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);
        }
    }
}
