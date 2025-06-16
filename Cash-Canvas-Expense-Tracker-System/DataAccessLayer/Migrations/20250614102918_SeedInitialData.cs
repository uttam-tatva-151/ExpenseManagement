using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CashCanvas.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "Email", "HashPassword", "IsActive", "LastLoginAt", "ModifiedAt", "UserName" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "wallet_holder_user3@yopmail.com", "$2a$11$DMIHU83vP3/khhmBkZ0rAePjQWVuThT0L95l1Q97vgJXlDBJIyLrm", true, null, null, "user3" },
                    { new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "wallet_holder_user2@yopmail.com", "$2a$11$DMIHU83vP3/khhmBkZ0rAePjQWVuThT0L95l1Q97vgJXlDBJIyLrm", true, null, null, "user2" },
                    { new Guid("00000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "wallet_holder_user1@yopmail.com", "$2a$11$DMIHU83vP3/khhmBkZ0rAePjQWVuThT0L95l1Q97vgJXlDBJIyLrm", true, null, null, "user1" },
                    { new Guid("00000000-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "wallet_holder_uttam@yopmail.com", "$2a$11$DMIHU83vP3/khhmBkZ0rAePjQWVuThT0L95l1Q97vgJXlDBJIyLrm", true, null, null, "uttam" },
                    { new Guid("00000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "wallet_holder_user4@yopmail.com", "$2a$11$DMIHU83vP3/khhmBkZ0rAePjQWVuThT0L95l1Q97vgJXlDBJIyLrm", true, null, null, "user4" }
                });

            migrationBuilder.InsertData(
                table: "Bills",
                columns: new[] { "BillId", "Amount", "CreatedAt", "DueDate", "Frequency", "IsContinued", "ModifiedAt", "Notes", "PaymentMethod", "ReminderDay", "Title", "UserId" },
                values: new object[,]
                {
                    { new Guid("30000000-0000-0000-0000-000000000001"), 501m, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 2, 12, 0, 0, 0, DateTimeKind.Utc), "Monthly", true, null, null, "Card", 2, "Bill 1", new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("30000000-0000-0000-0000-000000000002"), 502m, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 3, 12, 0, 0, 0, DateTimeKind.Utc), "Monthly", true, null, null, "Card", 2, "Bill 2", new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("30000000-0000-0000-0000-000000000003"), 503m, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 4, 12, 0, 0, 0, DateTimeKind.Utc), "Monthly", true, null, null, "Card", 2, "Bill 3", new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("30000000-0000-0000-0000-000000000004"), 504m, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 5, 12, 0, 0, 0, DateTimeKind.Utc), "Monthly", true, null, null, "Card", 2, "Bill 4", new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("30000000-0000-0000-0000-000000000005"), 505m, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 6, 12, 0, 0, 0, DateTimeKind.Utc), "Monthly", true, null, null, "Card", 2, "Bill 5", new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("30000000-0000-0000-0000-000000000006"), 501m, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 2, 12, 0, 0, 0, DateTimeKind.Utc), "Monthly", true, null, null, "Card", 2, "Bill 1", new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("30000000-0000-0000-0000-000000000007"), 502m, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 3, 12, 0, 0, 0, DateTimeKind.Utc), "Monthly", true, null, null, "Card", 2, "Bill 2", new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("30000000-0000-0000-0000-000000000008"), 503m, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 4, 12, 0, 0, 0, DateTimeKind.Utc), "Monthly", true, null, null, "Card", 2, "Bill 3", new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("30000000-0000-0000-0000-000000000009"), 504m, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 5, 12, 0, 0, 0, DateTimeKind.Utc), "Monthly", true, null, null, "Card", 2, "Bill 4", new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("30000000-0000-0000-0000-000000000010"), 505m, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 6, 12, 0, 0, 0, DateTimeKind.Utc), "Monthly", true, null, null, "Card", 2, "Bill 5", new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("30000000-0000-0000-0000-000000000011"), 501m, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 2, 12, 0, 0, 0, DateTimeKind.Utc), "Monthly", true, null, null, "Card", 2, "Bill 1", new Guid("00000000-0000-0000-0000-000000000003") },
                    { new Guid("30000000-0000-0000-0000-000000000012"), 502m, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 3, 12, 0, 0, 0, DateTimeKind.Utc), "Monthly", true, null, null, "Card", 2, "Bill 2", new Guid("00000000-0000-0000-0000-000000000003") },
                    { new Guid("30000000-0000-0000-0000-000000000013"), 503m, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 4, 12, 0, 0, 0, DateTimeKind.Utc), "Monthly", true, null, null, "Card", 2, "Bill 3", new Guid("00000000-0000-0000-0000-000000000003") },
                    { new Guid("30000000-0000-0000-0000-000000000014"), 504m, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 5, 12, 0, 0, 0, DateTimeKind.Utc), "Monthly", true, null, null, "Card", 2, "Bill 4", new Guid("00000000-0000-0000-0000-000000000003") },
                    { new Guid("30000000-0000-0000-0000-000000000015"), 505m, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 6, 12, 0, 0, 0, DateTimeKind.Utc), "Monthly", true, null, null, "Card", 2, "Bill 5", new Guid("00000000-0000-0000-0000-000000000003") },
                    { new Guid("30000000-0000-0000-0000-000000000016"), 501m, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 2, 12, 0, 0, 0, DateTimeKind.Utc), "Monthly", true, null, null, "Card", 2, "Bill 1", new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("30000000-0000-0000-0000-000000000017"), 502m, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 3, 12, 0, 0, 0, DateTimeKind.Utc), "Monthly", true, null, null, "Card", 2, "Bill 2", new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("30000000-0000-0000-0000-000000000018"), 503m, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 4, 12, 0, 0, 0, DateTimeKind.Utc), "Monthly", true, null, null, "Card", 2, "Bill 3", new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("30000000-0000-0000-0000-000000000019"), 504m, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 5, 12, 0, 0, 0, DateTimeKind.Utc), "Monthly", true, null, null, "Card", 2, "Bill 4", new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("30000000-0000-0000-0000-000000000020"), 505m, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 6, 12, 0, 0, 0, DateTimeKind.Utc), "Monthly", true, null, null, "Card", 2, "Bill 5", new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("30000000-0000-0000-0000-000000000021"), 501m, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 2, 12, 0, 0, 0, DateTimeKind.Utc), "Monthly", true, null, null, "Card", 2, "Bill 1", new Guid("00000000-0000-0000-0000-000000000005") },
                    { new Guid("30000000-0000-0000-0000-000000000022"), 502m, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 3, 12, 0, 0, 0, DateTimeKind.Utc), "Monthly", true, null, null, "Card", 2, "Bill 2", new Guid("00000000-0000-0000-0000-000000000005") },
                    { new Guid("30000000-0000-0000-0000-000000000023"), 503m, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 4, 12, 0, 0, 0, DateTimeKind.Utc), "Monthly", true, null, null, "Card", 2, "Bill 3", new Guid("00000000-0000-0000-0000-000000000005") },
                    { new Guid("30000000-0000-0000-0000-000000000024"), 504m, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 5, 12, 0, 0, 0, DateTimeKind.Utc), "Monthly", true, null, null, "Card", 2, "Bill 4", new Guid("00000000-0000-0000-0000-000000000005") },
                    { new Guid("30000000-0000-0000-0000-000000000025"), 505m, new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 6, 12, 0, 0, 0, DateTimeKind.Utc), "Monthly", true, null, null, "Card", 2, "Bill 5", new Guid("00000000-0000-0000-0000-000000000005") }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryName", "CreatedAt", "Description", "IsActive", "ModifiedAt", "Type", "UserId" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), "Category1", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Test category 1", true, null, "Income", new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-000000000002"), "Category2", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Test category 2", true, null, "Expense", new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-000000000003"), "Category3", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Test category 3", true, null, "Income", new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-000000000004"), "Category4", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Test category 4", true, null, "Expense", new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-000000000005"), "Category5", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Test category 5", true, null, "Income", new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-000000000006"), "Category1", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Test category 1", true, null, "Income", new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("10000000-0000-0000-0000-000000000007"), "Category2", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Test category 2", true, null, "Expense", new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("10000000-0000-0000-0000-000000000008"), "Category3", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Test category 3", true, null, "Income", new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("10000000-0000-0000-0000-000000000009"), "Category4", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Test category 4", true, null, "Expense", new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("10000000-0000-0000-0000-000000000010"), "Category5", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Test category 5", true, null, "Income", new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("10000000-0000-0000-0000-000000000011"), "Category1", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Test category 1", true, null, "Income", new Guid("00000000-0000-0000-0000-000000000003") },
                    { new Guid("10000000-0000-0000-0000-000000000012"), "Category2", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Test category 2", true, null, "Expense", new Guid("00000000-0000-0000-0000-000000000003") },
                    { new Guid("10000000-0000-0000-0000-000000000013"), "Category3", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Test category 3", true, null, "Income", new Guid("00000000-0000-0000-0000-000000000003") },
                    { new Guid("10000000-0000-0000-0000-000000000014"), "Category4", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Test category 4", true, null, "Expense", new Guid("00000000-0000-0000-0000-000000000003") },
                    { new Guid("10000000-0000-0000-0000-000000000015"), "Category5", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Test category 5", true, null, "Income", new Guid("00000000-0000-0000-0000-000000000003") },
                    { new Guid("10000000-0000-0000-0000-000000000016"), "Category1", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Test category 1", true, null, "Income", new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("10000000-0000-0000-0000-000000000017"), "Category2", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Test category 2", true, null, "Expense", new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("10000000-0000-0000-0000-000000000018"), "Category3", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Test category 3", true, null, "Income", new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("10000000-0000-0000-0000-000000000019"), "Category4", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Test category 4", true, null, "Expense", new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("10000000-0000-0000-0000-000000000020"), "Category5", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Test category 5", true, null, "Income", new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("10000000-0000-0000-0000-000000000021"), "Category1", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Test category 1", true, null, "Income", new Guid("00000000-0000-0000-0000-000000000005") },
                    { new Guid("10000000-0000-0000-0000-000000000022"), "Category2", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Test category 2", true, null, "Expense", new Guid("00000000-0000-0000-0000-000000000005") },
                    { new Guid("10000000-0000-0000-0000-000000000023"), "Category3", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Test category 3", true, null, "Income", new Guid("00000000-0000-0000-0000-000000000005") },
                    { new Guid("10000000-0000-0000-0000-000000000024"), "Category4", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Test category 4", true, null, "Expense", new Guid("00000000-0000-0000-0000-000000000005") },
                    { new Guid("10000000-0000-0000-0000-000000000025"), "Category5", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Test category 5", true, null, "Income", new Guid("00000000-0000-0000-0000-000000000005") }
                });

            migrationBuilder.InsertData(
                table: "PasswordRecoveryTokens",
                columns: new[] { "TokenId", "CreatedAt", "ExpirationTime", "Token", "UserId" },
                values: new object[,]
                {
                    { new Guid("60000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 2, 12, 0, 0, 0, DateTimeKind.Utc), "recovery_token_1", new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("60000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 2, 12, 0, 0, 0, DateTimeKind.Utc), "recovery_token_2", new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("60000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 2, 12, 0, 0, 0, DateTimeKind.Utc), "recovery_token_3", new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("60000000-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 2, 12, 0, 0, 0, DateTimeKind.Utc), "recovery_token_4", new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("60000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 2, 12, 0, 0, 0, DateTimeKind.Utc), "recovery_token_5", new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("60000000-0000-0000-0000-000000000006"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 2, 12, 0, 0, 0, DateTimeKind.Utc), "recovery_token_6", new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("60000000-0000-0000-0000-000000000007"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 2, 12, 0, 0, 0, DateTimeKind.Utc), "recovery_token_7", new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("60000000-0000-0000-0000-000000000008"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 2, 12, 0, 0, 0, DateTimeKind.Utc), "recovery_token_8", new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("60000000-0000-0000-0000-000000000009"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 2, 12, 0, 0, 0, DateTimeKind.Utc), "recovery_token_9", new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("60000000-0000-0000-0000-000000000010"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 2, 12, 0, 0, 0, DateTimeKind.Utc), "recovery_token_10", new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("60000000-0000-0000-0000-000000000011"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 2, 12, 0, 0, 0, DateTimeKind.Utc), "recovery_token_11", new Guid("00000000-0000-0000-0000-000000000003") },
                    { new Guid("60000000-0000-0000-0000-000000000012"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 2, 12, 0, 0, 0, DateTimeKind.Utc), "recovery_token_12", new Guid("00000000-0000-0000-0000-000000000003") },
                    { new Guid("60000000-0000-0000-0000-000000000013"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 2, 12, 0, 0, 0, DateTimeKind.Utc), "recovery_token_13", new Guid("00000000-0000-0000-0000-000000000003") },
                    { new Guid("60000000-0000-0000-0000-000000000014"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 2, 12, 0, 0, 0, DateTimeKind.Utc), "recovery_token_14", new Guid("00000000-0000-0000-0000-000000000003") },
                    { new Guid("60000000-0000-0000-0000-000000000015"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 2, 12, 0, 0, 0, DateTimeKind.Utc), "recovery_token_15", new Guid("00000000-0000-0000-0000-000000000003") },
                    { new Guid("60000000-0000-0000-0000-000000000016"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 2, 12, 0, 0, 0, DateTimeKind.Utc), "recovery_token_16", new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("60000000-0000-0000-0000-000000000017"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 2, 12, 0, 0, 0, DateTimeKind.Utc), "recovery_token_17", new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("60000000-0000-0000-0000-000000000018"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 2, 12, 0, 0, 0, DateTimeKind.Utc), "recovery_token_18", new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("60000000-0000-0000-0000-000000000019"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 2, 12, 0, 0, 0, DateTimeKind.Utc), "recovery_token_19", new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("60000000-0000-0000-0000-000000000020"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 2, 12, 0, 0, 0, DateTimeKind.Utc), "recovery_token_20", new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("60000000-0000-0000-0000-000000000021"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 2, 12, 0, 0, 0, DateTimeKind.Utc), "recovery_token_21", new Guid("00000000-0000-0000-0000-000000000005") },
                    { new Guid("60000000-0000-0000-0000-000000000022"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 2, 12, 0, 0, 0, DateTimeKind.Utc), "recovery_token_22", new Guid("00000000-0000-0000-0000-000000000005") },
                    { new Guid("60000000-0000-0000-0000-000000000023"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 2, 12, 0, 0, 0, DateTimeKind.Utc), "recovery_token_23", new Guid("00000000-0000-0000-0000-000000000005") },
                    { new Guid("60000000-0000-0000-0000-000000000024"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 2, 12, 0, 0, 0, DateTimeKind.Utc), "recovery_token_24", new Guid("00000000-0000-0000-0000-000000000005") },
                    { new Guid("60000000-0000-0000-0000-000000000025"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 2, 12, 0, 0, 0, DateTimeKind.Utc), "recovery_token_25", new Guid("00000000-0000-0000-0000-000000000005") }
                });

            migrationBuilder.InsertData(
                table: "RefreshTokens",
                columns: new[] { "TokenId", "CreatedAt", "CreatedByIp", "ExpirationTime", "IsActive", "RevokedAt", "RevokedByIp", "Token", "UserId" },
                values: new object[,]
                {
                    { new Guid("70000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "127.0.0.1", new DateTime(2024, 1, 8, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "refresh_token_1", new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("70000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "127.0.0.1", new DateTime(2024, 1, 8, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "refresh_token_2", new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("70000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "127.0.0.1", new DateTime(2024, 1, 8, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "refresh_token_3", new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("70000000-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "127.0.0.1", new DateTime(2024, 1, 8, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "refresh_token_4", new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("70000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "127.0.0.1", new DateTime(2024, 1, 8, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "refresh_token_5", new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("70000000-0000-0000-0000-000000000006"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "127.0.0.1", new DateTime(2024, 1, 8, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "refresh_token_6", new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("70000000-0000-0000-0000-000000000007"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "127.0.0.1", new DateTime(2024, 1, 8, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "refresh_token_7", new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("70000000-0000-0000-0000-000000000008"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "127.0.0.1", new DateTime(2024, 1, 8, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "refresh_token_8", new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("70000000-0000-0000-0000-000000000009"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "127.0.0.1", new DateTime(2024, 1, 8, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "refresh_token_9", new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("70000000-0000-0000-0000-000000000010"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "127.0.0.1", new DateTime(2024, 1, 8, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "refresh_token_10", new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("70000000-0000-0000-0000-000000000011"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "127.0.0.1", new DateTime(2024, 1, 8, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "refresh_token_11", new Guid("00000000-0000-0000-0000-000000000003") },
                    { new Guid("70000000-0000-0000-0000-000000000012"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "127.0.0.1", new DateTime(2024, 1, 8, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "refresh_token_12", new Guid("00000000-0000-0000-0000-000000000003") },
                    { new Guid("70000000-0000-0000-0000-000000000013"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "127.0.0.1", new DateTime(2024, 1, 8, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "refresh_token_13", new Guid("00000000-0000-0000-0000-000000000003") },
                    { new Guid("70000000-0000-0000-0000-000000000014"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "127.0.0.1", new DateTime(2024, 1, 8, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "refresh_token_14", new Guid("00000000-0000-0000-0000-000000000003") },
                    { new Guid("70000000-0000-0000-0000-000000000015"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "127.0.0.1", new DateTime(2024, 1, 8, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "refresh_token_15", new Guid("00000000-0000-0000-0000-000000000003") },
                    { new Guid("70000000-0000-0000-0000-000000000016"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "127.0.0.1", new DateTime(2024, 1, 8, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "refresh_token_16", new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("70000000-0000-0000-0000-000000000017"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "127.0.0.1", new DateTime(2024, 1, 8, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "refresh_token_17", new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("70000000-0000-0000-0000-000000000018"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "127.0.0.1", new DateTime(2024, 1, 8, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "refresh_token_18", new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("70000000-0000-0000-0000-000000000019"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "127.0.0.1", new DateTime(2024, 1, 8, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "refresh_token_19", new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("70000000-0000-0000-0000-000000000020"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "127.0.0.1", new DateTime(2024, 1, 8, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "refresh_token_20", new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("70000000-0000-0000-0000-000000000021"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "127.0.0.1", new DateTime(2024, 1, 8, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "refresh_token_21", new Guid("00000000-0000-0000-0000-000000000005") },
                    { new Guid("70000000-0000-0000-0000-000000000022"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "127.0.0.1", new DateTime(2024, 1, 8, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "refresh_token_22", new Guid("00000000-0000-0000-0000-000000000005") },
                    { new Guid("70000000-0000-0000-0000-000000000023"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "127.0.0.1", new DateTime(2024, 1, 8, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "refresh_token_23", new Guid("00000000-0000-0000-0000-000000000005") },
                    { new Guid("70000000-0000-0000-0000-000000000024"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "127.0.0.1", new DateTime(2024, 1, 8, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "refresh_token_24", new Guid("00000000-0000-0000-0000-000000000005") },
                    { new Guid("70000000-0000-0000-0000-000000000025"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "127.0.0.1", new DateTime(2024, 1, 8, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "refresh_token_25", new Guid("00000000-0000-0000-0000-000000000005") }
                });

            migrationBuilder.InsertData(
                table: "Budgets",
                columns: new[] { "BudgetId", "Amount", "CategoryId", "CreatedAt", "EndDate", "IsContinued", "ModifiedAt", "Notes", "Period", "StartDate", "UserId" },
                values: new object[,]
                {
                    { new Guid("50000000-0000-0000-0000-000000000001"), 1010m, new Guid("10000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "Monthly", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("50000000-0000-0000-0000-000000000002"), 1020m, new Guid("10000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "Monthly", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("50000000-0000-0000-0000-000000000003"), 1030m, new Guid("10000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "Monthly", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("50000000-0000-0000-0000-000000000004"), 1040m, new Guid("10000000-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "Monthly", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("50000000-0000-0000-0000-000000000005"), 1050m, new Guid("10000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "Monthly", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("50000000-0000-0000-0000-000000000006"), 1010m, new Guid("10000000-0000-0000-0000-000000000006"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "Monthly", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("50000000-0000-0000-0000-000000000007"), 1020m, new Guid("10000000-0000-0000-0000-000000000007"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "Monthly", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("50000000-0000-0000-0000-000000000008"), 1030m, new Guid("10000000-0000-0000-0000-000000000008"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "Monthly", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("50000000-0000-0000-0000-000000000009"), 1040m, new Guid("10000000-0000-0000-0000-000000000009"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "Monthly", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("50000000-0000-0000-0000-000000000010"), 1050m, new Guid("10000000-0000-0000-0000-000000000010"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "Monthly", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("50000000-0000-0000-0000-000000000011"), 1010m, new Guid("10000000-0000-0000-0000-000000000011"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "Monthly", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000003") },
                    { new Guid("50000000-0000-0000-0000-000000000012"), 1020m, new Guid("10000000-0000-0000-0000-000000000012"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "Monthly", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000003") },
                    { new Guid("50000000-0000-0000-0000-000000000013"), 1030m, new Guid("10000000-0000-0000-0000-000000000013"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "Monthly", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000003") },
                    { new Guid("50000000-0000-0000-0000-000000000014"), 1040m, new Guid("10000000-0000-0000-0000-000000000014"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "Monthly", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000003") },
                    { new Guid("50000000-0000-0000-0000-000000000015"), 1050m, new Guid("10000000-0000-0000-0000-000000000015"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "Monthly", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000003") },
                    { new Guid("50000000-0000-0000-0000-000000000016"), 1010m, new Guid("10000000-0000-0000-0000-000000000016"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "Monthly", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("50000000-0000-0000-0000-000000000017"), 1020m, new Guid("10000000-0000-0000-0000-000000000017"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "Monthly", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("50000000-0000-0000-0000-000000000018"), 1030m, new Guid("10000000-0000-0000-0000-000000000018"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "Monthly", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("50000000-0000-0000-0000-000000000019"), 1040m, new Guid("10000000-0000-0000-0000-000000000019"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "Monthly", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("50000000-0000-0000-0000-000000000020"), 1050m, new Guid("10000000-0000-0000-0000-000000000020"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "Monthly", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("50000000-0000-0000-0000-000000000021"), 1010m, new Guid("10000000-0000-0000-0000-000000000021"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "Monthly", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000005") },
                    { new Guid("50000000-0000-0000-0000-000000000022"), 1020m, new Guid("10000000-0000-0000-0000-000000000022"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "Monthly", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000005") },
                    { new Guid("50000000-0000-0000-0000-000000000023"), 1030m, new Guid("10000000-0000-0000-0000-000000000023"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "Monthly", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000005") },
                    { new Guid("50000000-0000-0000-0000-000000000024"), 1040m, new Guid("10000000-0000-0000-0000-000000000024"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "Monthly", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000005") },
                    { new Guid("50000000-0000-0000-0000-000000000025"), 1050m, new Guid("10000000-0000-0000-0000-000000000025"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, "Monthly", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("00000000-0000-0000-0000-000000000005") }
                });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "PaymentId", "AmountPaid", "BillId", "CreatedAt", "IsContinued", "ModifiedAt", "Notes", "PaymentDate", "PaymentMethod", "Status", "UserId" },
                values: new object[,]
                {
                    { new Guid("40000000-0000-0000-0000-000000000001"), 101m, new Guid("30000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, new DateTime(2024, 1, 1, 12, 1, 0, 0, DateTimeKind.Utc), "Card", "Complete", new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("40000000-0000-0000-0000-000000000002"), 102m, new Guid("30000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, new DateTime(2024, 1, 1, 12, 2, 0, 0, DateTimeKind.Utc), "Card", "Complete", new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("40000000-0000-0000-0000-000000000003"), 103m, new Guid("30000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, new DateTime(2024, 1, 1, 12, 3, 0, 0, DateTimeKind.Utc), "Card", "Complete", new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("40000000-0000-0000-0000-000000000004"), 104m, new Guid("30000000-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, new DateTime(2024, 1, 1, 12, 4, 0, 0, DateTimeKind.Utc), "Card", "Complete", new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("40000000-0000-0000-0000-000000000005"), 105m, new Guid("30000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, new DateTime(2024, 1, 1, 12, 5, 0, 0, DateTimeKind.Utc), "Card", "Complete", new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("40000000-0000-0000-0000-000000000006"), 101m, new Guid("30000000-0000-0000-0000-000000000006"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, new DateTime(2024, 1, 1, 12, 1, 0, 0, DateTimeKind.Utc), "Card", "Complete", new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("40000000-0000-0000-0000-000000000007"), 102m, new Guid("30000000-0000-0000-0000-000000000007"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, new DateTime(2024, 1, 1, 12, 2, 0, 0, DateTimeKind.Utc), "Card", "Complete", new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("40000000-0000-0000-0000-000000000008"), 103m, new Guid("30000000-0000-0000-0000-000000000008"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, new DateTime(2024, 1, 1, 12, 3, 0, 0, DateTimeKind.Utc), "Card", "Complete", new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("40000000-0000-0000-0000-000000000009"), 104m, new Guid("30000000-0000-0000-0000-000000000009"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, new DateTime(2024, 1, 1, 12, 4, 0, 0, DateTimeKind.Utc), "Card", "Complete", new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("40000000-0000-0000-0000-000000000010"), 105m, new Guid("30000000-0000-0000-0000-000000000010"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, new DateTime(2024, 1, 1, 12, 5, 0, 0, DateTimeKind.Utc), "Card", "Complete", new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("40000000-0000-0000-0000-000000000011"), 101m, new Guid("30000000-0000-0000-0000-000000000011"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, new DateTime(2024, 1, 1, 12, 1, 0, 0, DateTimeKind.Utc), "Card", "Complete", new Guid("00000000-0000-0000-0000-000000000003") },
                    { new Guid("40000000-0000-0000-0000-000000000012"), 102m, new Guid("30000000-0000-0000-0000-000000000012"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, new DateTime(2024, 1, 1, 12, 2, 0, 0, DateTimeKind.Utc), "Card", "Complete", new Guid("00000000-0000-0000-0000-000000000003") },
                    { new Guid("40000000-0000-0000-0000-000000000013"), 103m, new Guid("30000000-0000-0000-0000-000000000013"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, new DateTime(2024, 1, 1, 12, 3, 0, 0, DateTimeKind.Utc), "Card", "Complete", new Guid("00000000-0000-0000-0000-000000000003") },
                    { new Guid("40000000-0000-0000-0000-000000000014"), 104m, new Guid("30000000-0000-0000-0000-000000000014"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, new DateTime(2024, 1, 1, 12, 4, 0, 0, DateTimeKind.Utc), "Card", "Complete", new Guid("00000000-0000-0000-0000-000000000003") },
                    { new Guid("40000000-0000-0000-0000-000000000015"), 105m, new Guid("30000000-0000-0000-0000-000000000015"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, new DateTime(2024, 1, 1, 12, 5, 0, 0, DateTimeKind.Utc), "Card", "Complete", new Guid("00000000-0000-0000-0000-000000000003") },
                    { new Guid("40000000-0000-0000-0000-000000000016"), 101m, new Guid("30000000-0000-0000-0000-000000000016"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, new DateTime(2024, 1, 1, 12, 1, 0, 0, DateTimeKind.Utc), "Card", "Complete", new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("40000000-0000-0000-0000-000000000017"), 102m, new Guid("30000000-0000-0000-0000-000000000017"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, new DateTime(2024, 1, 1, 12, 2, 0, 0, DateTimeKind.Utc), "Card", "Complete", new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("40000000-0000-0000-0000-000000000018"), 103m, new Guid("30000000-0000-0000-0000-000000000018"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, new DateTime(2024, 1, 1, 12, 3, 0, 0, DateTimeKind.Utc), "Card", "Complete", new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("40000000-0000-0000-0000-000000000019"), 104m, new Guid("30000000-0000-0000-0000-000000000019"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, new DateTime(2024, 1, 1, 12, 4, 0, 0, DateTimeKind.Utc), "Card", "Complete", new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("40000000-0000-0000-0000-000000000020"), 105m, new Guid("30000000-0000-0000-0000-000000000020"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, new DateTime(2024, 1, 1, 12, 5, 0, 0, DateTimeKind.Utc), "Card", "Complete", new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("40000000-0000-0000-0000-000000000021"), 101m, new Guid("30000000-0000-0000-0000-000000000021"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, new DateTime(2024, 1, 1, 12, 1, 0, 0, DateTimeKind.Utc), "Card", "Complete", new Guid("00000000-0000-0000-0000-000000000005") },
                    { new Guid("40000000-0000-0000-0000-000000000022"), 102m, new Guid("30000000-0000-0000-0000-000000000022"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, new DateTime(2024, 1, 1, 12, 2, 0, 0, DateTimeKind.Utc), "Card", "Complete", new Guid("00000000-0000-0000-0000-000000000005") },
                    { new Guid("40000000-0000-0000-0000-000000000023"), 103m, new Guid("30000000-0000-0000-0000-000000000023"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, new DateTime(2024, 1, 1, 12, 3, 0, 0, DateTimeKind.Utc), "Card", "Complete", new Guid("00000000-0000-0000-0000-000000000005") },
                    { new Guid("40000000-0000-0000-0000-000000000024"), 104m, new Guid("30000000-0000-0000-0000-000000000024"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, new DateTime(2024, 1, 1, 12, 4, 0, 0, DateTimeKind.Utc), "Card", "Complete", new Guid("00000000-0000-0000-0000-000000000005") },
                    { new Guid("40000000-0000-0000-0000-000000000025"), 105m, new Guid("30000000-0000-0000-0000-000000000025"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), true, null, null, new DateTime(2024, 1, 1, 12, 5, 0, 0, DateTimeKind.Utc), "Card", "Complete", new Guid("00000000-0000-0000-0000-000000000005") }
                });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "TransactionId", "Amount", "CategoryId", "CreatedAt", "Description", "IsContinued", "ModifiedAt", "PaymentMethod", "TransactionDate", "TransactionType", "UserId" },
                values: new object[,]
                {
                    { new Guid("20000000-0000-0000-0000-000000000001"), 101m, new Guid("10000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Transaction 1", true, null, "UPI", new DateTime(2024, 1, 2, 12, 0, 0, 0, DateTimeKind.Utc), "Income", new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("20000000-0000-0000-0000-000000000002"), 102m, new Guid("10000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Transaction 2", true, null, "UPI", new DateTime(2024, 1, 3, 12, 0, 0, 0, DateTimeKind.Utc), "Expense", new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("20000000-0000-0000-0000-000000000003"), 103m, new Guid("10000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Transaction 3", true, null, "UPI", new DateTime(2024, 1, 4, 12, 0, 0, 0, DateTimeKind.Utc), "Income", new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("20000000-0000-0000-0000-000000000004"), 104m, new Guid("10000000-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Transaction 4", true, null, "UPI", new DateTime(2024, 1, 5, 12, 0, 0, 0, DateTimeKind.Utc), "Expense", new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("20000000-0000-0000-0000-000000000005"), 105m, new Guid("10000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Transaction 5", true, null, "UPI", new DateTime(2024, 1, 6, 12, 0, 0, 0, DateTimeKind.Utc), "Income", new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("20000000-0000-0000-0000-000000000006"), 101m, new Guid("10000000-0000-0000-0000-000000000006"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Transaction 1", true, null, "UPI", new DateTime(2024, 1, 2, 12, 0, 0, 0, DateTimeKind.Utc), "Income", new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("20000000-0000-0000-0000-000000000007"), 102m, new Guid("10000000-0000-0000-0000-000000000007"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Transaction 2", true, null, "UPI", new DateTime(2024, 1, 3, 12, 0, 0, 0, DateTimeKind.Utc), "Expense", new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("20000000-0000-0000-0000-000000000008"), 103m, new Guid("10000000-0000-0000-0000-000000000008"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Transaction 3", true, null, "UPI", new DateTime(2024, 1, 4, 12, 0, 0, 0, DateTimeKind.Utc), "Income", new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("20000000-0000-0000-0000-000000000009"), 104m, new Guid("10000000-0000-0000-0000-000000000009"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Transaction 4", true, null, "UPI", new DateTime(2024, 1, 5, 12, 0, 0, 0, DateTimeKind.Utc), "Expense", new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("20000000-0000-0000-0000-000000000010"), 105m, new Guid("10000000-0000-0000-0000-000000000010"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Transaction 5", true, null, "UPI", new DateTime(2024, 1, 6, 12, 0, 0, 0, DateTimeKind.Utc), "Income", new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("20000000-0000-0000-0000-000000000011"), 101m, new Guid("10000000-0000-0000-0000-000000000011"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Transaction 1", true, null, "UPI", new DateTime(2024, 1, 2, 12, 0, 0, 0, DateTimeKind.Utc), "Income", new Guid("00000000-0000-0000-0000-000000000003") },
                    { new Guid("20000000-0000-0000-0000-000000000012"), 102m, new Guid("10000000-0000-0000-0000-000000000012"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Transaction 2", true, null, "UPI", new DateTime(2024, 1, 3, 12, 0, 0, 0, DateTimeKind.Utc), "Expense", new Guid("00000000-0000-0000-0000-000000000003") },
                    { new Guid("20000000-0000-0000-0000-000000000013"), 103m, new Guid("10000000-0000-0000-0000-000000000013"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Transaction 3", true, null, "UPI", new DateTime(2024, 1, 4, 12, 0, 0, 0, DateTimeKind.Utc), "Income", new Guid("00000000-0000-0000-0000-000000000003") },
                    { new Guid("20000000-0000-0000-0000-000000000014"), 104m, new Guid("10000000-0000-0000-0000-000000000014"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Transaction 4", true, null, "UPI", new DateTime(2024, 1, 5, 12, 0, 0, 0, DateTimeKind.Utc), "Expense", new Guid("00000000-0000-0000-0000-000000000003") },
                    { new Guid("20000000-0000-0000-0000-000000000015"), 105m, new Guid("10000000-0000-0000-0000-000000000015"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Transaction 5", true, null, "UPI", new DateTime(2024, 1, 6, 12, 0, 0, 0, DateTimeKind.Utc), "Income", new Guid("00000000-0000-0000-0000-000000000003") },
                    { new Guid("20000000-0000-0000-0000-000000000016"), 101m, new Guid("10000000-0000-0000-0000-000000000016"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Transaction 1", true, null, "UPI", new DateTime(2024, 1, 2, 12, 0, 0, 0, DateTimeKind.Utc), "Income", new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("20000000-0000-0000-0000-000000000017"), 102m, new Guid("10000000-0000-0000-0000-000000000017"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Transaction 2", true, null, "UPI", new DateTime(2024, 1, 3, 12, 0, 0, 0, DateTimeKind.Utc), "Expense", new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("20000000-0000-0000-0000-000000000018"), 103m, new Guid("10000000-0000-0000-0000-000000000018"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Transaction 3", true, null, "UPI", new DateTime(2024, 1, 4, 12, 0, 0, 0, DateTimeKind.Utc), "Income", new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("20000000-0000-0000-0000-000000000019"), 104m, new Guid("10000000-0000-0000-0000-000000000019"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Transaction 4", true, null, "UPI", new DateTime(2024, 1, 5, 12, 0, 0, 0, DateTimeKind.Utc), "Expense", new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("20000000-0000-0000-0000-000000000020"), 105m, new Guid("10000000-0000-0000-0000-000000000020"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Transaction 5", true, null, "UPI", new DateTime(2024, 1, 6, 12, 0, 0, 0, DateTimeKind.Utc), "Income", new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("20000000-0000-0000-0000-000000000021"), 101m, new Guid("10000000-0000-0000-0000-000000000021"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Transaction 1", true, null, "UPI", new DateTime(2024, 1, 2, 12, 0, 0, 0, DateTimeKind.Utc), "Income", new Guid("00000000-0000-0000-0000-000000000005") },
                    { new Guid("20000000-0000-0000-0000-000000000022"), 102m, new Guid("10000000-0000-0000-0000-000000000022"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Transaction 2", true, null, "UPI", new DateTime(2024, 1, 3, 12, 0, 0, 0, DateTimeKind.Utc), "Expense", new Guid("00000000-0000-0000-0000-000000000005") },
                    { new Guid("20000000-0000-0000-0000-000000000023"), 103m, new Guid("10000000-0000-0000-0000-000000000023"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Transaction 3", true, null, "UPI", new DateTime(2024, 1, 4, 12, 0, 0, 0, DateTimeKind.Utc), "Income", new Guid("00000000-0000-0000-0000-000000000005") },
                    { new Guid("20000000-0000-0000-0000-000000000024"), 104m, new Guid("10000000-0000-0000-0000-000000000024"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Transaction 4", true, null, "UPI", new DateTime(2024, 1, 5, 12, 0, 0, 0, DateTimeKind.Utc), "Expense", new Guid("00000000-0000-0000-0000-000000000005") },
                    { new Guid("20000000-0000-0000-0000-000000000025"), 105m, new Guid("10000000-0000-0000-0000-000000000025"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Transaction 5", true, null, "UPI", new DateTime(2024, 1, 6, 12, 0, 0, 0, DateTimeKind.Utc), "Income", new Guid("00000000-0000-0000-0000-000000000005") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "BudgetId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "BudgetId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "BudgetId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "BudgetId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "BudgetId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "BudgetId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "BudgetId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "BudgetId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "BudgetId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "BudgetId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000010"));

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "BudgetId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000011"));

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "BudgetId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000012"));

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "BudgetId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000013"));

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "BudgetId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000014"));

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "BudgetId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000015"));

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "BudgetId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000016"));

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "BudgetId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000017"));

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "BudgetId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000018"));

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "BudgetId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000019"));

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "BudgetId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000020"));

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "BudgetId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000021"));

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "BudgetId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000022"));

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "BudgetId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000023"));

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "BudgetId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000024"));

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "BudgetId",
                keyValue: new Guid("50000000-0000-0000-0000-000000000025"));

            migrationBuilder.DeleteData(
                table: "PasswordRecoveryTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("60000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "PasswordRecoveryTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("60000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "PasswordRecoveryTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("60000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "PasswordRecoveryTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("60000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "PasswordRecoveryTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("60000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "PasswordRecoveryTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("60000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "PasswordRecoveryTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("60000000-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "PasswordRecoveryTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("60000000-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "PasswordRecoveryTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("60000000-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "PasswordRecoveryTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("60000000-0000-0000-0000-000000000010"));

            migrationBuilder.DeleteData(
                table: "PasswordRecoveryTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("60000000-0000-0000-0000-000000000011"));

            migrationBuilder.DeleteData(
                table: "PasswordRecoveryTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("60000000-0000-0000-0000-000000000012"));

            migrationBuilder.DeleteData(
                table: "PasswordRecoveryTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("60000000-0000-0000-0000-000000000013"));

            migrationBuilder.DeleteData(
                table: "PasswordRecoveryTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("60000000-0000-0000-0000-000000000014"));

            migrationBuilder.DeleteData(
                table: "PasswordRecoveryTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("60000000-0000-0000-0000-000000000015"));

            migrationBuilder.DeleteData(
                table: "PasswordRecoveryTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("60000000-0000-0000-0000-000000000016"));

            migrationBuilder.DeleteData(
                table: "PasswordRecoveryTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("60000000-0000-0000-0000-000000000017"));

            migrationBuilder.DeleteData(
                table: "PasswordRecoveryTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("60000000-0000-0000-0000-000000000018"));

            migrationBuilder.DeleteData(
                table: "PasswordRecoveryTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("60000000-0000-0000-0000-000000000019"));

            migrationBuilder.DeleteData(
                table: "PasswordRecoveryTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("60000000-0000-0000-0000-000000000020"));

            migrationBuilder.DeleteData(
                table: "PasswordRecoveryTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("60000000-0000-0000-0000-000000000021"));

            migrationBuilder.DeleteData(
                table: "PasswordRecoveryTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("60000000-0000-0000-0000-000000000022"));

            migrationBuilder.DeleteData(
                table: "PasswordRecoveryTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("60000000-0000-0000-0000-000000000023"));

            migrationBuilder.DeleteData(
                table: "PasswordRecoveryTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("60000000-0000-0000-0000-000000000024"));

            migrationBuilder.DeleteData(
                table: "PasswordRecoveryTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("60000000-0000-0000-0000-000000000025"));

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: new Guid("40000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: new Guid("40000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: new Guid("40000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: new Guid("40000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: new Guid("40000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: new Guid("40000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: new Guid("40000000-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: new Guid("40000000-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: new Guid("40000000-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: new Guid("40000000-0000-0000-0000-000000000010"));

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: new Guid("40000000-0000-0000-0000-000000000011"));

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: new Guid("40000000-0000-0000-0000-000000000012"));

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: new Guid("40000000-0000-0000-0000-000000000013"));

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: new Guid("40000000-0000-0000-0000-000000000014"));

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: new Guid("40000000-0000-0000-0000-000000000015"));

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: new Guid("40000000-0000-0000-0000-000000000016"));

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: new Guid("40000000-0000-0000-0000-000000000017"));

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: new Guid("40000000-0000-0000-0000-000000000018"));

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: new Guid("40000000-0000-0000-0000-000000000019"));

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: new Guid("40000000-0000-0000-0000-000000000020"));

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: new Guid("40000000-0000-0000-0000-000000000021"));

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: new Guid("40000000-0000-0000-0000-000000000022"));

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: new Guid("40000000-0000-0000-0000-000000000023"));

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: new Guid("40000000-0000-0000-0000-000000000024"));

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: new Guid("40000000-0000-0000-0000-000000000025"));

            migrationBuilder.DeleteData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("70000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("70000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("70000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("70000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("70000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("70000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("70000000-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("70000000-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("70000000-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("70000000-0000-0000-0000-000000000010"));

            migrationBuilder.DeleteData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("70000000-0000-0000-0000-000000000011"));

            migrationBuilder.DeleteData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("70000000-0000-0000-0000-000000000012"));

            migrationBuilder.DeleteData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("70000000-0000-0000-0000-000000000013"));

            migrationBuilder.DeleteData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("70000000-0000-0000-0000-000000000014"));

            migrationBuilder.DeleteData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("70000000-0000-0000-0000-000000000015"));

            migrationBuilder.DeleteData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("70000000-0000-0000-0000-000000000016"));

            migrationBuilder.DeleteData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("70000000-0000-0000-0000-000000000017"));

            migrationBuilder.DeleteData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("70000000-0000-0000-0000-000000000018"));

            migrationBuilder.DeleteData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("70000000-0000-0000-0000-000000000019"));

            migrationBuilder.DeleteData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("70000000-0000-0000-0000-000000000020"));

            migrationBuilder.DeleteData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("70000000-0000-0000-0000-000000000021"));

            migrationBuilder.DeleteData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("70000000-0000-0000-0000-000000000022"));

            migrationBuilder.DeleteData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("70000000-0000-0000-0000-000000000023"));

            migrationBuilder.DeleteData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("70000000-0000-0000-0000-000000000024"));

            migrationBuilder.DeleteData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: new Guid("70000000-0000-0000-0000-000000000025"));

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "TransactionId",
                keyValue: new Guid("20000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "TransactionId",
                keyValue: new Guid("20000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "TransactionId",
                keyValue: new Guid("20000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "TransactionId",
                keyValue: new Guid("20000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "TransactionId",
                keyValue: new Guid("20000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "TransactionId",
                keyValue: new Guid("20000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "TransactionId",
                keyValue: new Guid("20000000-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "TransactionId",
                keyValue: new Guid("20000000-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "TransactionId",
                keyValue: new Guid("20000000-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "TransactionId",
                keyValue: new Guid("20000000-0000-0000-0000-000000000010"));

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "TransactionId",
                keyValue: new Guid("20000000-0000-0000-0000-000000000011"));

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "TransactionId",
                keyValue: new Guid("20000000-0000-0000-0000-000000000012"));

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "TransactionId",
                keyValue: new Guid("20000000-0000-0000-0000-000000000013"));

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "TransactionId",
                keyValue: new Guid("20000000-0000-0000-0000-000000000014"));

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "TransactionId",
                keyValue: new Guid("20000000-0000-0000-0000-000000000015"));

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "TransactionId",
                keyValue: new Guid("20000000-0000-0000-0000-000000000016"));

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "TransactionId",
                keyValue: new Guid("20000000-0000-0000-0000-000000000017"));

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "TransactionId",
                keyValue: new Guid("20000000-0000-0000-0000-000000000018"));

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "TransactionId",
                keyValue: new Guid("20000000-0000-0000-0000-000000000019"));

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "TransactionId",
                keyValue: new Guid("20000000-0000-0000-0000-000000000020"));

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "TransactionId",
                keyValue: new Guid("20000000-0000-0000-0000-000000000021"));

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "TransactionId",
                keyValue: new Guid("20000000-0000-0000-0000-000000000022"));

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "TransactionId",
                keyValue: new Guid("20000000-0000-0000-0000-000000000023"));

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "TransactionId",
                keyValue: new Guid("20000000-0000-0000-0000-000000000024"));

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "TransactionId",
                keyValue: new Guid("20000000-0000-0000-0000-000000000025"));

            migrationBuilder.DeleteData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000010"));

            migrationBuilder.DeleteData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000011"));

            migrationBuilder.DeleteData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000012"));

            migrationBuilder.DeleteData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000013"));

            migrationBuilder.DeleteData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000014"));

            migrationBuilder.DeleteData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000015"));

            migrationBuilder.DeleteData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000016"));

            migrationBuilder.DeleteData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000017"));

            migrationBuilder.DeleteData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000018"));

            migrationBuilder.DeleteData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000019"));

            migrationBuilder.DeleteData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000020"));

            migrationBuilder.DeleteData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000021"));

            migrationBuilder.DeleteData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000022"));

            migrationBuilder.DeleteData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000023"));

            migrationBuilder.DeleteData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000024"));

            migrationBuilder.DeleteData(
                table: "Bills",
                keyColumn: "BillId",
                keyValue: new Guid("30000000-0000-0000-0000-000000000025"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: new Guid("10000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: new Guid("10000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: new Guid("10000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: new Guid("10000000-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: new Guid("10000000-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: new Guid("10000000-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: new Guid("10000000-0000-0000-0000-000000000010"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: new Guid("10000000-0000-0000-0000-000000000011"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: new Guid("10000000-0000-0000-0000-000000000012"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: new Guid("10000000-0000-0000-0000-000000000013"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: new Guid("10000000-0000-0000-0000-000000000014"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: new Guid("10000000-0000-0000-0000-000000000015"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: new Guid("10000000-0000-0000-0000-000000000016"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: new Guid("10000000-0000-0000-0000-000000000017"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: new Guid("10000000-0000-0000-0000-000000000018"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: new Guid("10000000-0000-0000-0000-000000000019"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: new Guid("10000000-0000-0000-0000-000000000020"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: new Guid("10000000-0000-0000-0000-000000000021"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: new Guid("10000000-0000-0000-0000-000000000022"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: new Guid("10000000-0000-0000-0000-000000000023"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: new Guid("10000000-0000-0000-0000-000000000024"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: new Guid("10000000-0000-0000-0000-000000000025"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("00000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("00000000-0000-0000-0000-000000000005"));
        }
    }
}
