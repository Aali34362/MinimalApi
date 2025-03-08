using Exotic.WebHook.Domain.EnumModels;
using Exotic.WebHook.Domain.Models;

namespace Exotic.WebHook.Domain.DomainEvents;

public class DomainEvent : BaseModel
{
    public Guid? ActorID { get; set; }
    public DateTime TimeStamp { get; set; }
    public EventType EventType { get; set; }
}
