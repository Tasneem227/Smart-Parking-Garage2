using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Smart_Parking_Garage.Entities;
using System.Reflection;

namespace Smart_Parking_Garage.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
     IdentityDbContext<ApplicationUser, ApplicationRole, string>(options)
{
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Gate> Gates { get; set; }
    public DbSet<Notification> Notifications { get; set; }
  
    public DbSet<ParkingSlot> ParkingSlots { get; set; }
    public DbSet<Payment> Payments { get; set; }
  
    public DbSet<Garage> Garages { get; set; }
    public DbSet<SensorReading> SensorsReadings { get; set; }
    public DbSet<UploadedImage> UploadedImages { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<EnvironmentReading> EnvironmentReadings { get; set; }
    public DbSet<DeviceCommand> DeviceCommands { get; set; }
    public DbSet<AlertLog> AlertLogs { get; set; }
    public DbSet<CarType> CarTypes { get; set; }
 
    public DbSet<MockCard> MockCards { get; set; }
    public DbSet<CapturedImage> CapturedImages { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
         
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<Gate>()
        .HasOne(x => x.Garage)
        .WithMany(x => x.Gates)
        .HasForeignKey(x => x.GarageId);

        modelBuilder.Entity<DeviceCommand>()
       .HasIndex(x => x.CommandId)
       .IsUnique();

        modelBuilder.Entity<MockCard>()
       .HasOne(c => c.ApplicationUser)
       .WithMany()
       .HasForeignKey(c => c.ApplicationUserId)
       .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Payment>()
            .HasOne(p => p.MockCard)
            .WithMany(c => c.Payments)
            .HasForeignKey(p => p.MockCardId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Payment>()
        .HasOne(p => p.Booking)
        .WithMany(b => b.Payments)
        .HasForeignKey(p => p.BookingId)
        .OnDelete(DeleteBehavior.NoAction);


    }
}
