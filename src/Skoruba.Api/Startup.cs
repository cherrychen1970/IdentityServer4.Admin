using System;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;

using Skoruba.EntityFramework.DbContexts;
using Skoruba.IdentityServer4.EntityFramework.DbContexts;
using Skoruba.AspNetIdentity.EntityFramework;

using Skoruba.Admin.Api.Configuration;
using Skoruba.Admin.Api.ExceptionHandling;
using Skoruba.Admin.Api.Helpers;
using Skoruba.Admin.Api.Resources;

namespace Skoruba.Admin.Api
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            HostingEnvironment = env;
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostingEnvironment { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            var adminApiConfiguration = Configuration.GetSection(nameof(AdminApiConfiguration)).Get<AdminApiConfiguration>();
            services.AddSingleton(adminApiConfiguration);
            //services.AddAutoMapper(typeof(Startup));
            services.AddAutoMapper(typeof(IdentityServer4.Marker), typeof(AspNetIdentity.Marker));
            services.AddNpgSqlDbContexts(Configuration["ConnectionStrings:IdentityConnection"]);
            services.AddDataProtection()
                .SetApplicationName("Skoruba.IdentityServer4")
                .PersistKeysToDbContext<DataProtectionDbContext>();

            services.AddEmailSenders(Configuration);
            services.AddScoped<ControllerExceptionFilterAttribute>();
            services.AddScoped<IApiErrorResources, ApiErrorResources>();
            services.AddApiAuthentication(Configuration);
            services.AddAuthorizationPolicies();
            services.AddAspNetIdentityServices();
            services.AddAdminServices();
            services.AddAdminApiCors(adminApiConfiguration);
            services.AddMvcServices();

            // CHERRY TESTING
            //services.AddAuditEventLogging(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AdminApiConfiguration adminApiConfiguration, IServiceProvider provider)
        {
            app.AddForwardHeaders();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            if (Configuration.GetValue<bool>("DatabaseProviderConfiguration:Migrate"))
                Migrate(provider);
            app.UseRouting();
            //app.UseAuthentication();
            app.UseCors();
            //app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }

        public void Migrate(IServiceProvider provider)
        {
            provider.EnsureDatabasesMigrate<AdminIdentityDbContext>();
            provider.EnsureDatabasesMigrate<AdminConfigurationDbContext>();
            provider.EnsureDatabasesMigrate<AdminPersistedGrantDbContext>();
            provider.EnsureDatabasesMigrate<AdminLogDbContext>();
            provider.EnsureDatabasesMigrate<AdminAuditLogDbContext>();
            provider.EnsureDatabasesMigrate<DataProtectionDbContext>();

        }



    }
}
