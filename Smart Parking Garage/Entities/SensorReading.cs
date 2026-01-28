namespace Smart_Parking_Garage.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


    public class SensorReading
{
        [Key]
        public int Id { get; set; }

        public DateTime Timestamp { get; set; }

        public double Temperature { get; set; }

        public int Humidity { get; set; }

        public int Gas { get; set; }

        public int TotalSlots { get; set; }

        public int OccupiedSlots { get; set; }

        public bool Slot1 { get; set; }

        public bool Slot2 { get; set; }

        public bool Slot3 { get; set; }

        public string EntryGate { get; set; }

        public string ExitGate { get; set; }
    }

