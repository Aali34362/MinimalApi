using Exotic.WebHook.Domain.EnumModels;

namespace Exotic.WebHook.Domain.Models;

public class WebHookRecord : BaseModel
{
    public WebHookRecord() { }

    public Guid WebHookID { get; set; }
    public WebHook WebHook { get; set; }
    public string Guid { get; set; }
    public HookEventType HookType { get; set; }
    public RecordResult Result { get; set; }
    public int StatusCode { get; set; }
    public string ResponseBody { get; set; }
    public string RequestBody { get; set; }
    public string RequestHeaders { get; set; }
    public string Exception { get; set; }
    public DateTime Timestamp { get; set; }
}

