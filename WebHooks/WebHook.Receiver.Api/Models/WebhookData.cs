using System.Text.Json;

namespace WebHook.Receiver.Api.Models;

public class WebhookData
{
    public Guid Id { get; set; }
    public string? UniqueId { get; set; }
    public string? Headers { get; set; } // Store as JSON string
    public string? Payload { get; set; }
    public DateTime Timestamp { get; set; }

    public Dictionary<string, string> GetHeadersDictionary()
    {
        return string.IsNullOrEmpty(Headers) ? new Dictionary<string, string>() : JsonSerializer.Deserialize<Dictionary<string, string>>(Headers)!;
    }

    public void SetHeadersDictionary(Dictionary<string, string> headers)
    {
        Headers = JsonSerializer.Serialize(headers);
    }
}
