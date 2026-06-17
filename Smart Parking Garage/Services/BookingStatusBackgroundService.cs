using Smart_Parking_Garage.Abstractions.Consts;

namespace Smart_Parking_Garage.Services;

public class BookingStatusBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<BookingStatusBackgroundService> _logger;

    public BookingStatusBackgroundService(
        IServiceScopeFactory scopeFactory,
        ILogger<BookingStatusBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();

                var context = scope.ServiceProvider
                    .GetRequiredService<ApplicationDbContext>();

                var now = DateTime.UtcNow;

                // Pending -> Active
                var activatedCount = await context.Bookings
                    .Where(b =>
                        b.Status == BookingStatuses.Pending &&
                        b.BookingStart <= now)
                    .ExecuteUpdateAsync(
                        s => s.SetProperty(
                            b => b.Status,
                            BookingStatuses.Active),
                        stoppingToken);

                // Active -> Completed
                var completedCount = await context.Bookings
                    .Where(b =>
                        b.Status == BookingStatuses.Active &&
                        b.BookingEnd <= now)
                    .ExecuteUpdateAsync(
                        s => s.SetProperty(
                            b => b.Status,
                            BookingStatuses.Completed),
                        stoppingToken);

                if (activatedCount > 0 || completedCount > 0)
                {
                    _logger.LogInformation(
                        "Bookings updated. Activated: {Activated}, Completed: {Completed}",
                        activatedCount,
                        completedCount);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating booking statuses");
            }

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}
