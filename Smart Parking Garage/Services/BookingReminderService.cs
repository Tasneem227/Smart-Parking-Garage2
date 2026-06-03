using Microsoft.EntityFrameworkCore;


namespace Smart_Parking_Garage.Services;

public class BookingReminderService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public BookingReminderService(IServiceScopeFactory scopeFactory)
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

            var now = DateTime.UtcNow;


            var bookings = await context.Bookings
                .Include(b => b.ParkingSlot)
                .Where(b => b.Status == "Active")
                .ToListAsync();

            foreach (var booking in bookings)
            {

                if (booking.BookingStart > now &&
                    booking.BookingStart.Subtract(now).TotalMinutes <= 15)
                {
                    await notificationService.SendAsync(
                        booking.ApplicationUserId,
                        "Reminder ⏰",
                        $"Your booking for Slot {booking.ParkingSlot.SlotNumber} in Garage {booking.GarageId} will start after {(int)Math.Ceiling((booking.BookingStart - now).TotalMinutes)} minutes",
                        "Booking"
                    );
                }


                if (booking.BookingEnd < now)
                {
                    booking.Status = "Completed";

                    await notificationService.SendAsync(
                        booking.ApplicationUserId,
                        "Booking Ended ⛔",
                        $"Your parking session for Slot {booking.ParkingSlot.SlotNumber} in Garage {booking.GarageId} has ended",
                        "Booking"
                    );
                }
            }


            await context.SaveChangesAsync();


            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}