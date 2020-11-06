using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using id4.Data;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.EntityFramework.DbContexts;
using Bluebird.Repositories;
using Bluebird.Repositories.EntityFramework;

namespace id4
{
    public static class StartupHelper
    {
        public static void NpgsqlMigrate(this IServiceProvider provider)
        {
            using (var scope = provider.CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<NpgsqlApplicationDbContext>())
                {
                    context.Database.Migrate();
                }
                using (var context = scope.ServiceProvider.GetRequiredService<NpgsqlConfigurationDbContext>())
                {
                    context.Database.Migrate();
                }
                using (var context = scope.ServiceProvider.GetRequiredService<NpgsqlPersistedGrantDbContext>())
                {
                    context.Database.Migrate();
                }
            }
        }
        public static void AddNpgsqlDbContexts(this IServiceCollection services, string connection)
        {
            // this is for migration
            services.AddDbContext<NpgsqlApplicationDbContext>(options =>
                options.UseNpgsql(connection, b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name)));
            services.AddDbContext<NpgsqlConfigurationDbContext>(options =>
                options.UseNpgsql(connection, b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name)));
            services.AddDbContext<NpgsqlPersistedGrantDbContext>(options =>
                options.UseNpgsql(connection, b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name)));

            // this is for all other services
            services.AddDbContext<ApplicationDbContext>(options =>
                //options.UseSqlite(connection));
                options.UseNpgsql(connection));
            services.AddDbContext<PersistedGrantDbContext>(options =>
                //options.UseSqlite(connection));
                options.UseNpgsql(connection));
            services.AddDbContext<ConfigurationDbContext>(options =>
                //options.UseSqlite(connection));
                options.UseNpgsql(connection));
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ClientRepo>();
        }
    }
}