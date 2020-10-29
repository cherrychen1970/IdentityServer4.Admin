using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Skoruba.Admin.Api.Helpers
{
    public static class DbMigrationHelpers
    {
        public static void EnsureDatabasesMigrate<TDbContext>(this IServiceProvider services)
            where TDbContext : DbContext
        {
            using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<TDbContext>())
                {
                    context.Database.Migrate();
                }
            }
        }
     }
}
