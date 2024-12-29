using Scalar.AspNetCore;
using URL_Shortener.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddNpgsqlDataSource("url-shortener");
builder.AddRedisDistributedCache("redis");

#pragma warning disable EXTEXP0018 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
builder.Services.AddHybridCache();
#pragma warning restore EXTEXP0018 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

builder.Services.AddOpenApi("openapi", options =>
{
    options.AddDocumentTransformer((document, context, cancToken) =>
    {
        document.Info.Title = "Url Shortener Open APi";
        document.Info.Version = "v1";
        document.Info.Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "ABC",
            Email = "abc@gmail.com"
        };
        return Task.CompletedTask;
    });

});

builder.Services.AddHostedService<DatabaseInitializer>();
builder.Services.AddScoped<UrlShorteningService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddOpenTelemetry()
    .WithMetrics(metrics => metrics.AddMeter("UrlShortening.Api"));


var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
        .WithTitle("Url Shortener Open APi")
        .WithDarkMode(true)
        .WithTheme(ScalarTheme.Moon)
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);

    });
}

app.UseHttpsRedirection();

app.MapPost("shorten", async (string url, UrlShorteningService urlShorteningService) =>
{
    if (Uri.TryCreate(url, UriKind.Absolute, out _))
        return Results.BadRequest("Invalid Url format");

    var shortCode = await urlShorteningService.ShortenUrl(url);

    return Results.Ok(new { shortCode });
});

app.MapGet("{shortCode}", async (string shortCode, UrlShorteningService urlShorteningService) =>
{
    return Results.Redirect(await urlShorteningService.GetOriginalUrl(shortCode)) ?? Results.NotFound();
});

app.MapGet("urls",async (UrlShorteningService urlShorteningService) =>
{
    return Results.Ok(await urlShorteningService.GetAllUrls()) ?? Results.NotFound();
});

app.Run();
