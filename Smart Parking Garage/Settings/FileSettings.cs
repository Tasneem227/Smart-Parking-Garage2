using static System.Net.WebRequestMethods;

namespace Smart_Parking_Garage.Settings;

public static class FileSettings
{
    public const int MaxFileSizeInMB = 1;
    public const string BaseUrl = "https://smartparkinggaragesystem.runasp.net/Uploads/Images/";
    public const int MaxFileSizeInBytes = MaxFileSizeInMB * 1024 * 1024;
    public static readonly string[] BlockedSignatures = ["4D-5A"  ,     //.exe
                                                        "2F-2A" ,      //.js
                                                        "D0-CF"];     // .msi
    public static readonly string[] AllowedImageSignatures =
            [
                "FF-D8-FF",      // JPEG
                "89-50-4E-47"   // PNG
            ];
}
