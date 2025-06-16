using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CashCanvas.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditLogWithTrigger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    AuditLogId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TableName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ActionType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    RecordId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ChangedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    OldValues = table.Column<string>(type: "jsonb", nullable: false),
                    NewValues = table.Column<string>(type: "jsonb", nullable: false),
                    Query = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.AuditLogId);
                });
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION audit_table_changes() RETURNS TRIGGER AS $$
                DECLARE
                    affected_id TEXT;
                    old_json JSONB;
                    new_json JSONB;
                BEGIN
                    IF (TG_OP = 'INSERT') THEN
                        new_json := to_jsonb(NEW);
                        affected_id := COALESCE((NEW).id::text, NULL);
                        INSERT INTO ""AuditLogs""(""TableName"", ""ActionType"", ""RecordId"", ""ChangedBy"", ""NewValues"")
                        VALUES (TG_TABLE_NAME, 'INSERT', affected_id, current_user, new_json);
                        RETURN NEW;
                    ELSIF (TG_OP = 'UPDATE') THEN
                        old_json := to_jsonb(OLD);
                        new_json := to_jsonb(NEW);
                        affected_id := COALESCE((NEW).id::text, NULL);
                        INSERT INTO ""AuditLogs""(""TableName"", ""ActionType"", ""RecordId"", ""ChangedBy"", ""OldValues"", ""NewValues"")
                        VALUES (TG_TABLE_NAME, 'UPDATE', affected_id, current_user, old_json, new_json);
                        RETURN NEW;
                    ELSIF (TG_OP = 'DELETE') THEN
                        old_json := to_jsonb(OLD);
                        affected_id := COALESCE((OLD).id::text, NULL);
                        INSERT INTO ""AuditLogs""(""TableName"", ""ActionType"", ""RecordId"", ""ChangedBy"", ""OldValues"")
                        VALUES (TG_TABLE_NAME, 'DELETE', affected_id, current_user, old_json);
                        RETURN OLD;
                    END IF;
                    RETURN NULL;
                END;
                $$ LANGUAGE plpgsql;
            ");

            migrationBuilder.Sql(@"
                DO $$
                DECLARE
                    r RECORD;
                BEGIN
                    FOR r IN
                        SELECT table_name
                        FROM information_schema.tables
                        WHERE table_schema = 'public'
                        AND table_name <> 'AuditLogs'
                        AND table_name NOT LIKE 'pg_%'
                        AND table_name NOT LIKE 'sql_%'
                        AND table_name NOT LIKE '\\_%' ESCAPE '\'
                        AND table_name IN (
                            SELECT table_name
                            FROM information_schema.columns
                            WHERE column_name = 'id'
                                AND table_schema = 'public'
                        )
                    LOOP
                        EXECUTE format('
                            DROP TRIGGER IF EXISTS trg_audit_%1$s ON ""%1$s"";
                            CREATE TRIGGER trg_audit_%1$s
                            AFTER INSERT OR UPDATE OR DELETE ON ""%1$s""
                            FOR EACH ROW EXECUTE FUNCTION audit_table_changes();
                        ', r.table_name);
                    END LOOP;
                END;
                $$;
                ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");
        }
    }
}
