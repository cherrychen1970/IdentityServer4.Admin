﻿using System;
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
using Skoruba.Admin.BusinessLogic.Identity.Dtos.Identity;
using Skoruba.Admin.BusinessLogic.Services;
using Skoruba.Admin.BusinessLogic.Services.Interfaces;
using Skoruba.Admin.ExceptionHandling;
using Skoruba.Admin.Configuration;
using Skoruba.Admin.Configuration.ApplicationParts;
using Skoruba.Admin.Configuration.Constants;
using Skoruba.Admin.Configuration.Interfaces;
using Skoruba.Admin.EntityFramework.Interfaces;
using Skoruba.Admin.EntityFramework.Repositories;
using Skoruba.Admin.EntityFramework.Repositories.Interfaces;
using Skoruba.Admin.Helpers.Localization;
using System.Linq;
using Skoruba.Admin.EntityFramework.MySql.Extensions;
using Skoruba.Admin.EntityFramework.Shared.Configuration;
using Skoruba.Admin.EntityFramework.SqlServer.Extensions;
using Skoruba.Admin.EntityFramework.PostgreSQL.Extensions;
using Skoruba.Admin.EntityFramework.Helpers;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Skoruba.IdentityServer4.Shared.Authentication;
using Skoruba.IdentityServer4.Shared.Configuration.Identity;

namespace Skoruba.Admin.Helpers
{
    public static class StartupHelpers
    {
        public static IServiceCollection AddAuditEventLogging<TAuditLoggingDbContext, TAuditLog>(this IServiceCollection services, IConfiguration configuration)
            where TAuditLog : AuditLog, new()
            where TAuditLoggingDbContext : IAuditLoggingDbContext<TAuditLog>
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
                .AddAuditSinks<DatabaseAuditEventLoggerSink<TAuditLog>>();

            // repository for library
            services.AddTransient<IAuditLoggingRepository<TAuditLog>, AuditLoggingRepository<TAuditLoggingDbContext, TAuditLog>>();

            // repository and service for admin
            services.AddTransient<IAuditLogRepository<TAuditLog>, AuditLogRepository<TAuditLoggingDbContext, TAuditLog>>();
            services.AddTransient<IAuditLogService, AuditLogService<TAuditLog>>();

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
        public static void RegisterDbContexts<AdminIdentityDbContext, IdentityServerConfigurationDbContext, IdentityServerPersistedGrantDbContext, TLogDbContext, TAuditLoggingDbContext, IdentityServerDataProtectionDbContext>(this IServiceCollection services, IConfiguration configuration)
            where AdminIdentityDbContext : DbContext
            where IdentityServerPersistedGrantDbContext : DbContext, IAdminPersistedGrantDbContext
            where IdentityServerConfigurationDbContext : DbContext, IAdminConfigurationDbContext
            where TLogDbContext : DbContext, IAdminLogDbContext
            where TAuditLoggingDbContext : DbContext, IAuditLoggingDbContext<AuditLog>
            where IdentityServerDataProtectionDbContext : DbContext, IDataProtectionKeyContext
        {
            var databaseProvider = configuration.GetSection(nameof(DatabaseProviderConfiguration)).Get<DatabaseProviderConfiguration>();

            var identityConnectionString = configuration.GetConnectionString("IdentityConnection");
            var configurationConnectionString = configuration.GetConnectionString("IdentityConnection");
            var persistedGrantsConnectionString = configuration.GetConnectionString("IdentityConnection");
            var errorLoggingConnectionString = configuration.GetConnectionString("IdentityConnection");
            var auditLoggingConnectionString = configuration.GetConnectionString("IdentityConnection");
            var dataProtectionConnectionString = configuration.GetConnectionString("IdentityConnection");

            switch (databaseProvider.ProviderType)
            {
                case DatabaseProviderType.SqlServer:
                    services.RegisterSqlServerDbContexts<AdminIdentityDbContext, IdentityServerConfigurationDbContext, IdentityServerPersistedGrantDbContext, TLogDbContext, TAuditLoggingDbContext, IdentityServerDataProtectionDbContext>(identityConnectionString, configurationConnectionString, persistedGrantsConnectionString, errorLoggingConnectionString, auditLoggingConnectionString, dataProtectionConnectionString);
                    break;
                case DatabaseProviderType.PostgreSQL:
                    services.RegisterNpgSqlDbContexts<AdminIdentityDbContext, IdentityServerConfigurationDbContext, IdentityServerPersistedGrantDbContext, TLogDbContext, TAuditLoggingDbContext, IdentityServerDataProtectionDbContext>(identityConnectionString, configurationConnectionString, persistedGrantsConnectionString, errorLoggingConnectionString, auditLoggingConnectionString, dataProtectionConnectionString);
                    break;
                case DatabaseProviderType.MySql:
                    services.RegisterMySqlDbContexts<AdminIdentityDbContext, IdentityServerConfigurationDbContext, IdentityServerPersistedGrantDbContext, TLogDbContext, TAuditLoggingDbContext, IdentityServerDataProtectionDbContext>(identityConnectionString, configurationConnectionString, persistedGrantsConnectionString, errorLoggingConnectionString, auditLoggingConnectionString, dataProtectionConnectionString);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(databaseProvider.ProviderType), $@"The value needs to be one of {string.Join(", ", Enum.GetNames(typeof(DatabaseProviderType)))}.");
            }
        }

