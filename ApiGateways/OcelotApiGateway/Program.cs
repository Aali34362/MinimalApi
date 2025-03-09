using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using OcelotApiGateway;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi("openapi", options =>
{
    options.AddDocumentTransformer((document, context, cancToken) =>
    {
        document.Info.Title = "Ocelot Api Gateway Open APi";
        document.Info.Version = "v1";
        document.Info.Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "ABC",
            Email = "abc@gmail.com"
        };
        return Task.CompletedTask;
    });

});

builder.Configuration.AddOcelotConfigs();

builder.Configuration
    .AddJsonFile("ocelot.json", optional: true, reloadOnChange: true);

builder.Services.AddOcelot();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
        .WithTitle("Ocelot Api Gateway Open APi")
        .WithDarkMode(true)
        .WithTheme(ScalarTheme.Moon)
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);

    });
}

await app.UseOcelot();

app.Run();
