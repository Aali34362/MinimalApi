using cancellation_token_api.Models;
using cancellation_token_api.Response;
using MediatR;

namespace cancellation_token_api.EndPoints;

public static class UserEndpoints
{
    public static void MapUsersEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("users", async (
           CreateUserRequest request,
           ISender sender,
           CancellationToken cancellationToken) =>
        {
            var command = new CreateWorkoutRequest (request.Id, request.UserName,
            request.UserEmail,request.HasPublicProfile);
            Result<Guid> result = await sender.Send(command, cancellationToken);
            //return result.Match(Results.Ok, CustomResults.Problem);
            return Results.Ok;
        }).MapToApiVersion(1);


        app.MapPost("users/{userId}/follow.{followedId}", async (
           Guid userId,
           Guid followedId,
           ISender sender,
           CancellationToken cancellationToken) =>
        {
            var command = new StartFollwoingCommand(userId, followedId);
            Result result = await sender.Send(command, cancellationToken);
            //return result.Match(Results.NoContent, CustomResults.Problem);
            return Results.Ok;
        }).MapToApiVersion(1);

        app.MapGet("users/{userId}", async (
           Guid userId,
           ISender sender,
           CancellationToken cancellationToken) =>
        {
            var command = new GetUserByIdQuery(userId);
            Result<UserResponse> result = await sender.Send(command, cancellationToken);
            //return result.Match(Results.NoContent, CustomResults.Problem);
            return Results.Ok;
        });

        app.MapGet("users/{userId}/followers/stats", async (
           Guid userId,
           ISender sender,
           CancellationToken cancellationToken) =>
        {
            var command = new GetFollowerStatsQuery(userId);
            Result<FollowerStatsResponse> result = await sender.Send(command, cancellationToken);
            //return result.Match(Results.NoContent, CustomResults.Problem);
            return Results.Ok;
        });
    }

}
