using Exotic.WebHook.Domain.DomainEvents;

namespace Exotic.WebHook.Domain.DomainEvents;

public class WebHookCreated : DomainEvent
{
    public WebHookCreated()
    {

    }
    public Guid WebHookId { get; set; }
}
