using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using IdentityServer4.EntityFramework.Storage;

using Skoruba.AspNetIdentity.EntityFramework;
using Skoruba.IdentityServer4.EntityFramework.DbContexts;

using Skoruba.EntityFramework.DbContexts;


namespace Skoruba.EntityFramework.PostgreSQL.Extensions
{
    public static class DatabaseExtensions
    {
        public static void AddNpgSqlDbContext<TDbContext>(this IServiceCollection services, string connectionString, string migrationsAssembly = null)
        where TDbContext : DbContext
        {
            if (migrationsAssembly != null)
                services.AddDbContext<TDbContext>(options =>
                    options.UseNpgsql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));
            else
                services.AddDbContext<TDbContext>(options => options.UseNpgsql(connectionString));
        }



        public static void RegisterNpgSqlDbContexts(this IServiceCollection services, string connectionString)
        {
            //var migrationsAssembly = typeof(DatabaseExtensions).GetTypeInfo().Assembly.GetName().Name;
            var migrationsAssembly = Assembly.GetCallingAssembly().GetName().Name;

            // Config DB for identity
            services.AddDbContext<AdminIdentityDbContext>(options =>
                options.UseNpgsql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));

            // Config DB from existing connection
            /*
            services.AddDbContext<AdminConfigurationDbContext>(options =>
                options.UseNpgsql(connectionString, b=>b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name) ));      
            services.AddDbContext<AdminPersistedGrantDbContext>(options =>
                options.UseNpgsql(connectionString, b=>b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name) ));  
                */

            // Config DB from existing connection
            services.AddConfigurationDbContext<AdminConfigurationDbContext>(options =>
                options.ConfigureDbContext = b =>
                    b.UseNpgsql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));

            // Operational DB from existing connection
            services.AddOperationalDbContext<AdminPersistedGrantDbContext>(options => options.ConfigureDbContext = b =>
                b.UseNpgsql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));

            // Log DB from existing connection
            services.AddDbContext<AdminLogDbContext>(options => options.UseNpgsql(connectionString,
                optionsSql => optionsSql.MigrationsAssembly(migrationsAssembly)));

            // Audit logging connection
            services.AddDbContext<AdminAuditLogDbContext>(options => options.UseNpgsql(connectionString,
                optionsSql => optionsSql.MigrationsAssembly(migrationsAssembly)));

            services.AddDbContext<DataProtectionDbContext>(options => options.UseNpgsql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));
        }
    }
}
