using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Smart_Parking_Garage.Entities;
using System.Reflection;

namespace Smart_Parking_Garage.Persistance;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
    IdentityDbContext<ApplicationUser>(options)
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
        
           // your actual table name
         
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ParkingSlot>()
         .HasOne(p => p.Sensor)
         .WithOne(s => s.ParkingSlot)
         .HasForeignKey<Sensor>(s => s.ParkingSlotId);


    }
}
