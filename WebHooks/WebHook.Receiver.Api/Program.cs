using Microsoft.EntityFrameworkCore;
using WebHook.Receiver.Api.Context;
using WebHook.Receiver.Api.HubService;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<WebhookReceiverContext>(options =>
    options.UseInMemoryDatabase("WebhookDB"));
builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin() // Allow all origins
               .AllowAnyMethod() // Allow all HTTP methods (GET, POST, etc.)
               .AllowAnyHeader(); // Allow all headers
    });
});

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use CORS policy
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.MapHub<WebhookHub>("/webhookHub");

app.Run();
