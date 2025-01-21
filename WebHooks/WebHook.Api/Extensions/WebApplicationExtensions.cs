using Microsoft.EntityFrameworkCore;
using WebHook.Api.Data;


namespace WebHook.Api.Extensions;

public static class WebApplicationExtensions
{
    public static async Task ApplyMigrationAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<WebHooksDbContext>();
        await db.Database.MigrateAsync();
    }
}
