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
        public static IServiceCollection AddAdminServices(this IServiceCollection services)
        {
            // TODO : add more repository
            //Repositories
            services.AddScoped<ClientRepository>();
            services.AddScoped<IRepository<Client,int>,ClientRepository>();
            services.AddScoped<IRepository<ClientScope,int>,  ClientScopeRepository>();
            services.AddScoped<IRepository<ClientSecret,int>,  ClientSecretRepository>();
            services.AddScoped<IRepository<ClientProperty,int>, ClientPropertyRepository>();
            services.AddScoped<IRepository<ClientClaim,int>, ClientClaimRepository >();
            services.AddScoped<IRepository<ClientRedirectUri,int>, ClientRedirectUriRepository>();
            services.AddScoped<IRepository<ClientGrantType,int>, ClientGrantTypeRepository>();
            
            // identity
            services.AddScoped<IRepository<IdentityResource,int>, IdentityResourceRepository>();
            services.AddScoped<IRepository<IdentityClaim,int>, IdentityClaimRepository>();
            services.AddScoped<IRepository<IdentityResourceProperty,int>, IdentityPropertyRepository>();

            // all the api resource repository
            services.AddScoped<IRepository<ApiResource,int>, ApiResourceRepository>();
            services.AddScoped<IRepository<ApiResourceProperty,int>, ApiResourcePropertyRepository>();            
            services.AddScoped<IRepository<ApiResourceClaim,int>,ApiResourceClaimRepository>();
            services.AddScoped<IRepository<ApiSecret,int>, ApiSecretRepository>();            
            services.AddScoped<IRepository<ApiScope,int>, ApiScopeRepository>();
            services.AddScoped<IRepository<ApiScopeClaim,int>, ApiScopeClaimRepository>();
            
            // TODO : use factory
            //services.AddScoped<Repository<AdminConfigurationDbContext, IdentityResource, IdentityResourceDto,int>>();

            //Services
            services.AddScoped<ClientService>();

            //Resources
            services.AddScoped<IApiResourceServiceResources, ApiResourceServiceResources>();
            services.AddScoped<IClientServiceResources, ClientServiceResources>();
            services.AddScoped<IIdentityResourceServiceResources, IdentityResourceServiceResources>();
            services.AddScoped<IPersistedGrantServiceResources, PersistedGrantServiceResources>();

            return services;
        }
    }
}
