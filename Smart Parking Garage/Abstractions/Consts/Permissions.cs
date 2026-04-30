namespace Smart_Parking_Garage.Contracts.Abstractions.Consts;

public class Permissions
{
    public static string Type { get; } = "permissions";

    public const string GetBookings = "Bookings:read";
    public const string GetBookingById = "Bookings:readById";
    public const string AddBookings = "Bookings:add";
    public const string UpdateBookings = "Bookings:update";
    public const string DeleteBookingById = "Bookings:deleteById";
    public const string DeleteBookingsByUserId = "Bookings:deleteByUserId";

    public const string SendChatbotMessage = "Chatbot:send";

    public const string GetGarages = "Garages:read";
    public const string GetGarageById = "Garage:read";
    public const string GetGaragesStatus = "Garages:readstatus";
    public const string AddGarages = "Garages:add";
    public const string UpdateGarages = "Garages:update";
    public const string DeleteGarages = "Garages:delete";

    public const string GetGates = "Gates:read";
    public const string UpdateGatesStatus = "Gates:updatestatus";
    public const string AddGates = "Gates:add";
    public const string UpdateGates = "Gates:update";
    public const string DeleteGates = "Gates:delete";

    public const string GetParkingSlots = "ParkingSlots:read";
    public const string GetParkingSlotsById = "ParkingSlots:readById";
    public const string AddParkingSlots = "ParkingSlots:add";
    public const string UpdateParkingSlots = "ParkingSlots:update";
    public const string DeleteParkingSlots = "ParkingSlots:delete";

    public const string AddSensorsData = "SensorsData:add";
    public const string UpdateSensorsData = "SensorsData:update";

    public const string GetUsers = "users:read";
    public const string AddUsers = "users:add";
    public const string UpdateUsers = "users:update";

    public const string GetRoles = "roles:read";
    public const string AddRoles = "roles:add";
    public const string UpdateRoles = "roles:update";


    public static IList<string?> GetAllPermissions() =>
        typeof(Permissions).GetFields().Select(x => x.GetValue(x) as string).ToList();
}
