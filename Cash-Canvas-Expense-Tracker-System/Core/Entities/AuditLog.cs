namespace CashCanvas.Core.Entities;

public class AuditLog
{
    public int AuditLogId { get; set; }
    public string TableName { get; set; }
    public string ActionType { get; set; }
    public string RecordId { get; set; }
    public string ChangedBy { get; set; }
    public DateTime ChangedAt { get; set; }
    public string OldValues { get; set; }   // JSON
    public string NewValues { get; set; }   // JSON
    public string Query { get; set; }
}
