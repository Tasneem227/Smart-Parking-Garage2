namespace Smart_Parking_Garage.Persistence.EntitiesConfigurations;

public class UploadedFileConfiguration : IEntityTypeConfiguration<UploadedImage>
{
    public void Configure(EntityTypeBuilder<UploadedImage> builder)
    {
        builder.Property(x => x.ImageName).HasMaxLength(250);
        builder.Property(x => x.StoredImageName).HasMaxLength(250);
        builder.Property(x => x.ContentType).HasMaxLength(50);
        builder.Property(x => x.ImageExtension).HasMaxLength(10);
        builder.Property(x => x.ImageType).HasMaxLength(50);
    }

    
}
