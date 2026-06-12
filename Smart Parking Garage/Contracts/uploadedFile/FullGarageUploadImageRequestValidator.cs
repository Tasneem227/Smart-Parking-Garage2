using Smart_Parking_Garage.Settings;

namespace Smart_Parking_Garage.Contracts.uploadedFile;

public class FullGarageUploadImageRequestValidator:AbstractValidator<FullGarageUploadImageRequest>
{
    public FullGarageUploadImageRequestValidator()
    {

        RuleFor(x => x.DeviceId)
            .NotEmpty()
            .WithMessage("DeviceId is required.");

        RuleFor(x => x.CommandId)
            .Equal(103)
            .WithMessage("CommandId is not for uploading full garage image.");

        RuleFor(x => x.ImageType)
            .NotEmpty()
            .WithMessage("ImageType is required.")
            .Must(x => x.Equals("garage_full", StringComparison.OrdinalIgnoreCase))
            .WithMessage("ImageType must be 'garage_full'.");

        RuleFor(x => x.File)
            .NotNull()
            .WithMessage("Image file is required.");

        RuleFor(x => x.File)
            .Must(HaveValidSignature)
            .When(x => x.File is not null)
            .WithMessage("Only JPG, JPEG, and PNG files are allowed.");

        RuleFor(x => x.File.Length)
            .LessThanOrEqualTo(4 * 1024 * 1024)
            .When(x => x.File is not null)
            .WithMessage("File size must not exceed 4 MB.");
    }

    private static bool HaveValidSignature(IFormFile file)
    {
        using var binary = new BinaryReader(file.OpenReadStream());

        var bytes = binary.ReadBytes(8);
        var fileSignature = BitConverter.ToString(bytes);

        // Reject blocked signatures
        if (FileSettings.BlockedSignatures.Any(signature =>
                fileSignature.StartsWith(
                    signature,
                    StringComparison.OrdinalIgnoreCase)))
        {
            return false;
        }

        // Allow image signatures
        return FileSettings.AllowedImageSignatures.Any(signature =>
            fileSignature.StartsWith(
                signature,
                StringComparison.OrdinalIgnoreCase));
    }
}

