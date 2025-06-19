using MediatR;

namespace CashCanvas.Core.Beans.Events;

public class NotificationCreatedEvent(Guid userId) : INotification
{
    public Guid UserId { get; } = userId;
}
