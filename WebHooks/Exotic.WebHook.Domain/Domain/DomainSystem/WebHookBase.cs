namespace Exotic.WebHook.Domain.DomainSystem;

public interface IWebHookNotifyBase { }

public class WebHookNotifyBase<T, U>(T action) : IWebHookNotifyBase where T : System.Enum
{
    private T _action { get; set; } = action;
    public U? payload { get; set; }

    public string action
    {
        get { return _action.ToString(); }
    } 
    public Hook_User_DTO? actor { get; set; } = new Hook_User_DTO();

    public DateTime timeStamp { get; set; } = DateTime.Now;
}

[Serializable]
public class Hook_User_DTO
{
    public string? id { get; set; }
    public string? name { get; set; }
}
