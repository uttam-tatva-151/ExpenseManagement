using System.Text.Json;
using CashCanvas.Common.ConstantHandler;
using CashCanvas.Core.Beans.Enums;
using CashCanvas.Core.Beans.Events;
using CashCanvas.Core.DTOs;
using CashCanvas.Core.Entities;
using CashCanvas.Data.UnitOfWork;
using CashCanvas.Services.Interfaces;
using MediatR;

namespace CashCanvas.Services.Services;

public class NotificationService(IUnitOfWork unitOfWork, IMediator mediator) : INotificationService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMediator _mediator = mediator;
    public async Task SyncBillWithNotificationsAsync(Guid userId, Bill bill, string action)
    {
        string title = "Bill Reminder";
        string message = $"Your bill '{bill.Title}' is due on {bill.DueDate:yyyy-MM-dd}.";
        Notifications notification = CreateNotification(userId, bill, NotificationType.Reminder, title, message);

        IEnumerable<Notifications> existingAllNotifications = await _unitOfWork.Notifications.GetListAsync(
                                                                                n => n.UserId == userId &&
                                                                                    n.Type == NotificationType.Reminder &&
                                                                                    !n.IsDeleted &&
                                                                                    n.Meta.Contains($"\"BillId\":\"{bill.BillId}\"")
                                                                            );
        if (action == Constants.DATABASE_ACTION_DELETE)
        {
            existingAllNotifications.ToList().ForEach(n => n.IsDeleted = true);
            _unitOfWork.Notifications.UpdateRange(existingAllNotifications);
            return;
        }
        else
        {
            bool isBillDueSoon = bill.NextDueDate <= DateTime.UtcNow.AddDays(bill.ReminderDay);
            if (isBillDueSoon)
            {
                if (!existingAllNotifications.Contains(notification))
                {
                    await _unitOfWork.Notifications.AddAsync(notification);
                    await _unitOfWork.CompleteAsync();
                    await _mediator.Publish(new NotificationCreatedEvent(notification.UserId));
                }
            }
        }

    }

    public async Task SyncBudgetWithNotificationsAsync(Guid userId, Budget budget, string action)
    {
        string title = "Budget Alert Notification";
        string message = $"Your budget for '{budget.Category.CategoryName}' is close to being exceeded ({{percentage}}% used). It started on {budget.StartDate:dd-MM-yy} and covers a period of {budget.Period}.";
        Notifications notification = CreateNotification(userId, budget, NotificationType.Milestone, title, message);

        IEnumerable<Notifications> existingAllNotifications = await _unitOfWork.Notifications.GetListAsync(
                                                                                n => n.UserId == userId &&
                                                                                    n.Type == NotificationType.Milestone &&
                                                                                    !n.IsDeleted &&
                                                                                    n.Meta.Contains($"\"BudgetId\":\"{budget.BudgetId}\"")
                                                                            );

        if (action == Constants.DATABASE_ACTION_DELETE)
        {
            existingAllNotifications.ToList().ForEach(n => n.IsDeleted = true);
            _unitOfWork.Notifications.UpdateRange(existingAllNotifications);
            return;
        }
        else
        {

            DateTime StartDate = FindStartDateForCurrentFrequency(budget.StartDate, budget.Period);
            List<Transaction> transactions = await _unitOfWork.Transactions.GetListAsync(
                t => t.UserId == userId &&
                     t.CategoryId == budget.CategoryId &&
                     t.TransactionDate >= StartDate &&
                     t.TransactionDate <= DateTime.UtcNow);
            decimal totalSpent = transactions.Sum(t => t.Amount);
            if (totalSpent >= budget.Amount * 0.7m)
            {
                await AddNotificationWithPercentageAsync(notification, message, totalSpent, budget.Amount, existingAllNotifications);
            }
        }
    }

    public async Task SyncTransactionAffectingBudgetNotificationsAsync(Guid userId, Transaction transaction)
    {
        // Find all budgets related to this transaction's category and user
        List<Budget> budgets = await _unitOfWork.Budgets.GetListAsync(
            b => b.UserId == userId && b.CategoryId == transaction.CategoryId
        );

        foreach (Budget budget in budgets)
        {
            string title = "Budget Alert Notification";
            string message = $"Your budget for '{budget.Category.CategoryName}' is close to being exceeded ({{percentage}}% used). It started on {budget.StartDate:dd-MM-yy} and covers a period of {budget.Period}.";

            Notifications notification = CreateNotification(userId, budget, NotificationType.Milestone, title, message);

            IEnumerable<Notifications> existingAllNotifications = await _unitOfWork.Notifications.GetListAsync(
                n => n.UserId == userId &&
                    n.Type == NotificationType.Milestone &&
                    !n.IsDeleted &&
                    n.Meta.Contains($"\"BudgetId\":\"{budget.BudgetId}\"")
            );

            // Find the current frequency's period start date for the budget
            DateTime periodStart = FindStartDateForCurrentFrequency(budget.StartDate, budget.Period);

            // Get all transactions for this category/user in this budget period
            List<Transaction> periodTransactions = await _unitOfWork.Transactions.GetListAsync(
                t => t.UserId == userId &&
                     t.CategoryId == budget.CategoryId &&
                     t.TransactionDate >= periodStart &&
                     t.TransactionDate <= DateTime.UtcNow
            );

            decimal totalSpent = periodTransactions.Sum(t => t.Amount);

            if (totalSpent >= budget.Amount * 0.7m)
            {
                await AddNotificationWithPercentageAsync(notification, message, totalSpent, budget.Amount, existingAllNotifications);
            }

        }
    }

    public async Task<List<NotificationDTO>> GetActiveNotificationsForUserAsync(Guid userId)
    {
        List<Notifications> notifications = await _unitOfWork.Notifications.GetListAsync(
            n => n.UserId == userId && !n.IsDeleted && !n.IsRead
        );

        return [.. notifications.Select(n => new NotificationDTO
        {
            Id = n.Id,
            UserId = n.UserId,
            Type = n.Type,
            Title = n.Title,
            Message = n.Message,
            CreatedAt = n.CreatedAt,
            IsRead = n.IsRead,
            Meta = JsonSerializer.Deserialize<NotificationDTO.NotificationMeta>(n.Meta)
        })];
    }

    public async Task SyncReminderNotificationAsync(Reminder reminder)
    {
        NotificationDTO.NotificationMeta meta = new()
        {
            SourceType = NotificationSourceType.Reminder,
            ReminderId = reminder.ReminderId,
            BillId = reminder.BillId,
            ReminderDateTime = reminder.ReminderDate,
            Context = $"Reminder for Bill-{reminder.BillId} set for {reminder.ReminderDate:yyyy-MM-dd HH:mm} (ReminderId: {reminder.ReminderId})"
        };

        var exists = (await _unitOfWork.Notifications.GetListAsync(
            n => n.UserId == reminder.UserId &&
                 n.Type == NotificationType.Reminder &&
                 !n.IsDeleted &&
                 n.Meta.Contains($"\"ReminderId\":\"{reminder.ReminderId}\"") &&
                 n.Meta.Contains($"\"ReminderDateTime\":\"{reminder.ReminderDate:O}\"")
        )).Any();

        if (!exists)
        {
            Notifications notification = new()
            {
                Id = Guid.NewGuid(),
                UserId = reminder.UserId,
                Type = NotificationType.Reminder,
                Title = "Bill Reminder",
                Message = reminder.Notes ?? "You have a bill reminder scheduled.",
                Meta = JsonSerializer.Serialize(meta),
                CreatedAt = DateTime.UtcNow,
                IsRead = false,
                IsDeleted = false,
            };
            await _unitOfWork.Notifications.AddAsync(notification);
            await _unitOfWork.CompleteAsync();
            await _mediator.Publish(new NotificationCreatedEvent(notification.UserId));
        }
    }

    private async Task AddNotificationWithPercentageAsync(Notifications notification, string messageTemplate,
                                                            decimal totalSpent, decimal budgetAmount,
                                                                IEnumerable<Notifications> existingAllNotifications)
    {
        string percentage = ((totalSpent / budgetAmount) * 100).ToString("F2");
        notification.Message = messageTemplate.Replace("{{percentage}}", percentage);

        if (!existingAllNotifications.Contains(notification))
        {
            await _unitOfWork.Notifications.AddAsync(notification);
            await _unitOfWork.CompleteAsync();
            await _mediator.Publish(new NotificationCreatedEvent(notification.UserId));
        }
    }
    private static DateTime FindStartDateForCurrentFrequency(DateTime createdAt, BudgetPeriod frequency)
    {
        DateTime today = DateTime.UtcNow;
        int days = GetDaysForBudgetPeriod(frequency);
        int totalDays = (today.Date - createdAt.Date).Days;
        int periodsElapsed = totalDays / days;
        return createdAt.Date.AddDays(periodsElapsed * days);
    }
    private static int GetDaysForBudgetPeriod(BudgetPeriod period)
    {
        return period switch
        {
            BudgetPeriod.Weekly => 7,
            BudgetPeriod.BiWeekly => 14,
            BudgetPeriod.Monthly => 30,
            BudgetPeriod.Quarterly => 90,
            BudgetPeriod.HalfYearly => 182,
            BudgetPeriod.Yearly => 365,
            _ => throw new ArgumentOutOfRangeException(nameof(period), period, "Unknown BudgetPeriod"),
        };

    }
    private static Notifications CreateNotification(Guid userId, object source, NotificationType type, string title, string message)
    {
        NotificationDTO.NotificationMeta meta = new();

        switch (source)
        {
            case Bill bill:
                meta.SourceType = NotificationSourceType.Bill;
                meta.BillId = bill.BillId;
                meta.DueDate = bill.DueDate;
                meta.Context = "'Remind me before " + bill.ReminderDay + " days' ~ by User (Default set at time of bill creation)";
                break;

            case Budget budget:
                meta.SourceType = NotificationSourceType.Budget;
                meta.BudgetId = budget.BudgetId;
                meta.BudgetCategory = budget.Category.CategoryId.ToString();
                meta.Context = "Starts From :- " + budget.StartDate.ToString("dd-MM-yy") + " for Budget Frequncey :- " + budget.Period.ToString();
                break;

            case Reminder reminder:
                meta.SourceType = NotificationSourceType.Reminder;
                meta.ReminderId = reminder.ReminderId;
                meta.BillId = reminder.BillId;
                meta.ReminderDateTime = reminder.ReminderDate;
                meta.Context = $"Custom reminder for Bill {reminder.BillId} at {reminder.ReminderDate:yyyy-MM-dd HH:mm}";
                break;

            case Transaction transaction:
                meta.SourceType = NotificationSourceType.Transaction;
                meta.TransactionId = transaction.TransactionId;
                meta.Amount = transaction.Amount;
                meta.BudgetCategory = transaction.CategoryId.ToString();
                meta.Context =
                         $"[{transaction.TransactionType}] {transaction.Amount:C} on {transaction.TransactionDate:yyyy-MM-dd} in Category-{transaction.CategoryId} )";
                break;

            default:
                meta.SourceType = NotificationSourceType.Custom;
                meta.Context = "Unknown source type";
                break;
        }

        return new Notifications
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Type = type,
            Title = title,
            Message = message,
            IsRead = false,
            CreatedAt = DateTime.UtcNow,
            Meta = JsonSerializer.Serialize(meta),
            IsDeleted = false
        };
    }

}
