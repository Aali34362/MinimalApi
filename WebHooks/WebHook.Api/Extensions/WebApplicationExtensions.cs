using Microsoft.EntityFrameworkCore;
using WebHooks.Data;

namespace WebHooks.Extensions;

public static class WebApplicationExtensions
{
    public static async Task ApplyMigrationAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<WebHookDbContext>();
        await db.Database.MigrateAsync();
    }
}
