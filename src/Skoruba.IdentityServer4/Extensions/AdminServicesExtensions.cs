using Microsoft.EntityFrameworkCore;
using IdentityServer4.EntityFramework.Entities;
using Skoruba.IdentityServer4.Resources;
using Skoruba.IdentityServer4.Services;
using Skoruba.IdentityServer4.Services.Interfaces;
using Skoruba.EntityFramework.Interfaces;
using Skoruba.IdentityServer4.Repositories;
using Skoruba.EntityFramework.Repositories.Interfaces;
using Skoruba.EntityFramework.Shared.DbContexts;
using Skoruba.IdentityServer4.Dtos.Configuration;
using Skoruba.Core.Repositories;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AdminServicesExtensions
    {
        public static IServiceCollection AddAdminServices<TConfigurationDbContext,TGrantDbContext>(this IServiceCollection services)
        {
            // TODO : add more repository
            //Repositories
            services.AddTransient<ClientRepository>();
            services.AddTransient<ApiResourceRepository>();
            // TODO : use factory
            services.AddScoped<Repository<ConfDbContext, IdentityResource, IdentityResourceDto,int>>();


            //Services
            services.AddTransient<IClientService, ClientService>();

            //Resources
            services.AddScoped<IApiResourceServiceResources, ApiResourceServiceResources>();
            services.AddScoped<IClientServiceResources, ClientServiceResources>();
            services.AddScoped<IIdentityResourceServiceResources, IdentityResourceServiceResources>();
            services.AddScoped<IPersistedGrantServiceResources, PersistedGrantServiceResources>();

            return services;
        }
    }
}
