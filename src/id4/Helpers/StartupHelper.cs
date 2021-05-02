using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using IdDbContexts = IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.Configuration;
using Bluebird.Repositories;

using id4.Data;
using id4.Models;
using id4.Services;

namespace id4
{
    public static class StartupHelper
    {
        public static void SqliteMigrate(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider().CreateScope().ServiceProvider;
            using (var scope = provider.CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<SqliteApplicationDbContext>())
                {
                    context.Database.Migrate();
                }
                using (var context = scope.ServiceProvider.GetRequiredService<SqliteConfigurationDbContext>())
                {
                    context.Database.Migrate();
                }
                using (var context = scope.ServiceProvider.GetRequiredService<SqlitePersistedGrantDbContext>())
                {
                    context.Database.Migrate();
                }
            }
        }
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
        public static void AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqliteConnection");
            services.AddSqliteDbContexts(connectionString);
            services.SqliteMigrate();
        }

        public static void AddSqliteDbContexts(this IServiceCollection services, string connection)
        {
            // this is for migration
            services.AddDbContext<SqliteApplicationDbContext>(options =>
                options.UseSqlite(connection, b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name)));
            services.AddDbContext<SqliteConfigurationDbContext>(options =>
                options.UseSqlite(connection, b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name)));
            services.AddDbContext<SqlitePersistedGrantDbContext>(options =>
                options.UseSqlite(connection, b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name)));

            // this is for all other services
            services.AddDbContext<ApplicationDbContext>(options =>
                //options.UseSqlite(connection));
                options.UseSqlite(connection));
            services.AddDbContext<IdDbContexts.PersistedGrantDbContext>(options =>
                //options.UseSqlite(connection));
                options.UseSqlite(connection));
            services.AddDbContext<IdDbContexts.ConfigurationDbContext>(options =>
                //options.UseSqlite(connection));
                options.UseSqlite(connection));
            services.AddDbContext<ConfigurationDbContext>(options =>
                //options.UseSqlite(connection));
                options.UseSqlite(connection));
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
            services.AddDbContext<IdDbContexts.PersistedGrantDbContext>(options =>
                //options.UseSqlite(connection));
                options.UseNpgsql(connection));
            services.AddDbContext<IdDbContexts.ConfigurationDbContext>(options =>
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

        public static void AddCustomIdentityServer(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqliteConnection");
            var builder = services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;

                    options.UserInteraction = new UserInteractionOptions
                    {
                        LogoutUrl = "/Account/Logout",
                        LoginUrl = "/Account/Login",
                        LoginReturnUrlParameter = "returnUrl"
                    };
                })
                .AddAspNetIdentity<AppUser>()
                // this adds the config data from DB (clients, resources, CORS)
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = db => db.UseSqlite(connectionString);
                })
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = db => db.UseSqlite(connectionString);
                    options.EnableTokenCleanup = true;
                    // options.TokenCleanupInterval = 15; // interval in seconds. 15 seconds useful for debugging
                })
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator<AppUser>>()
                ;

            // not recommended for production - you need to store your key material somewhere secure
            builder.AddDeveloperSigningCredential();
        }

        public static void ConfigureIIS(this IServiceCollection services, string name = "Windows", bool automatic = false)
        {
            // configures IIS out-of-proc settings (see https://github.com/aspnet/AspNetCore/issues/14882)
            services.Configure<IISOptions>(iis =>
            {
                iis.AuthenticationDisplayName = name;
                iis.AutomaticAuthentication = automatic;
            });

            // configures IIS in-proc settings
            services.Configure<IISServerOptions>(iis =>
            {
                iis.AuthenticationDisplayName = name;
                iis.AutomaticAuthentication = automatic;
            });
        }

        public static IMvcBuilder AddNewtonsoftJsonCamelCase(this IMvcBuilder builder)
        {
            return builder.AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
        }

        public static void UseDefaultErrorHandling(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.EnvironmentName == "development")
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
        }
    }
}