using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddOpenApi("openapi", options =>
{
    options.AddDocumentTransformer((document, context, cancToken) =>
    {
        document.Info.Title = "Dapr.EShop.Orders Open APi";
        document.Info.Version = "v1";
        document.Info.Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "ABC",
            Email = "abc@gmail.com"
        };
        return Task.CompletedTask;
    });

});


var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
        .WithTitle("Dapr.EShop.Orders Open APi")
        .WithDarkMode(true)
        .WithTheme(ScalarTheme.Moon)
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);

    });
}

app.UseHttpsRedirection();

app.MapPost("/process-order", async (OrderRequest request) => 
{
    await ProcessOrderAsync(request);
    return Results.Ok($"Order {request.OrderId} confirmation: #{Guid.NewGuid().ToString()[..8]}");
});

app.Run();


async Task ProcessOrderAsync(OrderRequest request)
{
    await Task.Delay(100);
}
public record OrderRequest(string OrderId, string CustomerId, List<string> Items);