namespace Exotic.WebHook.Domain.EnumModels;

public enum RecordResult
{
    undefined = 0,
    ok,
    parameter_error,
    http_error,
    dataQueryError
}

public enum HookEventType
{
    hook, //(Hook created, Hook deleted ...)
    file, // (Some file uploaded, file deleted)
    note, // (Note posted, note updated)
    project, // (Some project created, project disabled)
    milestone // (Milestone created, milestone is done etc...)
}

// Actions of HookEventType
public enum HookResourceAction
{
    undefined,
    hook_created,
    hook_removed,
    hook_updated,
    // etc...
}

// Actions of ProjectEventType
public enum projectAction
{
    undefined,
    project_created,
    project_renamed,
    project_archived,
    //etc...
}

public enum EventType
{
    WebHook,
    System,
    Project,
}

public enum HookGrou2Action
{
    undefined,
    something_happened,
}

public enum HookGrou3Action
{
    undefined,
    something_else_happened
}