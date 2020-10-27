using System.Reflection;
using IdentityServer4.EntityFramework.Storage;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Skoruba.AuditLogging.EntityFramework.DbContexts;
using Skoruba.AuditLogging.EntityFramework.Entities;
using Skoruba.IdentityServer4.Admin.EntityFramework.Interfaces;

namespace SkorubaIdentityServer4Admin.Admin.EntityFramework.MySql.Extensions
{
    public static class DatabaseExtensions
    {
        /// <summary>
        /// Register DbContexts for IdentityServer ConfigurationStore and PersistedGrants, Identity and Logging
        /// Configure the connection strings in AppSettings.json
        /// </summary>
        /// <typeparam name="IdentityServerConfigurationDbContext"></typeparam>
        /// <typeparam name="IdentityServerPersistedGrantDbContext"></typeparam>
        /// <typeparam name="TLogDbContext"></typeparam>
        /// <typeparam name="AdminIdentityDbContext"></typeparam>
        /// <typeparam name="TAuditLoggingDbContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="identityConnectionString"></param>
        /// <param name="configurationConnectionString"></param>
        /// <param name="persistedGrantConnectionString"></param>
        /// <param name="errorLoggingConnectionString"></param>
        /// <param name="auditLoggingConnectionString"></param>
        public static void RegisterMySqlDbContexts<AdminIdentityDbContext, IdentityServerConfigurationDbContext,
            IdentityServerPersistedGrantDbContext, TLogDbContext, TAuditLoggingDbContext, IdentityServerDataProtectionDbContext>(this IServiceCollection services,
            string identityConnectionString, string configurationConnectionString,
            string persistedGrantConnectionString, string errorLoggingConnectionString,
            string auditLoggingConnectionString, string dataProtectionConnectionString = null)
            where AdminIdentityDbContext : DbContext
            where IdentityServerPersistedGrantDbContext : DbContext, IAdminPersistedGrantDbContext
            where IdentityServerConfigurationDbContext : DbContext, IAdminConfigurationDbContext
            where TLogDbContext : DbContext, IAdminLogDbContext
            where TAuditLoggingDbContext : DbContext, IAuditLoggingDbContext<AuditLog>
            where IdentityServerDataProtectionDbContext : DbContext, IDataProtectionKeyContext
        {
            var migrationsAssembly = typeof(DatabaseExtensions).GetTypeInfo().Assembly.GetName().Name;

            // Config DB for identity
            services.AddDbContext<AdminIdentityDbContext>(options =>
                options.UseMySql(identityConnectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));

            // Config DB from existing connection
            services.AddConfigurationDbContext<IdentityServerConfigurationDbContext>(options =>
                options.ConfigureDbContext = b =>
                    b.UseMySql(configurationConnectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));

            // Operational DB from existing connection
            services.AddOperationalDbContext<IdentityServerPersistedGrantDbContext>(options => options.ConfigureDbContext = b =>
                b.UseMySql(persistedGrantConnectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));

            // Log DB from existing connection
            services.AddDbContext<TLogDbContext>(options => options.UseMySql(errorLoggingConnectionString,
                optionsSql => optionsSql.MigrationsAssembly(migrationsAssembly)));

            // Audit logging connection
            services.AddDbContext<TAuditLoggingDbContext>(options => options.UseMySql(auditLoggingConnectionString,
                optionsSql => optionsSql.MigrationsAssembly(migrationsAssembly)));

            // DataProtectionKey DB from existing connection
            if(!string.IsNullOrEmpty(dataProtectionConnectionString))
                services.AddDbContext<IdentityServerDataProtectionDbContext>(options => options.UseMySql(dataProtectionConnectionString,
                    optionsSql => optionsSql.MigrationsAssembly(migrationsAssembly)));
        }

        /// <summary>
        /// Register DbContexts for IdentityServer ConfigurationStore and PersistedGrants and Identity
        /// Configure the connection strings in AppSettings.json
        /// </summary>
        /// <typeparam name="IdentityServerConfigurationDbContext"></typeparam>
        /// <typeparam name="IdentityServerPersistedGrantDbContext"></typeparam>
        /// <typeparam name="AdminIdentityDbContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="identityConnectionString"></param>
        /// <param name="configurationConnectionString"></param>
        /// <param name="persistedGrantConnectionString"></param>
        public static void RegisterMySqlDbContexts<AdminIdentityDbContext, IdentityServerConfigurationDbContext,
            IdentityServerPersistedGrantDbContext, IdentityServerDataProtectionDbContext>(this IServiceCollection services,
            string identityConnectionString, string configurationConnectionString,
            string persistedGrantConnectionString, string dataProtectionConnectionString)
            where AdminIdentityDbContext : DbContext
            where IdentityServerPersistedGrantDbContext : DbContext, IAdminPersistedGrantDbContext
            where IdentityServerConfigurationDbContext : DbContext, IAdminConfigurationDbContext
            where IdentityServerDataProtectionDbContext : DbContext, IDataProtectionKeyContext
        {
            var migrationsAssembly = typeof(DatabaseExtensions).GetTypeInfo().Assembly.GetName().Name;

            // Config DB for identity
            services.AddDbContext<AdminIdentityDbContext>(options => options.UseMySql(identityConnectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));

            // Config DB from existing connection
            services.AddConfigurationDbContext<IdentityServerConfigurationDbContext>(options => options.ConfigureDbContext = b => b.UseMySql(configurationConnectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));

            // Operational DB from existing connection
            services.AddOperationalDbContext<IdentityServerPersistedGrantDbContext>(options => options.ConfigureDbContext = b => b.UseMySql(persistedGrantConnectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));

            // DataProtectionKey DB from existing connection
            services.AddDbContext<IdentityServerDataProtectionDbContext>(options => options.UseMySql(dataProtectionConnectionString,
                optionsSql => optionsSql.MigrationsAssembly(migrationsAssembly)));

        }
    }
}





