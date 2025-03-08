namespace Exotic.WebHook.Domain.DomainEvents;

public class WebHookUpdated : DomainEvent
{
    public WebHookUpdated()
    {

    }
    public Guid WebHookId { get; set; }
}
