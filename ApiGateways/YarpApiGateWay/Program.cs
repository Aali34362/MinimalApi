using Scalar.AspNetCore;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi("openapi", options =>
{
    options.AddDocumentTransformer((document, context, cancToken) =>
    {
        document.Info.Title = "Yarp Api Gateway Open APi";
        document.Info.Version = "v1";
        document.Info.Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "ABC",
            Email = "abc@gmail.com"
        };
        return Task.CompletedTask;
    });

});

// Load main appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Load separate reverse proxy configs
builder.Configuration.AddJsonFile("appsettings.Orders.json", optional: true, reloadOnChange: true);
builder.Configuration.AddJsonFile("appsettings.Checkout.json", optional: true, reloadOnChange: true);

// Load reverse proxy configs dynamically
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddRateLimiter(options => {
    options.AddPolicy("FixedWindow", context =>
    RateLimitPartition.GetFixedWindowLimiter(
    partitionKey: context.Request.Path, 
    factory: _ => new FixedWindowRateLimiterOptions()
    {
        AutoReplenishment = true,
        PermitLimit = 10,
        Window = TimeSpan.FromSeconds(10)
    }));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
        .WithTitle("Yarp Api Gateway Open APi")
        .WithDarkMode(true)
        .WithTheme(ScalarTheme.Moon)
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);

    });
}

app.MapReverseProxy();

app.Run();
