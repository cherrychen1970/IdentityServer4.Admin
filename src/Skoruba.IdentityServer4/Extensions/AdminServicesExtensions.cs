using Microsoft.EntityFrameworkCore;
using IdentityServer4.EntityFramework.Entities;
using Skoruba.IdentityServer4.Resources;
using Skoruba.IdentityServer4.Services;
using Skoruba.IdentityServer4.EntityFramework.Repositories;
using Skoruba.IdentityServer4.EntityFramework.DbContexts;
using Skoruba.IdentityServer4.Models;
using Skoruba.Repositories;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AdminServicesExtensions
    {
        public static IServiceCollection AddAdminDbContexts(this IServiceCollection services)
        {
            return services;
        }
        public static IServiceCollection AddAdminServices(this IServiceCollection services)
        {
            // TODO : add more repository
            //Repositories
            services.AddTransient<ClientRepository>();
            services.AddTransient<ApiResourceRepository>();
            // TODO : use factory
            services.AddScoped<Repository<ConfDbContext, IdentityResource, IdentityResourceDto,int>>();

            //Services
            services.AddTransient<ClientService>();

            //Resources
            services.AddScoped<IApiResourceServiceResources, ApiResourceServiceResources>();
            services.AddScoped<IClientServiceResources, ClientServiceResources>();
            services.AddScoped<IIdentityResourceServiceResources, IdentityResourceServiceResources>();
            services.AddScoped<IPersistedGrantServiceResources, PersistedGrantServiceResources>();

            return services;
        }
    }
}
