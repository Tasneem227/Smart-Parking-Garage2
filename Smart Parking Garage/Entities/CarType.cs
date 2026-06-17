using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Parking_Garage.Entities;

public class CarType
{
    public int Id { get; set; }
    public string carType { get; set; }
    public string UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public ApplicationUser? ApplicationUser { get; set; }
}
