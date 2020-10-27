using Microsoft.EntityFrameworkCore;
using Skoruba.Admin.BusinessLogic.Resources;
using Skoruba.Admin.BusinessLogic.Services;
using Skoruba.Admin.BusinessLogic.Services.Interfaces;
using Skoruba.Admin.EntityFramework.Interfaces;
using Skoruba.Admin.EntityFramework.Repositories;
using Skoruba.Admin.EntityFramework.Repositories.Interfaces;
using Skoruba.Admin.EntityFramework.Shared.DbContexts;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AdminServicesExtensions
    {
        public static IServiceCollection AddAdminServices<TConfigurationDbContext,TGrantDbContext>(this IServiceCollection services)
        {
            //Repositories
            services.AddTransient<IClientRepository, ClientRepository>();
            services.AddTransient<IIdentityResourceRepository, IdentityResourceRepository>();
            services.AddTransient<IApiResourceRepository, ApiResourceRepository>();
            services.AddTransient<IPersistedGrantRepository, PersistedGrantRepository>();
            services.AddTransient<ILogRepository, LogRepository>();

            //Services
            services.AddTransient<IClientService, ClientService>();
            services.AddTransient<IApiResourceService, ApiResourceService>();
            services.AddTransient<IIdentityResourceService, IdentityResourceService>();
            services.AddTransient<IPersistedGrantService, PersistedGrantService>();
            services.AddTransient<ILogService, LogService>();

            //Resources
            services.AddScoped<IApiResourceServiceResources, ApiResourceServiceResources>();
            services.AddScoped<IClientServiceResources, ClientServiceResources>();
            services.AddScoped<IIdentityResourceServiceResources, IdentityResourceServiceResources>();
            services.AddScoped<IPersistedGrantServiceResources, PersistedGrantServiceResources>();

            return services;
        }
    }
}
