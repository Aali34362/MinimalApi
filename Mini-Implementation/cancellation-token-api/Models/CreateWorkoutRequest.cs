namespace cancellation_token_api.Models;

public class CreateWorkoutRequest(Guid userid, string? name)
{
    public Guid UserId { get; set; } = userid;
    public string? Name { get; set; } = name;
}
