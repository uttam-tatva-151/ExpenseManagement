using CashCanvas.Core.Beans.Enums;

namespace CashCanvas.Core.DTOs;

public class NotificationDTO
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public NotificationType Type { get; set; }
    public string Title { get; set; } = null!;
    public string Message { get; set; } = null!;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifyAt { get; set; }
    public bool IsDeleted { get; set; }

    // Strongly-typed meta
    public NotificationMeta? Meta { get; set; }

    public class NotificationMeta
    {
        public NotificationSourceType SourceType { get; set; } = NotificationSourceType.Custom; // "Bill", "Reminder", "Budget", "Transaction"
        public Guid? BillId { get; set; }
        public DateTime? DueDate { get; set; }
        public Guid? ReminderId { get; set; }
        public DateTime? ReminderDateTime { get; set; }
        public Guid? BudgetId { get; set; }
        public string? BudgetCategory { get; set; }
        public Guid? TransactionId { get; set; }
        public decimal? Amount { get; set; }
        public string? Context { get; set; }
    }

}
