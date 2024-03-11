using MinimalApi;
using MinimalApi.EndPoints;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
#if(UseSwagger)
builder.Services.AddSwaggerGen();
#endif
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
#if (UseSwagger)
    app.UseSwagger();
    app.UseSwaggerUI();
#endif
}

app.UseHttpsRedirection();

app.UseAuthorization();

//app.MapControllers();
//new UserEndPoint().MapEndPoints(app);
app.MapAllEndpoints();

app.Run();
