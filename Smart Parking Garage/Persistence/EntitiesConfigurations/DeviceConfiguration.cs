
namespace Smart_Parking_Garage.Persistence.EntitiesConfigurations;

public class DeviceConfiguration : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.HasKey(x => x.DeviceId);
    }
}
