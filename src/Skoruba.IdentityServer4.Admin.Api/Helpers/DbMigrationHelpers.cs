using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.EntityFramework.Mappers;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Skoruba.AuditLogging.EntityFramework.DbContexts;
using Skoruba.AuditLogging.EntityFramework.Entities;
using Skoruba.Admin.EntityFramework.Interfaces;

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
