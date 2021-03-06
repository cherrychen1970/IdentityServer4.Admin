﻿using Microsoft.EntityFrameworkCore;
using IdentityServer4.EntityFramework.Entities;
using Skoruba.IdentityServer4.Models;
using Skoruba.IdentityServer4.Resources;
using Skoruba.IdentityServer4.Services;
using Skoruba.IdentityServer4.EntityFramework.Repositories;
using Bluebird.Repositories;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AdminServicesExtensions
    {
        public static IServiceCollection AddAdminServices(this IServiceCollection services)
        {
            // TODO : add more repository
            //Repositories
            //services.AddScoped<ClientRepository>();
            //services.AddScoped<ClientClaimRepository>();
           // services.AddScoped<IRepository<ClientDto,int>,ClientRepository>();

            //services.AddScoped<IRepository<ClientScope,int>,  AdminConfigurationRepository<ClientScope, string>>();
            //services.AddScoped<IRepository<ClientSecretDto,int>,  ClientSecretRepository>();
            //services.AddScoped<IRepository<ClientPropertyDto,int>, ClientPropertyRepository>();
            //services.AddScoped<IRepository<ClientClaimDto,int>, ClientClaimRepository >();
            //services.AddScoped<IRepository<ClientRedirectUri,int>, ClientRedirectUriRepository>();
            //services.AddScoped<IRepository<ClientGrantType,int>, ClientGrantTypeRepository>();
            
            // identity
            services.AddScoped<IRepository<IdentityResource,int>, IdentityResourceRepository>();
            services.AddScoped<IRepository<IdentityResourceClaim,int>, IdentityClaimRepository>();
            services.AddScoped<IRepository<IdentityResourceProperty,int>, IdentityPropertyRepository>();

            // all the api resource repository
            services.AddScoped<IRepository<ApiResource,int>, ApiResourceRepository>();
            services.AddScoped<IRepository<ApiResourceProperty,int>, ApiResourcePropertyRepository>();            
            services.AddScoped<IRepository<ApiResourceClaim,int>,ApiResourceClaimRepository>();
            services.AddScoped<IRepository<ApiResourceSecret,int>, ApiSecretRepository>();            
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
