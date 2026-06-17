using Smart_Parking_Garage.Settings;

namespace Smart_Parking_Garage.Contracts.uploadedFile;

public class UploadedImageRequestValidator:AbstractValidator<UploadedImageRequest>
{
    public UploadedImageRequestValidator()
    {
        RuleFor(x => x.Image)
            .NotNull()
            .WithMessage("Image file is required.");

        RuleFor(x => x.Image)
            .Must(HaveValidSignature)
            .When(x => x.Image is not null)
            .WithMessage("Only JPG, JPEG, and PNG files are allowed.");

        RuleFor(x => x.Image.Length)
            .LessThanOrEqualTo(4 * 1024 * 1024)
            .When(x => x.Image is not null)
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
