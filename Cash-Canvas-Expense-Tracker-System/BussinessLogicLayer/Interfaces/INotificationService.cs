using CashCanvas.Core.DTOs;
using CashCanvas.Core.Entities;

namespace CashCanvas.Services.Interfaces;

public interface INotificationService
{
    Task SyncBillWithNotificationsAsync(Guid userId, Bill bill, string action);
    Task SyncBudgetWithNotificationsAsync(Guid userId, Budget budget, string action);
    Task SyncTransactionAffectingBudgetNotificationsAsync(Guid userId, Transaction transaction);
    Task SyncReminderNotificationAsync(Reminder reminder);
    Task<List<NotificationDTO>> GetActiveNotificationsForUserAsync(Guid userId);
}
