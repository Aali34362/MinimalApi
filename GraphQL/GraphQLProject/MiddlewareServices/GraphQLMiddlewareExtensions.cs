using GraphQL;
using GraphQL.Types;

namespace GraphQLProject.MiddlewareServices;

public class GraphQLMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context, IDocumentExecuter executer, ISchema schema)
    {
        // Handle GraphQL Playground
       

        // Handle GraphQL endpoint
       

        // Pass to the next middleware if not handled
        await _next(context);
    }
}
public static class GraphQLMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomGraphQL(this IApplicationBuilder app)
    {
        return app.UseMiddleware<GraphQLMiddleware>();
    }
}
