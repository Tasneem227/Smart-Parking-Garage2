using Smart_Parking_Garage.Contracts.SensorReading;

namespace Smart_Parking_Garage.Services;

public interface ISensorService
{
    Task StoreParkingDataAsync(ParkingDataJson data );
    Task UpdateFromSensorAsync(ParkingDataJson data);

}
