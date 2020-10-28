using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Skoruba.AspNetIdentity.Dtos;
using Skoruba.AspNetIdentity.Mappers.Configuration;
using Skoruba.AspNetIdentity.Resources;
using Skoruba.AspNetIdentity.Services;
using Skoruba.AspNetIdentity.Services.Interfaces;
using Skoruba.EntityFramework.Identity.Repositories;
using Skoruba.EntityFramework.Identity.Repositories.Interfaces;
using Skoruba.EntityFramework.Interfaces;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AdminServicesExtensions
    {
        public static IMapperConfigurationBuilder AddAspNetIdentityMapping(this IServiceCollection services)
        {
            var builder = new MapperConfigurationBuilder();

            services.AddSingleton<IConfigurationProvider>(sp => new MapperConfiguration(cfg =>
            {
                foreach (var profileType in builder.ProfileTypes)
                    cfg.AddProfile(profileType);
            }));

            services.AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<IConfigurationProvider>(), sp.GetService));

            return builder;
        }

        public static IServiceCollection AddAspNetIdentityServices<TKey>( this IServiceCollection services, HashSet<Type> profileTypes)
            where TKey : IEquatable<TKey>
 
        {
            //Repositories
            services.AddTransient<IIdentityRepository<TKey>>();
            //services.AddTransient<IPersistedGrantAspNetIdentityRepository, PersistedGrantAspNetIdentityRepository<TKey>>();
          
            //Services
            services.AddTransient<IIdentityService<TKey>>();            
            
            //Resources
            services.AddScoped<IIdentityServiceResources, IdentityServiceResources>();
            services.AddScoped<IPersistedGrantAspNetIdentityServiceResources, PersistedGrantAspNetIdentityServiceResources>();

            //Register mapping
            services.AddAspNetIdentityMapping()
                .UseIdentityMappingProfile<TKey>()
                .AddProfilesType(profileTypes);

            return services;
        }
    }
}
