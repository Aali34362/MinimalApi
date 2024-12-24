using GraphQL;
using GraphQL.Types;
using GraphQLProject.DatabaseConnections.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GraphQLProject.MiddlewareServices;

public class GraphQLMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
    {
        // Skip middleware for Swagger, Health Checks, or static files
        var path = context.Request.Path;
        if (path.StartsWithSegments("/swagger") ||
            path.StartsWithSegments("/swagger") ||
            path.StartsWithSegments("/health") ||
            path.StartsWithSegments("/static"))
        {
            await _next(context);
            return;
        }
        // Resolve the IConfiguration service
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var connectionString = configuration.GetConnectionString("DBConnection");
        if (string.IsNullOrEmpty(connectionString))
        {
            context.Response.StatusCode = 500; // Internal Server Error
            await context.Response.WriteAsync("Database connection is not configured.");
            return;
        }

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
