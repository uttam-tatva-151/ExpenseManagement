using CashCanvas.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CashCanvas.Web.Controllers;

public class NotificationController(INotificationService notificationService) : Controller
{
    private readonly INotificationService _notificationService = notificationService;
    
}
