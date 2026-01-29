namespace Smart_Parking_Garage.Contracts.Garage;

public class GarageSlotsStatus
{
    public int GarageId { get; set; }
    public int TotalSlots { get; set; }
    public int AvailableSlots { get; set; }
    public List<SlotStatus> Slots { get; set; }
}
