using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashCanvas.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUpsertErrorLogFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE OR REPLACE FUNCTION upsert_error_log(in_log error_log_dto)
RETURNS VOID AS $$
DECLARE
    v_ErrorMessage TEXT := TRIM(COALESCE(in_log.""ErrorMessage"", ''));
    v_StackTrace TEXT := TRIM(COALESCE(in_log.""StackTrace"", ''));
    v_ExceptionType VARCHAR(255) := TRIM(COALESCE(in_log.""ExceptionType"", ''));
    v_StatusCode VARCHAR(20) := TRIM(COALESCE(in_log.""StatusCode"", ''));
    v_ControllerName VARCHAR(255) := TRIM(COALESCE(in_log.""ControllerName"", ''));
    v_ActionName VARCHAR(255) := TRIM(COALESCE(in_log.""ActionName"", ''));
BEGIN
    IF v_ErrorMessage IS NULL OR LENGTH(v_ErrorMessage) = 0 THEN
        RAISE EXCEPTION 'ErrorMessage is required and cannot be empty or whitespace.';
    END IF;
    IF v_ExceptionType IS NULL OR LENGTH(v_ExceptionType) = 0 THEN
        RAISE EXCEPTION 'ExceptionType is required and cannot be empty or whitespace.';
    END IF;
    IF v_StatusCode IS NULL OR LENGTH(v_StatusCode) = 0 THEN
        RAISE EXCEPTION 'StatusCode is required and cannot be empty or whitespace.';
    END IF;

    INSERT INTO public.""ErrorLog"" (
        ""ErrorMessage"",
        ""StackTrace"",
        ""ExceptionType"",
        ""StatusCode"",
        ""ControllerName"",
        ""ActionName"",
        ""ErrorOccurAt"",
        ""ErrorOccurCount"",
        ""IsSolved""
    ) VALUES (
        v_ErrorMessage,
        v_StackTrace,
        v_ExceptionType,
        v_StatusCode,
        v_ControllerName,
        v_ActionName,
        now(),
        1,
        false
    )
    ON CONFLICT (""ExceptionType"", ""StatusCode"", ""ControllerName"", ""ActionName"")
    DO UPDATE SET
        ""ErrorOccurCount"" = ""ErrorLog"".""ErrorOccurCount"" + 1,
        ""ErrorMessage"" = EXCLUDED.""ErrorMessage"",
        ""StackTrace"" = EXCLUDED.""StackTrace"",
        ""ErrorOccurAt"" = now(),
        ""IsSolved"" = false;
END;
$$ LANGUAGE plpgsql;
    ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
