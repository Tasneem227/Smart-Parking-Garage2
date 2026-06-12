public class DeviceCommand
{
    public int Id { get; set; }

    public string CommandId { get; set; } = default!;

    public string CommandType { get; set; } = default!;

    public string Status { get; set; } = "pending";

    public string? Error { get; set; }

    public string? DeviceId { get; set; }

    public int RetryCount { get; set; }

    // وقت وصول ACK
    public DateTimeOffset? TimeStamp { get; set; }

    // وقت آخر إرسال للأمر
    public DateTimeOffset LastSentAt { get; set; }

    public Device? Device { get; set; }
}