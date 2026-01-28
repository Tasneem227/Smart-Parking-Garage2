namespace Smart_Parking_Garage.Entities;

public class Sensor
{
    public int SensorId { get; set; }
    public string Type { get; set; }   // Ultrasonic / Camera
    public int ParkingSlotId { get; set; }
    public string Status { get; set; } // Active / Offline
    public DateTime LastHeartbeat { get; set; }

    // Navigation
   
    public ParkingSlot? ParkingSlot { get; set; }
    //public ICollection<SensorLog> Logs { get; set; }
}
