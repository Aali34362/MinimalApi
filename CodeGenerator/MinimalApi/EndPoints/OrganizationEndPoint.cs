namespace MinimalApi.EndPoints;

public class OrganizationEndPoint : IEndpoints
{
    public void MapEndPoints(IEndpointRouteBuilder endpointRoute)
    {
        endpointRoute.MapGet("organization", () => "name of organization");
        endpointRoute.MapPost("organization", () => Results.Ok("organization created"));
    }
}