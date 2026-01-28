using Microsoft.EntityFrameworkCore;
using Smart_Parking_Garage.Contracts.SensorReading;
using Smart_Parking_Garage.Persistance;
using System.Text.Json;

namespace Smart_Parking_Garage.Services;

public class SensorService(ApplicationDbContext context):ISensorService
{
    private readonly ApplicationDbContext _context = context;

    public async Task StoreParkingDataAsync(ParkingDataJson data)
    {

        var parkingData = new SensorReading
        {
            Timestamp = DateTime.Parse(data.Timestamp),
            Temperature = data.Temperature,
            Humidity = data.Humidity,
            Gas = data.Gas,
            TotalSlots = data.TotalSlots,
            OccupiedSlots = data.OccupiedSlots,
            Slot1 = data.Slot1,
            Slot2 = data.Slot2,
            Slot3 = data.Slot3,
            EntryGate = data.EntryGate,
            ExitGate = data.ExitGate
        };

        _context.SensorsReadings.Add(parkingData);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateFromSensorAsync(ParkingDataJson data)
    {

        var slot1 = await _context.ParkingSlots.FirstAsync(s => s.SlotNumber == "1");
        slot1.IsOccupied = data.Slot1;

        var slot2 = await _context.ParkingSlots.FirstAsync(s => s.SlotNumber == "2");
        slot2.IsOccupied = data.Slot2;

        var slot3 = await _context.ParkingSlots.FirstAsync(s => s.SlotNumber == "3");
        slot3.IsOccupied = data.Slot3;

        var entryGate = await _context.Gates.FirstAsync(g => g.GateType == "EntryGate");
        entryGate.Status = data.EntryGate;

        var exitGate = await _context.Gates.FirstAsync(g => g.GateType == "ExitGate");
        exitGate.Status = data.ExitGate;

        await _context.SaveChangesAsync();
    }
}
