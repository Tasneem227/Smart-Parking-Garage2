using Microsoft.EntityFrameworkCore;


namespace Smart_Parking_Garage.Services;

public class BookingReminderService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<BookingReminderService> _Logger;

    public BookingReminderService(IServiceScopeFactory scopeFactory,ILogger<BookingReminderService> logger)
    {
        _scopeFactory = scopeFactory;
        _Logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();

                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

                var now = DateTime.UtcNow;


                var bookings = await context.Bookings
                    .Include(b => b.ParkingSlot)
                    .Where(b => b.Status == "Pending")
                    .Include(G => G.Garage)
                    .ToListAsync(stoppingToken);

                foreach (var booking in bookings)
                {
                    var minutesLeft = (booking.BookingStart - now).TotalMinutes;

                    if (!booking.ReminderSent &&
                        minutesLeft <= 15 &&
                        minutesLeft > 0)
                    {
                        await notificationService.SendAsync(

                           booking.ApplicationUserId,
                            "Reminder ⏰",
                            $"Your booking for Slot {booking.ParkingSlot.SlotNumber} in Garage {booking.Garage.Name} will start after 15 minutes",
                            "Booking"
                        );

                        booking.ReminderSent = true;
                        _Logger.LogInformation(
                           "Reminder sent for Booking {BookingId}",
                           booking.BookingId);
                    }
                    var slot = await context.ParkingSlots.FirstOrDefaultAsync(x =>x.ParkingSlotId == booking.ParkingSlotId);
                    if (booking.BookingEnd < now)
                    {
                        slot.IsOccupied = false;
                        await notificationService.SendAsync(
                            booking.ApplicationUserId,
                            "Booking Ended ⛔",
                            $"Your parking session for Slot {booking.ParkingSlot.SlotNumber} in Garage {booking.Garage.Name} has ended",
                            "Booking"
                        );
                    }
                }

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _Logger.LogError(
                    ex,
                    "Error occurred while processing booking reminders");
            }


            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}