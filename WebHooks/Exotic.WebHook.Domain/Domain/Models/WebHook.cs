using Exotic.WebHook.Domain.EnumModels;

namespace Exotic.WebHook.Domain.Models;

public class WebHook : BaseModel
{
    public WebHook()
    {
        this.Headers = new HashSet<WebHookHeader>();
        this.HookEvents = new HookEventType[0];
        this.Records = new List<WebHookRecord>();
    }

    public string WebHookUrl { get; set; }
    public string? Secret { get; set; }
    public string ContentType { get; set; }
    public bool IsActive { get; set; }
    public HookEventType[] HookEvents { get; set; }
    virtual public HashSet<WebHookHeader> Headers { get; set; }
    virtual public ICollection<WebHookRecord> Records { get; set; }
    public DateTime? LastTrigger { get; set; }
}
