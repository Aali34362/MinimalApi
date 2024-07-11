namespace cancellation_token_api.Models;

public class StartFollwoingCommand(Guid UserId, Guid FollowedId)
{
    public Guid userId { get; set; } = UserId;
    public Guid followedId { get; set; } = FollowedId;
}
