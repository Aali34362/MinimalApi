using WebHook.Api.Models;
using WebHook.Api.Repositories;
using WebHook.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddSingleton<InMemoryOrderRepository>();
builder.Services.AddSingleton<InMemoryWebHookSubscriptionRepository>();
builder.Services.AddHttpClient<WebhookDispatcher>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => {
        options.SwaggerEndpoint("/openapi/v1.json", "OpenApi V1");
    });
}

app.UseHttpsRedirection();

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
