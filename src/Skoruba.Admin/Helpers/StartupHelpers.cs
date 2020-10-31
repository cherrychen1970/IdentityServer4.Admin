using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Skoruba.AuditLogging.EntityFramework.DbContexts;
using Skoruba.AuditLogging.EntityFramework.Entities;
using Skoruba.AuditLogging.EntityFramework.Extensions;
using Skoruba.AuditLogging.EntityFramework.Repositories;
using Skoruba.AuditLogging.EntityFramework.Services;
using Skoruba.Admin.ExceptionHandling;
using Skoruba.Admin.Configuration;
using Skoruba.Admin.Configuration.ApplicationParts;
using Skoruba.Admin.Configuration.Constants;
using Skoruba.Admin.Configuration.Interfaces;
using Skoruba.Admin.Helpers.Localization;
using System.Linq;
using Skoruba.EntityFramework.Shared.Configuration;
using Skoruba.EntityFramework.PostgreSQL.Extensions;
using Skoruba.Helpers;
using Skoruba.IdentityServer4.EntityFramework.DbContexts;
using Skoruba.AspNetIdentity.EntityFramework.Models;
using Skoruba.AspNetIdentity.EntityFramework;

namespace Skoruba.Admin.Helpers
{
    public static class StartupHelpers
    {
        public static IServiceCollection AddAuditEventLogging(this IServiceCollection services, IConfiguration configuration)            
        {
            var auditLoggingConfiguration = configuration.GetSection(nameof(AuditLoggingConfiguration)).Get<AuditLoggingConfiguration>();

            services.AddAuditLogging(options => { options.Source = auditLoggingConfiguration.Source; })
                .AddDefaultHttpEventData(subjectOptions =>
                    {
                        subjectOptions.SubjectIdentifierClaim = auditLoggingConfiguration.SubjectIdentifierClaim;
                        subjectOptions.SubjectNameClaim = auditLoggingConfiguration.SubjectNameClaim;
                    },
                    actionOptions =>
                    {
                        actionOptions.IncludeFormVariables = auditLoggingConfiguration.IncludeFormVariables;
                    })
                .AddAuditSinks<DatabaseAuditEventLoggerSink<AuditLog>>();

            // repository for library
            services.AddTransient<IAuditLoggingRepository<AuditLog>, AuditLoggingRepository<AuditLoggingDbContext<AuditLog>, AuditLog>>();

            // repository and service for admin
            services.AddTransient<AdminAuditLogDbContext>();            
            return services;
        }

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
        /// <param name="configuration"></param>
        public static void RegisterDbContexts<TKey>(this IServiceCollection services, IConfiguration configuration ) where TKey : IEquatable<TKey>
        {
            var databaseProvider = configuration.GetSection(nameof(DatabaseProviderConfiguration)).Get<DatabaseProviderConfiguration>();

            var identityConnectionString = configuration.GetConnectionString("IdentityConnection");
;            switch (databaseProvider.ProviderType)
            {
                case DatabaseProviderType.PostgreSQL:
                    services.RegisterNpgSqlDbContexts(identityConnectionString);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(databaseProvider.ProviderType), $@"The value needs to be one of {string.Join(", ", Enum.GetNames(typeof(DatabaseProviderType)))}.");
            }
        }

        /// <summary>
        /// Using of Forwarded Headers, Hsts, XXssProtection and Csp
        /// </summary>
        /// <param name="app"></param>
        public static void UseSecurityHeaders(this IApplicationBuilder app)
        {
            var forwardingOptions = new ForwardedHeadersOptions()
            {
                ForwardedHeaders = ForwardedHeaders.All
            };

            forwardingOptions.KnownNetworks.Clear();
            forwardingOptions.KnownProxies.Clear();

            app.UseForwardedHeaders(forwardingOptions);

            app.UseXXssProtection(options => options.EnabledWithBlockMode());
            app.UseXContentTypeOptions();
            app.UseXfo(options => options.SameOrigin());
            app.UseReferrerPolicy(options => options.NoReferrer());
            var allowCspUrls = new List<string>
            {
                "https://fonts.googleapis.com/",
                "https://fonts.gstatic.com/"
            };

            app.UseCsp(options =>
            {
                options.FontSources(configuration =>
                {
                    configuration.SelfSrc = true;
                    configuration.CustomSources = allowCspUrls;
                });

                //TODO: consider remove unsafe sources - currently using for toastr inline scripts in Notification.cshtml
                options.ScriptSources(configuration =>
                {
                    configuration.SelfSrc = true;
                    configuration.UnsafeInlineSrc = true;
                    configuration.UnsafeEvalSrc = true;
                });

                options.StyleSources(configuration =>
                {
                    configuration.SelfSrc = true;
                    configuration.CustomSources = allowCspUrls;
                    configuration.UnsafeInlineSrc = true;
                });
            });
        }

