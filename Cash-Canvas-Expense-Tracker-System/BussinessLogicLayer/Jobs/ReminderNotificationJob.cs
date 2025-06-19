using CashCanvas.Core.Entities;
using CashCanvas.Data.BaseRepository;
using CashCanvas.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CashCanvas.Services.Jobs;

public class ReminderNotificationJob(IServiceProvider services) : BackgroundService
{
    private readonly IServiceProvider _services = services;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (IServiceScope scope = _services.CreateScope())
            {
                IGenericRepository<Reminder> reminderRepo = scope.ServiceProvider.GetRequiredService<IGenericRepository<Reminder>>();
                INotificationService notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

                DateTime now = DateTime.UtcNow;
                // Get reminders where reminder date is today
                List<Reminder> reminders = (await reminderRepo.GetListAsync(
                    r => r.IsContinued && r.ReminderDate.Date == now.Date
                ));

                foreach (Reminder reminder in reminders)
                {
                    await notificationService.SyncReminderNotificationAsync(reminder);
                }
            }

            await Task.Delay(TimeSpan.FromHours(3), stoppingToken); // Runs every 3 Hours
        }
    }
}
