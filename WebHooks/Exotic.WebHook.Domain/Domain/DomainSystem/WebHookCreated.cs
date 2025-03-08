using Exotic.WebHook.Domain.EnumModels;

namespace Exotic.WebHook.Domain.DomainSystem;

public class Hook_HookCreated
    : WebHookNotifyBase<HookResourceAction, Hook_HookCreatedPayload>
{
    public Hook_HookCreated(HookResourceAction action) : base(action)
    {

    }
    public Hook_HookCreated( HookResourceAction action, Hook_User_DTO actor, Hook_HookCreatedPayload payload) : base(action)
    {
        this.actor = actor;
        this.payload = payload;
    }
}

[Serializable]
public class Hook_HookCreatedPayload
{
    // Add any custom payload hire
}