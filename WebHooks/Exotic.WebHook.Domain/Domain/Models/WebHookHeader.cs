namespace Exotic.WebHook.Domain.Models;

public class WebHookHeader : BaseModel
{
    public Guid WebHookID { get; set; }
    public WebHook? WebHook { get; set; }
    public string? Name { get; set; }
    public string? Value { get; set; }
    public DateTime CreatedTimestamp { get; set; }
}
