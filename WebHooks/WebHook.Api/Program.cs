using AspNetCoreRateLimit;
using Microsoft.AspNetCore.RateLimiting;
using Scalar.AspNetCore;
using System.Collections.Concurrent;
using System.Threading.RateLimiting;
using WebHook.Api.Models;
using WebHook.Api.Repositories;
using WebHook.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi("openapi", options =>
{
    options.AddDocumentTransformer((document, context, cancToken) =>
    {
        document.Info.Title = "Web Hook Open APi";
        document.Info.Version = "v1";
        document.Info.Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "ABC",
            Email = "abc@gmail.com"
        };
        return Task.CompletedTask;
    });

});

builder.Services.AddSingleton<InMemoryOrderRepository>();
builder.Services.AddSingleton<InMemoryWebHookSubscriptionRepository>();
builder.Services.AddHttpClient<WebhookDispatcher>();

//Fixed window limiter
builder.Services.AddRateLimiter(_ => _
    .AddFixedWindowLimiter(policyName: "fixed", options =>
    {
        options.PermitLimit = 4;
        options.Window = TimeSpan.FromSeconds(12);
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 2;
    }));

// Configure services
////builder.Services.AddRateLimiter(options =>
////{
////    options.OnRejected = (context, _) =>
////    {
////        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
////        return new ValueTask();
////    };
////});

// Tenant-specific rate limit data
var tenantRateLimits = new ConcurrentDictionary<string, int>
{
    ["Tenant1"] = 100, // Premium
    ["Tenant2"] = 10   // Basic
};



var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    ////app.UseSwaggerUI(options => {
    ////    options.SwaggerEndpoint("/openapi/v1.json", "OpenApi V1");
    ////});
    app.MapScalarApiReference(options =>
    {
        options
        .WithTitle("Web Hooks Open APi")
        .WithDarkMode(true)
        .WithTheme(ScalarTheme.Moon)
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
        
    });
}

app.UseHttpsRedirection();
app.UseRateLimiter();
// Add rate limiter middleware
////app.UseRateLimiter(options =>
////{
////    options.AddPolicy("TenantRateLimiter", context =>
////    {
////        // Get tenant ID from request (e.g., header, token, or API key)
////        var tenantId = context.HttpContext.Request.Headers["Tenant-ID"].ToString();

////        // Default limit if tenant is not found
////        int rateLimit = tenantRateLimits.GetValueOrDefault(tenantId, 10);

////        return RateLimitPolicy.FixedWindow(rateLimit, TimeSpan.FromMinutes(1));
////    });
////    options.OnRejected = async (context, cancellationToken) =>
////    {
////        var tenantId = context.HttpContext.Request.Headers["Tenant-ID"];
////        await context.HttpContext.Response.WriteAsync(
////            $"Tenant '{tenantId}' has exceeded their rate limit. Upgrade your subscription for more requests.", cancellationToken);
////    };
////});

static string GetTicks() => (DateTime.Now.Ticks & 0x11111).ToString("00000");

app.MapGet("/", () => Results.Ok($"Hello {GetTicks()}"))
                           .RequireRateLimiting("fixed");

//Create an order
app.MapPost("/orders", async (
    CreateOrderRequest request,
    InMemoryOrderRepository orderRepository,
    WebhookDispatcher webhookDispatcher
    ) =>
{
    var order = new Orders(Guid.NewGuid(), request.CustomerName, request.Amount, DateTime.Now);
    orderRepository.Add(order);
    await webhookDispatcher.DispatchAsync("order.created", order);
    return Results.Ok(order);
}).WithTags("Orders");

//Get all orders
app.MapGet("/orders", (InMemoryOrderRepository orderRepository) => 
{
    return Results.Ok(orderRepository.GetAll());
}).WithTags("Orders");

app.MapPost("webhooks/subscriptions", (
    CreateWebHookRequest request,
    InMemoryWebHookSubscriptionRepository webHookRepository) =>
{
    var webHook = new WebHookSubscription(Guid.NewGuid(), request.EventType, request.WebHookUrl, DateTime.UtcNow);
    webHookRepository.Add(webHook);
    return Results.Ok(webHook);
}).WithTags("Orders");

app.Run();