        /// <summary>
        /// Add middleware for localization
        /// </summary>
        /// <param name="app"></param>
        public static void ConfigureLocalization(this IApplicationBuilder app)
        {
            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);
        }

        /// <summary>
        /// Add authorization policies
        /// </summary>
        /// <param name="services"></param>
        public static void AddAuthorizationPolicies(this IServiceCollection services, IRootConfiguration rootConfiguration)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthorizationConsts.AdministrationPolicy,
                    policy => policy.RequireRole(rootConfiguration.AdminConfiguration.AdministrationRole));
            });
        }

        /// <summary>
        /// Add exception filter for controller
        /// </summary>
        /// <param name="services"></param>
        public static void AddMvcExceptionFilters(this IServiceCollection services)
        {
            //Exception handling
            services.AddScoped<ControllerExceptionFilterAttribute>();
        }

        /// <summary>
        /// Register services for MVC and localization including available languages
        /// </summary>
        /// <param name="services"></param>
        public static void AddMvcWithLocalization(this IServiceCollection services, IConfiguration configuration)   
        {
            services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();

            services.AddLocalization(opts => { opts.ResourcesPath = ConfigurationConsts.ResourcesPath; });

            services.TryAddTransient(typeof(IGenericControllerLocalizer<>), typeof(GenericControllerLocalizer<>));

            services.AddControllersWithViews(o =>
                {
                    o.Conventions.Add(new GenericControllerRouteConvention());
                })
                .AddViewLocalization(
                    LanguageViewLocationExpanderFormat.Suffix,
                    opts => { opts.ResourcesPath = ConfigurationConsts.ResourcesPath; })
                .AddDataAnnotationsLocalization()
                .ConfigureApplicationPartManager(m =>
                {
                    m.FeatureProviders.Add(new GenericTypeControllerFeatureProvider());
                });

            var cultureConfiguration = configuration.GetSection(nameof(CultureConfiguration)).Get<CultureConfiguration>();
            services.Configure<RequestLocalizationOptions>(
            opts =>
            {
                // If cultures are specified in the configuration, use them (making sure they are among the available cultures),
                // otherwise use all the available cultures
                var supportedCultureCodes = (cultureConfiguration?.Cultures?.Count > 0 ?
                    cultureConfiguration.Cultures.Intersect(CultureConfiguration.AvailableCultures) :
                    CultureConfiguration.AvailableCultures).ToArray();

                if (!supportedCultureCodes.Any()) supportedCultureCodes = CultureConfiguration.AvailableCultures;
                var supportedCultures = supportedCultureCodes.Select(c => new CultureInfo(c)).ToList();

                // If the default culture is specified use it, otherwise use CultureConfiguration.DefaultRequestCulture ("en")
                var defaultCultureCode = string.IsNullOrEmpty(cultureConfiguration?.DefaultCulture) ?
                    CultureConfiguration.DefaultRequestCulture : cultureConfiguration?.DefaultCulture;

                // If the default culture is not among the supported cultures, use the first supported culture as default
                if (!supportedCultureCodes.Contains(defaultCultureCode)) defaultCultureCode = supportedCultureCodes.FirstOrDefault();

                opts.DefaultRequestCulture = new RequestCulture(defaultCultureCode);
                opts.SupportedCultures = supportedCultures;
                opts.SupportedUICultures = supportedCultures;
            });
        }


        /// <summary>
        /// Register services for authentication, including Identity.
        /// For production mode is used OpenId Connect middleware which is connected to IdentityServer4 instance.
        /// For testing purpose is used cookie middleware with fake login url.
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <typeparam name="TUserIdentity"></typeparam>
        /// <typeparam name="TUserIdentityRole"></typeparam>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)            
        {
            var adminConfiguration = configuration.GetSection(nameof(AdminConfiguration)).Get<AdminConfiguration>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
                options.Secure = CookieSecurePolicy.SameAsRequest;
                options.OnAppendCookie = cookieContext =>
                    AuthenticationHelpers.CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
                options.OnDeleteCookie = cookieContext =>
                    AuthenticationHelpers.CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
            });

            services
                .AddIdentity<User,Role>(options => configuration.GetSection(nameof(IdentityOptions)).Bind(options))
                .AddEntityFrameworkStores<AdminIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = AuthenticationConsts.OidcAuthenticationScheme;

                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultForbidScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
                        options =>
                        {
                            options.Cookie.Name = adminConfiguration.IdentityAdminCookieName;
                        })
                    .AddOpenIdConnect(AuthenticationConsts.OidcAuthenticationScheme, options =>
                    {
                        options.Authority = adminConfiguration.IdentityServerBaseUrl;
                        options.RequireHttpsMetadata = adminConfiguration.RequireHttpsMetadata;
                        options.ClientId = adminConfiguration.ClientId;
                        options.ClientSecret = adminConfiguration.ClientSecret;
                        options.ResponseType = adminConfiguration.OidcResponseType;

                        options.Scope.Clear();
                        foreach (var scope in adminConfiguration.Scopes)
                        {
                            options.Scope.Add(scope);
                        }

                        options.ClaimActions.MapJsonKey(adminConfiguration.TokenValidationClaimRole, adminConfiguration.TokenValidationClaimRole, adminConfiguration.TokenValidationClaimRole);

                        options.SaveTokens = true;

                        options.GetClaimsFromUserInfoEndpoint = true;

                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            NameClaimType = adminConfiguration.TokenValidationClaimName,
                            RoleClaimType = adminConfiguration.TokenValidationClaimRole
                        };

                        options.Events = new OpenIdConnectEvents
                        {
                            OnMessageReceived = context => OnMessageReceived(context, adminConfiguration),
                            OnRedirectToIdentityProvider = context => OnRedirectToIdentityProvider(context, adminConfiguration)
                        };
                    });
        }

        private static Task OnMessageReceived(MessageReceivedContext context, AdminConfiguration adminConfiguration)
        {
            context.Properties.IsPersistent = true;
            context.Properties.ExpiresUtc = new DateTimeOffset(DateTime.Now.AddHours(adminConfiguration.IdentityAdminCookieExpiresUtcHours));

            return Task.FromResult(0);
        }

        private static Task OnRedirectToIdentityProvider(RedirectContext n, AdminConfiguration adminConfiguration)
        {
            n.ProtocolMessage.RedirectUri = adminConfiguration.IdentityAdminRedirectUri;

            return Task.FromResult(0);
        }

    }
}
