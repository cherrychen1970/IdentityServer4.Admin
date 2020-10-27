using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Skoruba.Admin.BusinessLogic.Identity.Dtos.Identity;
using Skoruba.Admin.BusinessLogic.Identity.Mappers.Configuration;
using Skoruba.Admin.BusinessLogic.Identity.Resources;
using Skoruba.Admin.BusinessLogic.Identity.Services;
using Skoruba.Admin.BusinessLogic.Identity.Services.Interfaces;
using Skoruba.Admin.EntityFramework.Identity.Repositories;
using Skoruba.Admin.EntityFramework.Identity.Repositories.Interfaces;
using Skoruba.Admin.EntityFramework.Interfaces;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AdminServicesExtensions
    {
        public static IMapperConfigurationBuilder AddAdminAspNetIdentityMapping(this IServiceCollection services)
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

        public static IServiceCollection AddAdminAspNetIdentityServices<TUser,TKey>( this IServiceCollection services, HashSet<Type> profileTypes)
            where TUser : IdentityUser<TKey>
            where TKey : IEquatable<TKey>
 
        {
            //Repositories
            services.AddTransient<IIdentityRepository<TUser,TKey>>();
            services.AddTransient<IPersistedGrantAspNetIdentityRepository, PersistedGrantAspNetIdentityRepository>();
          
            //Services
            services.AddTransient<IIdentityService>();
            services.AddTransient<IPersistedGrantAspNetIdentityService, PersistedGrantAspNetIdentityService>();
            
            //Resources
            services.AddScoped<IIdentityServiceResources, IdentityServiceResources>();
            services.AddScoped<IPersistedGrantAspNetIdentityServiceResources, PersistedGrantAspNetIdentityServiceResources>();

            //Register mapping
            services.AddAdminAspNetIdentityMapping()
                .UseIdentityMappingProfile<TUser,TKey>()
                .AddProfilesType(profileTypes);

            return services;
        }
    }
}
