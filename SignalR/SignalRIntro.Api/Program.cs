using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using Scalar.AspNetCore;
using SignalRIntro.Api.Interfaces;
using SignalRIntro.Api.SignalRHub;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi("openapi", options =>
{
    options.AddDocumentTransformer((document, context, cancToken) =>
    {
        document.Info.Title = "Signal_R Intro Open APi";
        document.Info.Version = "v1";
        document.Info.Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "ABC",
            Email = "abc@gmail.com"
        };
        return Task.CompletedTask;
    });
});

builder.Services.AddSignalR();
builder.Logging.AddConsole().SetMinimumLevel(LogLevel.Debug);
var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
        .WithTitle("Signal_R Intro Open APi")
        .WithDarkMode(true)
        .WithTheme(ScalarTheme.Moon)
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.MapPost("broadcast", async (string message, IHubContext<ChatHub, IChatClient> context) =>
{
    try
    {
        await context.Clients.All.ReceiveMessage(message);
    }
    catch(Exception ex)
    {
        Console.WriteLine($"Exception : {ex.Message}");
    }
    return Results.NoContent();
});

app.UseHttpsRedirection();

app.MapHub<ChatHub>("chat-hub");

app.Run();
