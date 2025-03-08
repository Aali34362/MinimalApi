namespace Exotic.WebHook.Domain.DomainEvents;

public class WebHookRemoved : DomainEvent
{
    public WebHookRemoved()
    {

    }
    public Guid WebHookId { get; set; }
}
