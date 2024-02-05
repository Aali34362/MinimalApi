
namespace MinimalApi.EndPoints;

public class UserEndPoint : IEndpoints
{
    public void MapEndPoints(IEndpointRouteBuilder endpointRoute)
    {
        endpointRoute.MapGet("users", () => "list of users");
        endpointRoute.MapPost("users", () => Results.Ok("user created"));
    }
}
