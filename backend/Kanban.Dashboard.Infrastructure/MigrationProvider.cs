using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Kanban.Dashboard.Infrastructure;

public class MigrationProvider
{
    public static async Task Migrate(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<MigrationProvider>>();

        logger.LogInformation("Checking database migrations...");
        await EnsureDatabaseCreated(logger, context)
            .ConfigureAwait(false);

        logger.LogInformation("Checking database health...");
        await DatabaseHealthCommand(context)
            .ConfigureAwait(false);
    }

    private static async Task EnsureDatabaseCreated(ILogger logger, DbContext context)
    {
        var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
        {
            logger.LogInformation("Applying {@Count} migrations...", pendingMigrations.Count());
            context.Database.SetCommandTimeout(TimeSpan.FromMinutes(5));
            await context.Database.MigrateAsync()
                .ConfigureAwait(false);
        }
    }

    private static async Task DatabaseHealthCommand(DbContext context)
    {
        await context.Database.ExecuteSqlRawAsync("SELECT 1")
            .ConfigureAwait(false);
    }
}