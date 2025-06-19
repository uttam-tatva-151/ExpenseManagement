using CashCanvas.Core.Beans.Events;
using CashCanvas.Core.DTOs;
using CashCanvas.Services.Interfaces;
using CashCanvas.Web.Hubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace CashCanvas.Web.Events;

public class NotificationCreatedHandler : INotificationHandler<NotificationCreatedEvent>
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly INotificationService _notificationService;

        public NotificationCreatedHandler(
            IHubContext<NotificationHub> hubContext,
            INotificationService notificationService)
        {
            _hubContext = hubContext;
            _notificationService = notificationService;
        }

        public async Task Handle(NotificationCreatedEvent notification, CancellationToken cancellationToken)
        {
            Guid userId = notification.UserId;
            List<NotificationDTO> notifications = await _notificationService.GetActiveNotificationsForUserAsync(userId);
            await _hubContext.Clients.User(userId.ToString())
                .SendAsync("AllActiveNotifications", notifications, cancellationToken: cancellationToken);
        }
    }
