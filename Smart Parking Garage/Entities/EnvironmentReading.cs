using Microsoft.Extensions.Options;
using System;
using System.Threading.Channels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Smart_Parking_Garage.Entities;
public class EnvironmentReading
{
    public int Id { get; set; }

    public string DeviceId { get; set; } = default!;

    public decimal? Temperature { get; set; }

    public decimal? Humidity { get; set; }

    public bool? Gas { get; set; }

    public DateTimeOffset Timestamp { get; set; }= DateTimeOffset.UtcNow;

    public Device Device { get; set; } = default!;
}