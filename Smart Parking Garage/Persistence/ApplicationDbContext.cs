using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Reflection;

namespace Smart_Parking_Garage.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
     IdentityDbContext<ApplicationUser, ApplicationRole, string>(options)
{
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Gate> Gates { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<ParkingSession> ParkingSessions { get; set; }
    public DbSet<ParkingSlot> ParkingSlots { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Sensor> Sensors { get; set; }
    public DbSet<Garage> Garages { get; set; }

    public DbSet<SensorReading> SensorsReadings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
         
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<ParkingSlot>()
         .HasOne(p => p.Sensor)
         .WithOne(s => s.ParkingSlot)
         .HasForeignKey<Sensor>(s => s.ParkingSlotId);


    }
}
