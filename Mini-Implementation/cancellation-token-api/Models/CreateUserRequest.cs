using System.ComponentModel;

namespace cancellation_token_api.Models;

public class CreateUserRequest
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    [DefaultValue("false")]
    public bool HasPublicProfile { get; set; }
}
