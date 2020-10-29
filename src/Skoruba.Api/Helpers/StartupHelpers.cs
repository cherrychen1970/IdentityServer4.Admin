using Newtonsoft.Json.Serialization;
using System;
using IdentityModel;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Skoruba.AuditLogging.EntityFramework.DbContexts;
using Skoruba.AuditLogging.EntityFramework.Entities;
using Skoruba.AuditLogging.EntityFramework.Extensions;
using Skoruba.AuditLogging.EntityFramework.Repositories;
using Skoruba.AuditLogging.EntityFramework.Services;

using Skoruba.Admin.Api.AuditLogging;
using Skoruba.Admin.Api.Configuration;
using Skoruba.Admin.Api.Configuration.ApplicationParts;
using Skoruba.Admin.Api.Configuration.Constants;
using Skoruba.Admin.Api.Helpers.Localization;
using Skoruba.AspNetIdentity.EntityFramework;
using Skoruba.Admin.Api.EntityModels;
using Skoruba.AspNetIdentity.EntityFramework.Models;


namespace Skoruba.Admin.Api.Helpers
{
    public static class StartupHelpers
    {
        public static IServiceCollection AddAuditEventLogging(
            this IServiceCollection services, IConfiguration configuration)
        {
            var auditLoggingConfiguration = configuration.GetSection(nameof(AuditLoggingConfiguration))
                .Get<AuditLoggingConfiguration>();
            services.AddSingleton(auditLoggingConfiguration);

            // TEST CHERRY
            services.AddAuditLogging(options => { options.Source = auditLoggingConfiguration.Source; })
                .AddEventData<MockApiAuditSubject, ApiAuditAction>()
                .AddAuditSinks<DatabaseAuditEventLoggerSink<AuditLog>>();

            services.AddTransient<IAuditLoggingRepository<AuditLog>, AuditLoggingRepository<AuditLoggingDbContext<AuditLog>, AuditLog>>();
            return services;
        }

        public static IServiceCollection AddAdminApiCors(this IServiceCollection services, AdminApiConfiguration adminApiConfiguration)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        if (adminApiConfiguration.CorsAllowAnyOrigin)
                        {
                            builder.AllowAnyOrigin();
                        }
                        else
                        {
                            builder.WithOrigins(adminApiConfiguration.CorsAllowOrigins);
                        }

                        builder.AllowAnyHeader();
                        builder.AllowAnyMethod();
                    });
            });

            return services;
        }

        /// <summary>
        /// Register services for MVC
        /// </summary>
        /// <param name="services"></param>
        public static void AddMvcServices(this IServiceCollection services)
        {
            services.AddLocalization(opts => { opts.ResourcesPath = ConfigurationConsts.ResourcesPath; });

            services.TryAddTransient(typeof(IGenericControllerLocalizer<>), typeof(GenericControllerLocalizer<>));

            services.AddControllersWithViews(o => { o.Conventions.Add(new GenericControllerRouteConvention()); })
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    })            
                .AddDataAnnotationsLocalization()
                .ConfigureApplicationPartManager(m =>
                {
                    m.FeatureProviders.Add(new GenericTypeControllerFeatureProvider());
                });
        }

        /// <summary>
        /// Add authentication middleware for an API
        /// </summary>
        /// <typeparam name="AdminIdentityDbContext">DbContext for an access to Identity</typeparam>
        /// <typeparam name="TUser">Entity with User</typeparam>
        /// <typeparam name="IdentityRole<TKey>">Entity with Role</typeparam>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddApiAuthentication<TKey>(this IServiceCollection services,IConfiguration configuration) where TKey : IEquatable<TKey>
        {
            var adminApiConfiguration = configuration.GetSection(nameof(AdminApiConfiguration)).Get<AdminApiConfiguration>();

            services
                .AddIdentity<IdentityUser<TKey>, IdentityRole<TKey>>(options => configuration.GetSection(nameof(IdentityOptions)).Bind(options))
                .AddEntityFrameworkStores<AdminIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                options.DefaultForbidScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
            })
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = adminApiConfiguration.IdentityServerBaseUrl;
                    options.ApiName = adminApiConfiguration.OidcApiName;
                    options.RequireHttpsMetadata = adminApiConfiguration.RequireHttpsMetadata;
                });
        }

       public static void AddAuthorizationPolicies(this IServiceCollection services)
        {
            var adminApiConfiguration = services.BuildServiceProvider().GetService<AdminApiConfiguration>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthorizationConsts.AdministrationPolicy,
                    policy =>
                        policy.RequireAssertion(context => context.User.HasClaim(c =>
                                (c.Type == JwtClaimTypes.Role && c.Value == adminApiConfiguration.AdministrationRole) ||
                                (c.Type == $"client_{JwtClaimTypes.Role}" && c.Value == adminApiConfiguration.AdministrationRole)
                            )
                        ));
            });
        }
        public static void AddForwardHeaders(this IApplicationBuilder app)
        {
            var forwardingOptions = new ForwardedHeadersOptions()
            {
                ForwardedHeaders = ForwardedHeaders.All
            };

            forwardingOptions.KnownNetworks.Clear();
            forwardingOptions.KnownProxies.Clear();

            app.UseForwardedHeaders(forwardingOptions);
        }
    }
}
