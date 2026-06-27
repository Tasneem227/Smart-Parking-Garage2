using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Parking_Garage.Entities;

public class CapturedImage
{
    public int Id { get; set; }

    public int CommandId { get; set; } = default!;

    public string ImageUrl { get; set; } = default!;

    public DateTimeOffset CreatedAt { get; set; }
    public string? AnalysisJson { get; set; }

    [ForeignKey(nameof(CommandId))]
    public DeviceCommand? DeviceCommand { get; set; }
}