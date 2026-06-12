namespace Smart_Parking_Garage.Entities;

public class Device
{
   
        public string DeviceId { get; set; } = default!;

        public int GarageId { get; set; } 

        public int SlotsCount { get; set; }

        public bool HasCamera { get; set; }

        public bool HasEnvSensors { get; set; }

        public bool HasEntryGate { get; set; }

        public bool HasExitGate { get; set; }

        public DateTimeOffset TimeStamp { get; set; }=DateTimeOffset.UtcNow;
        public Garage? Garage { get; set; }

        public ICollection<DeviceCommand> DeviceCommands { get; set; } = new List<DeviceCommand>();
       
}

