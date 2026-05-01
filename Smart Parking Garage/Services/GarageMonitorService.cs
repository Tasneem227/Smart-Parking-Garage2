using Microsoft.EntityFrameworkCore;


namespace Smart_Parking_Garage.Services;

public class GarageMonitorService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    private bool _allGaragesWereFull = false;
    private bool _hasSentAvailableNotification = false;

    public GarageMonitorService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

            var garages = await context.Garages.ToListAsync();

            bool allFull = garages.All(g => g.AvailableSlots == 0);

            // 🟥 كل الجراجات بقت FULL
            if (allFull && !_allGaragesWereFull)
            {
                var users = await context.Users.ToListAsync();

                foreach (var user in users)
                {
                    await notificationService.SendAsync(
                        user.Id,
                        "Garage Alert 🚨",
                        "All garages in the system are now FULL 🚫",
                        "System"
                    );
                }

                _allGaragesWereFull = true;
                _hasSentAvailableNotification = false; // reset
            }

            // 🟢 أول مرة يظهر مكان بعد ما كانوا FULL
            if (_allGaragesWereFull && !allFull && !_hasSentAvailableNotification)
            {
                var availableGarage = garages.FirstOrDefault(g => g.AvailableSlots > 0);

                if (availableGarage != null)
                {
                    var users = await context.Users.ToListAsync();

                    foreach (var user in users)
                    {
                        await notificationService.SendAsync(
                            user.Id,
                            "Garage Available 🟢",
                            $"{availableGarage.Name}now has available spaces",
                            "System"
                        );
                    }
                }

                _hasSentAvailableNotification = true;
                _allGaragesWereFull = false;
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}