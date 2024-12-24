using GraphiQl;
using GraphQL.Server.Ui.Playground;
using GraphQL.Types;
using GraphQLProject.ConfigureServices;
using GraphQLProject.MiddlewareServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDependencyServices();
builder.Services.AddGraphQLServices();
builder.Services.AddDbConnectionServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCustomGraphQL();

app.UseHttpsRedirection();

////app.UseGraphiQl("/graphql");
// Add middleware for GraphQL Playground
app.UseGraphQLPlayground("/ui/playground", new PlaygroundOptions
{
    GraphQLEndPoint = "/graphql"
});
app.UseGraphQL<ISchema>();

// Use the custom GraphQL middleware
////app.UseCustomGraphQL();


app.UseAuthorization();

app.MapControllers();

app.Run();
