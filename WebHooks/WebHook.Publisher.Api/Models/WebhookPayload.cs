namespace WebHook.Publisher.Api.Models;

public class WebHookPayload
{
    public string? Event { get; set; }
    public string? Data { get; set; }
}
