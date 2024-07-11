using Asp.Versioning;
using Asp.Versioning.Builder;
using cancellation_token_api.EndPoints;
using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

/*builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));*/

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/*builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);*/

//builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

//builder.Services.AddControllers();

builder.Services.AddApiVersioning( options => 
    { 
        options.DefaultApiVersion = new ApiVersion(1);
        //options.ReportApiVersions = true;
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
        //options.ApiVersionReader = ApiVersionReader.Combine
        //(
        //    new UrlSegmentApiVersionReader(),
        //    new HeaderApiVersionReader("X-ApiVersion")
        //);
    }).AddApiExplorer(options => 
    { 
        options.GroupNameFormat = "'v'V";
        options.SubstituteApiVersionInUrl = true;
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.ApplyMigrations();
}

app.UseHttpsRedirection();

app.MapHealthChecks("health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

//app.UseRequestContextLogging();
//app.UseSerilogRequestLogging();
//app.UseExceptionHandler();

app.MapGet("/long-running-request", async (CancellationToken cancellationToken) =>
{
    var randomId = Guid.NewGuid();
    var results = new List<string>();

    for (int i = 0; i < 100; i++)
    {
        if (cancellationToken.IsCancellationRequested)
            return Results.StatusCode(499);

        await Task.Delay(1000);
        var result = $"{randomId} - Result {i}";
        Console.WriteLine(result);
        results.Add(result);
    }
    return Results.Ok(results);
})
    .WithName("GetAllData")
    .WithOpenApi(); //https://github.com/dotnet/aspnet-api-versioning/issues/920

//app.MapPost("/upload-large-file", async (
//        [FromForm] FileUploadRequest request,
//        CancellationToken cancellationToken) =>
//{
//    try
//    {
//        var s3Client = new AmazonS3Client();
//        await s3Client.PutObjectAsync(new PutObjectRequest()
//        {
//            BucketName = "user-service-large-messages",
//            Key = $"{Guid.NewGuid()} - {request?.File?.FileName}",
//            InputStream = request?.File?.OpenReadStream()
//        }, cancellationToken);

//        return Results.NoContent();
//    }
//    catch (OperationCanceledException e)
//    {
//        await Console.Out.WriteLineAsync($"{e.InnerException} - {e.CancellationToken}");
//        return Results.StatusCode(499);
//    }
//})
//    .WithName("UploadLargeFile")
//    .DisableAntiforgery()
//    .WithOpenApi();

//app.UseAuthorization();

//app.MapControllers();

//app.MapUserEndpoints();

ApiVersionSet apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .HasApiVersion(new ApiVersion(2))
            //.HasDeprecatedApiVersion(new ApiVersion(1))
            .ReportApiVersions()
            .Build();

RouteGroupBuilder versionedGroup = app.MapGroup("api/v{apiVersion:apiVersion}").WithApiVersionSet(apiVersionSet);


//app.MapWorkoutEndpoints();
versionedGroup.MapWorkoutEndpoints();

app.Run();
