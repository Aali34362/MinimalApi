using cancellation_token_api.Models;
using MediatR;
using Asp.Versioning;
using Asp.Versioning.Builder;

namespace cancellation_token_api.EndPoints;

public static class WorkoutEndpoints
{
    public static void MapWorkoutEndpoints(this IEndpointRouteBuilder app)
    {
        //Third Version of Api Versioning
        app.MapPost("workouts", async (
           CreateWorkoutRequest request,
           ISender sender,
           CancellationToken cancellationToken) =>
        {
            var command = new CreateWorkoutRequest(request.UserId, request.Name);
            Result<Guid> result = await sender.Send(command, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
            //return result.Match(Results.Ok);
            //return Results.Ok;
        });

        //Second Version of Api Versioning
        /*ApiVersionSet apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            //.HasDeprecatedApiVersion(new ApiVersion(1))
            .ReportApiVersions()
            .Build() ;

        RouteGroupBuilder groupBuilder = app.MapGroup("api/v{apiVersion:apiVersion}").WithApiVersionSet(apiVersionSet);

        groupBuilder.MapPost("workouts", async (
            CreateWorkoutRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new CreateWorkoutRequest(request.UserId, request.Name);
            // Results<Guid> result = await sender.Send(command, cancellationToken);
            //return result.Match(Results.Ok, CustomResults.Problem);
            return Results.Ok;
        });*/

        //First Version of Api Versioning
        //app.MapPost("api/v{apiVersion:apiVersion}/workouts", async (
        //    CreateWorkoutRequest request,
        //    ISender sender,
        //    CancellationToken cancellationToken) =>
        //{
        //    var command = new CreateWorkoutRequest(request.UserId, request.Name);
        //    Results<Guid> result = await sender.Send(command, cancellationToken);
        //    return result.Match(Results.Ok, CustomResults.Problem);
        //}).WithApiVersionSet(apiVersionSet);
    }
}