        /// <summary>
        /// Register in memory DbContexts for IdentityServer ConfigurationStore and PersistedGrants, Identity and Logging
        /// For testing purpose only
        /// </summary>
        /// <typeparam name="IdentityServerConfigurationDbContext"></typeparam>
        /// <typeparam name="IdentityServerPersistedGrantDbContext"></typeparam>
        /// <typeparam name="TLogDbContext"></typeparam>
        /// <typeparam name="AdminIdentityDbContext"></typeparam>
        /// <typeparam name="TAuditLoggingDbContext"></typeparam>
        /// <param name="services"></param>
        public static void RegisterDbContextsStaging<AdminIdentityDbContext, IdentityServerConfigurationDbContext, IdentityServerPersistedGrantDbContext, TLogDbContext, TAuditLoggingDbContext, IdentityServerDataProtectionDbContext>(this IServiceCollection services)
            where AdminIdentityDbContext : DbContext
            where IdentityServerPersistedGrantDbContext : DbContext, IAdminPersistedGrantDbContext
            where IdentityServerConfigurationDbContext : DbContext, IAdminConfigurationDbContext
            where TLogDbContext : DbContext, IAdminLogDbContext
            where TAuditLoggingDbContext : DbContext, IAuditLoggingDbContext<AuditLog>
            where IdentityServerDataProtectionDbContext : DbContext, IDataProtectionKeyContext
        {
            var persistedGrantsDatabaseName = Guid.NewGuid().ToString();
            var configurationDatabaseName = Guid.NewGuid().ToString();
            var logDatabaseName = Guid.NewGuid().ToString();
            var identityDatabaseName = Guid.NewGuid().ToString();
            var auditLoggingDatabaseName = Guid.NewGuid().ToString();
            var dataProtectionDatabaseName = Guid.NewGuid().ToString();

            var operationalStoreOptions = new OperationalStoreOptions();
            services.AddSingleton(operationalStoreOptions);

            var storeOptions = new ConfigurationStoreOptions();
            services.AddSingleton(storeOptions);

            services.AddDbContext<AdminIdentityDbContext>(optionsBuilder => optionsBuilder.UseInMemoryDatabase(identityDatabaseName));
            services.AddDbContext<IdentityServerPersistedGrantDbContext>(optionsBuilder => optionsBuilder.UseInMemoryDatabase(persistedGrantsDatabaseName));
            services.AddDbContext<IdentityServerConfigurationDbContext>(optionsBuilder => optionsBuilder.UseInMemoryDatabase(configurationDatabaseName));
            services.AddDbContext<TLogDbContext>(optionsBuilder => optionsBuilder.UseInMemoryDatabase(logDatabaseName));
            services.AddDbContext<TAuditLoggingDbContext>(optionsBuilder => optionsBuilder.UseInMemoryDatabase(auditLoggingDatabaseName));
            services.AddDbContext<IdentityServerDataProtectionDbContext>(optionsBuilder => optionsBuilder.UseInMemoryDatabase(dataProtectionDatabaseName));
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
        public static void AddMvcWithLocalization<TUserDto, RoleDto<TKey>, TUser, IdentityRole<TKey>, TKey, IdentityUserClaim<TKey>, IdentityUserRole<TKey>, IdentityUserLogin<TKey>, IdentityRoleClaim<TKey>, IdentityUserToken<TKey>,
            TUsersDto, IdentityRolesDto<TKey>, IdentityUserRole<TKey>sDto, IdentityUserClaim<TKey>sDto,
            UserProviderDto<TKey>, TUserProvidersDto, UserChangePasswordDto<TKey>, IdentityRoleClaim<TKey>sDto, IdentityUserClaim<TKey>Dto, IdentityRoleClaim<TKey>Dto>(this IServiceCollection services, IConfiguration configuration)
            where TUserDto : UserDto<TKey>, new()
            where RoleDto<TKey> : RoleDto<TKey>, new()
            where TUser : IdentityUser<TKey>
            where IdentityRole<TKey> : IdentityRole<TKey>
            where TKey : IEquatable<TKey>
            where IdentityUserClaim<TKey> : IdentityUserClaim<TKey>
            where IdentityUserRole<TKey> : IdentityUserRole<TKey>
            where IdentityUserLogin<TKey> : IdentityUserLogin<TKey>
            where IdentityRoleClaim<TKey> : IdentityRoleClaim<TKey>
            where IdentityUserToken<TKey> : IdentityUserToken<TKey>
            where TUsersDto : UsersDto<TUserDto, TKey>
            where IdentityRolesDto<TKey> : RolesDto<RoleDto<TKey>, TKey>
            where IdentityUserRole<TKey>sDto : UserRolesDto<RoleDto<TKey>, TKey>
            where IdentityUserClaim<TKey>sDto : UserClaimsDto<IdentityUserClaim<TKey>Dto, TKey>
            where UserProviderDto<TKey> : UserProviderDto<TKey>
            where TUserProvidersDto : UserProvidersDto<UserProviderDto<TKey>, TKey>
            where UserChangePasswordDto<TKey> : UserChangePasswordDto<TKey>
            where IdentityRoleClaim<TKey>sDto : RoleClaimsDto<IdentityRoleClaim<TKey>Dto, TKey>
            where IdentityUserClaim<TKey>Dto : UserClaimDto<TKey>
            where IdentityRoleClaim<TKey>Dto: RoleClaimDto<TKey>
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
                    m.FeatureProviders.Add(new GenericTypeControllerFeatureProvider<TUserDto, RoleDto<TKey>, TUser, IdentityRole<TKey>, TKey, IdentityUserClaim<TKey>, IdentityUserRole<TKey>, IdentityUserLogin<TKey>, IdentityRoleClaim<TKey>, IdentityUserToken<TKey>,
                        TUsersDto, IdentityRolesDto<TKey>, IdentityUserRole<TKey>sDto, IdentityUserClaim<TKey>sDto,
                        UserProviderDto<TKey>, TUserProvidersDto, UserChangePasswordDto<TKey>, IdentityRoleClaim<TKey>sDto, IdentityUserClaim<TKey>Dto, IdentityRoleClaim<TKey>Dto>());
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

        public static void AddAuthenticationServicesStaging<TContext, TUserIdentity, TUserIdentityRole>(
            this IServiceCollection services)
            where TContext : DbContext where TUserIdentity : class where TUserIdentityRole : class
        {
            services.AddIdentity<TUserIdentity, TUserIdentityRole>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<TContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultForbidScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);
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
        public static void AddAuthenticationServices<TContext, TUserIdentity, TUserIdentityRole>(this IServiceCollection services, IConfiguration configuration)
            where TContext : DbContext where TUserIdentity : class where TUserIdentityRole : class
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
                .AddIdentity<TUserIdentity, TUserIdentityRole>(options => configuration.GetSection(nameof(IdentityOptions)).Bind(options))
                .AddEntityFrameworkStores<TContext>()
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

        public static void AddIdSHealthChecks<IdentityServerConfigurationDbContext, IdentityServerPersistedGrantDbContext, AdminIdentityDbContext, TLogDbContext, TAuditLoggingDbContext, IdentityServerDataProtectionDbContext>(this IServiceCollection services, IConfiguration configuration, AdminConfiguration adminConfiguration)
            where IdentityServerConfigurationDbContext : DbContext, IAdminConfigurationDbContext
            where IdentityServerPersistedGrantDbContext : DbContext, IAdminPersistedGrantDbContext
            where AdminIdentityDbContext : DbContext
            where TLogDbContext : DbContext, IAdminLogDbContext
            where TAuditLoggingDbContext : DbContext, IAuditLoggingDbContext<AuditLog>
            where IdentityServerDataProtectionDbContext : DbContext, IDataProtectionKeyContext
        {
            var configurationDbConnectionString = configuration.GetConnectionString(ConfigurationConsts.ConfigurationDbConnectionStringKey);
            var persistedGrantsDbConnectionString = configuration.GetConnectionString(ConfigurationConsts.PersistedGrantDbConnectionStringKey);
            var identityDbConnectionString = configuration.GetConnectionString(ConfigurationConsts.IdentityDbConnectionStringKey);
            var logDbConnectionString = configuration.GetConnectionString(ConfigurationConsts.AdminLogDbConnectionStringKey);
            var auditLogDbConnectionString = configuration.GetConnectionString(ConfigurationConsts.AdminAuditLogDbConnectionStringKey);
            var dataProtectionDbConnectionString = configuration.GetConnectionString(ConfigurationConsts.DataProtectionDbConnectionStringKey);

            var identityServerUri = adminConfiguration.IdentityServerBaseUrl;
            var healthChecksBuilder = services.AddHealthChecks()
                .AddDbContextCheck<IdentityServerConfigurationDbContext>("ConfigurationDbContext")
                .AddDbContextCheck<IdentityServerPersistedGrantDbContext>("PersistedGrantsDbContext")
                .AddDbContextCheck<AdminIdentityDbContext>("IdentityDbContext")
                .AddDbContextCheck<TLogDbContext>("LogDbContext")
                .AddDbContextCheck<TAuditLoggingDbContext>("AuditLogDbContext")
                .AddDbContextCheck<IdentityServerDataProtectionDbContext>("DataProtectionDbContext")

                .AddIdentityServer(new Uri(identityServerUri), "Identity Server");

            var serviceProvider = services.BuildServiceProvider();
            var scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var configurationTableName = DbContextHelpers.GetEntityTable<IdentityServerConfigurationDbContext>(scope.ServiceProvider);
                var persistedGrantTableName = DbContextHelpers.GetEntityTable<IdentityServerPersistedGrantDbContext>(scope.ServiceProvider);
                var identityTableName = DbContextHelpers.GetEntityTable<AdminIdentityDbContext>(scope.ServiceProvider);
                var logTableName = DbContextHelpers.GetEntityTable<TLogDbContext>(scope.ServiceProvider);
                var auditLogTableName = DbContextHelpers.GetEntityTable<TAuditLoggingDbContext>(scope.ServiceProvider);
                var dataProtectionTableName = DbContextHelpers.GetEntityTable<IdentityServerDataProtectionDbContext>(scope.ServiceProvider);

                var databaseProvider = configuration.GetSection(nameof(DatabaseProviderConfiguration)).Get<DatabaseProviderConfiguration>();
                switch (databaseProvider.ProviderType)
                {
                    case DatabaseProviderType.SqlServer:
                        healthChecksBuilder
                            .AddSqlServer(configurationDbConnectionString, name: "ConfigurationDb",
                                healthQuery: $"SELECT TOP 1 * FROM dbo.[{configurationTableName}]")
                            .AddSqlServer(persistedGrantsDbConnectionString, name: "PersistentGrantsDb",
                                healthQuery: $"SELECT TOP 1 * FROM dbo.[{persistedGrantTableName}]")
                            .AddSqlServer(identityDbConnectionString, name: "IdentityDb",
                                healthQuery: $"SELECT TOP 1 * FROM dbo.[{identityTableName}]")
                            .AddSqlServer(logDbConnectionString, name: "LogDb",
                                healthQuery: $"SELECT TOP 1 * FROM dbo.[{logTableName}]")
                            .AddSqlServer(auditLogDbConnectionString, name: "AuditLogDb",
                                healthQuery: $"SELECT TOP 1 * FROM dbo.[{auditLogTableName}]")
                            .AddSqlServer(dataProtectionDbConnectionString, name: "DataProtectionDb",
                                healthQuery: $"SELECT TOP 1 * FROM dbo.[{dataProtectionTableName}]");
                        break;
                    case DatabaseProviderType.PostgreSQL:
                        healthChecksBuilder
                            .AddNpgSql(configurationDbConnectionString, name: "ConfigurationDb",
                                healthQuery: $"SELECT * FROM \"{configurationTableName}\" LIMIT 1")
                            .AddNpgSql(persistedGrantsDbConnectionString, name: "PersistentGrantsDb",
                                healthQuery: $"SELECT * FROM \"{persistedGrantTableName}\" LIMIT 1")
                            .AddNpgSql(identityDbConnectionString, name: "IdentityDb",
                                healthQuery: $"SELECT * FROM \"{identityTableName}\" LIMIT 1")
                            .AddNpgSql(logDbConnectionString, name: "LogDb",
                                healthQuery: $"SELECT * FROM \"{logTableName}\" LIMIT 1")
                            .AddNpgSql(auditLogDbConnectionString, name: "AuditLogDb",
                                healthQuery: $"SELECT * FROM \"{auditLogTableName}\"  LIMIT 1")
                            .AddNpgSql(dataProtectionDbConnectionString, name: "DataProtectionDb",
                                healthQuery: $"SELECT * FROM \"{dataProtectionTableName}\"  LIMIT 1");
                        break;
                    case DatabaseProviderType.MySql:
                        healthChecksBuilder
                            .AddMySql(configurationDbConnectionString, name: "ConfigurationDb")
                            .AddMySql(persistedGrantsDbConnectionString, name: "PersistentGrantsDb")
                            .AddMySql(identityDbConnectionString, name: "IdentityDb")
                            .AddMySql(logDbConnectionString, name: "LogDb")
                            .AddMySql(auditLogDbConnectionString, name: "AuditLogDb")
                            .AddMySql(dataProtectionDbConnectionString, name: "DataProtectionDb");
                        break;
                    default:
                        throw new NotImplementedException($"Health checks not defined for database provider {databaseProvider.ProviderType}");
                }
            }
        }
    }
}