using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashCanvas.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFieldInBillTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "Bills");

            migrationBuilder.AddColumn<DateTime>(
                name: "NextDueDate",
                table: "Bills",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000001"),
                column: "NextDueDate",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000002"),
                column: "NextDueDate",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000003"),
                column: "NextDueDate",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000004"),
                column: "NextDueDate",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000005"),
                column: "NextDueDate",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000006"),
                column: "NextDueDate",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000007"),
                column: "NextDueDate",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000008"),
                column: "NextDueDate",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000009"),
                column: "NextDueDate",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000010"),
                column: "NextDueDate",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000011"),
                column: "NextDueDate",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000012"),
                column: "NextDueDate",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000013"),
                column: "NextDueDate",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000014"),
                column: "NextDueDate",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000015"),
                column: "NextDueDate",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000016"),
                column: "NextDueDate",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000017"),
                column: "NextDueDate",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000018"),
                column: "NextDueDate",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000019"),
                column: "NextDueDate",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000020"),
                column: "NextDueDate",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000021"),
                column: "NextDueDate",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000022"),
                column: "NextDueDate",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000023"),
                column: "NextDueDate",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000024"),
                column: "NextDueDate",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000025"),
                column: "NextDueDate",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NextDueDate",
                table: "Bills");

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "Bills",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000001"),
                column: "IsPaid",
                value: false);

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000002"),
                column: "IsPaid",
                value: false);

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000003"),
                column: "IsPaid",
                value: false);

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000004"),
                column: "IsPaid",
                value: false);

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000005"),
                column: "IsPaid",
                value: false);

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000006"),
                column: "IsPaid",
                value: false);

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000007"),
                column: "IsPaid",
                value: false);

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000008"),
                column: "IsPaid",
                value: false);

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000009"),
                column: "IsPaid",
                value: false);

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000010"),
                column: "IsPaid",
                value: false);

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000011"),
                column: "IsPaid",
                value: false);

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000012"),
                column: "IsPaid",
                value: false);

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000013"),
                column: "IsPaid",
                value: false);

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000014"),
                column: "IsPaid",
                value: false);

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000015"),
                column: "IsPaid",
                value: false);

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000016"),
                column: "IsPaid",
                value: false);

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000017"),
                column: "IsPaid",
                value: false);

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000018"),
                column: "IsPaid",
                value: false);

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000019"),
                column: "IsPaid",
                value: false);

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000020"),
                column: "IsPaid",
                value: false);

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000021"),
                column: "IsPaid",
                value: false);

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000022"),
                column: "IsPaid",
                value: false);

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000023"),
                column: "IsPaid",
                value: false);

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000024"),
                column: "IsPaid",
                value: false);

            migrationBuilder.UpdateData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000025"),
                column: "IsPaid",
                value: false);
        }
    }
}
