namespace Smart_Parking_Garage.Abstractions.Consts;

public static class RegexPatterns
{
    public const string password = "(?=(.*[0-9]))(?=.*[\\!@#$%^&*()\\\\[\\]{}\\-_+=~`|:;\"'<>,./?])(?=.*[a-z])(?=(.*[A-Z]))(?=(.*)).{8,}";
    public const string phone = "^01[0-9]{9}$";
}

