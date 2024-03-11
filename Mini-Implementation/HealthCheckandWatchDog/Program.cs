using Scrutor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
/*
https://andrewlock.net/using-scrutor-to-automatically-register-your-services-with-the-asp-net-core-di-container/
builder .Services .Scan() => Scrutor Package.
*/
/*builder
    .Services
    .Scan(
        selector => selector
        .FromAssemblies(
               Gatherly.Infrastructure.AssemblyReference.Assembly,
               Gatherly.Persistence.AssemblyReference.Assembly)
        .AddClasses(false)
        .UsingRegistrationStrategy(RegistrationStrategy.Skip)
        .AsImplementedInterfaces()
        .WithScopedLifetime());*/

//builder.Services.AddMemoryCache();


builder.Services.AddStackExchangeRedisCache(redisOptions => {
    string connection = builder.Configuration.GetConnectionString("Redis");
            redisOptions.Configuration = connection;
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapHealthChecks("health");
app.UseAuthorization();

app.MapControllers();

app.Run();
