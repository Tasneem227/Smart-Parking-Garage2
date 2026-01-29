namespace Smart_Parking_Garage.Contracts.Garage;

public class SlotStatus
{
    public int SlotId { get; set; }
    public string Slotnumber { get; set; }
    public string Status { get; set; }
    public decimal? SalaryPerHour { get; set; }
}
