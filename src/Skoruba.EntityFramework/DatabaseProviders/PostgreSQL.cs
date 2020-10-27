using System;
using System.Reflection;
using IdentityServer4.EntityFramework.Storage;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Skoruba.AuditLogging.EntityFramework.DbContexts;
using Skoruba.AuditLogging.EntityFramework.Entities;
using Skoruba.EntityFramework.Interfaces;
using Skoruba.EntityFramework.Shared.DbContexts;

namespace Skoruba.EntityFramework.PostgreSQL.Extensions
{
    public static class DatabaseExtensions
    {
        public static void RegisterNpgSqlDbContexts<TKey>(this IServiceCollection services, string connectionString)
            where TKey : IEquatable<TKey>
        {
            var migrationsAssembly = typeof(DatabaseExtensions).GetTypeInfo().Assembly.GetName().Name;

            // Config DB for identity
            services.AddDbContext<AdminIdentityDbContext<TKey>>(options =>
                options.UseNpgsql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));

            // Config DB from existing connection
            services.AddConfigurationDbContext<IdentityServerConfigurationDbContext>(options =>
                options.ConfigureDbContext = b =>
                    b.UseNpgsql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));

            // Operational DB from existing connection
            services.AddOperationalDbContext<IdentityServerPersistedGrantDbContext>(options => options.ConfigureDbContext = b =>
                b.UseNpgsql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));

            // Log DB from existing connection
            services.AddDbContext<AdminLogDbContext>(options => options.UseNpgsql(connectionString,
                optionsSql => optionsSql.MigrationsAssembly(migrationsAssembly)));

            // Audit logging connection
            services.AddDbContext<AdminAuditLogDbContext>(options => options.UseNpgsql(connectionString,
                optionsSql => optionsSql.MigrationsAssembly(migrationsAssembly)));

            // DataProtectionKey DB from existing connection
            if (!string.IsNullOrEmpty(connectionString))
                services.AddDbContext<IdentityServerDataProtectionDbContext>(options => options.UseNpgsql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));
        }
    }
}