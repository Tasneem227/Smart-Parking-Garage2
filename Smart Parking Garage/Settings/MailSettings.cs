namespace Smart_Parking_Garage.Settings;

public class MailSettings
{
    public string UserName { get; set; }=string.Empty;
    public bool EnableSSL { get; set; }
    public string host { get; set; }=string.Empty;
    public int port { get; set; }
    public string password { get; set; }=string.Empty;
    
}
