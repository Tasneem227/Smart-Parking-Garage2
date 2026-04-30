using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smart_Parking_Garage.Authentication.Filters;
using Smart_Parking_Garage.Contracts.Abstractions.Consts;
using Smart_Parking_Garage.Contracts.SensorReading;
using Smart_Parking_Garage.Services;
using System.Text.Json;

namespace Smart_Parking_Garage.Controllers;
[Route("api/[controller]")]
[ApiController]
public class SensorsController(ISensorService sensorService) : ControllerBase
{
    private readonly ISensorService _SensorService = sensorService;

    [HasPermission(Permissions.AddSensorsData)]
    [HttpPost("store")]
    public async Task<IActionResult> StoreData(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        using var stream = new StreamReader(file.OpenReadStream());
        var json = await stream.ReadToEndAsync();

        var data = JsonSerializer.Deserialize<ParkingDataJson>(json);
        await _SensorService.StoreParkingDataAsync(data);
        return Ok("Data stored successfully!");
    }

    [HasPermission(Permissions.UpdateSensorsData)]
    [HttpPost("update")]
    public async Task<IActionResult> UpdateSensor(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        using var stream = new StreamReader(file.OpenReadStream());
        var json = await stream.ReadToEndAsync();

        var data = JsonSerializer.Deserialize<ParkingDataJson>(json);

        await _SensorService.UpdateFromSensorAsync(data);
        return Ok("Updated successfully");
    }
}
