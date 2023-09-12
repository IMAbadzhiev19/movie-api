using Microsoft.EntityFrameworkCore;
using Movie.WebHost.Database;

namespace Movie.WebHost.Extensions;

public static class MigrationExtension
{
    public static void ApplyMigration(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.MigrateAsync();
    }
}
