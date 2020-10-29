using System;
using System.Collections.Generic;
using AutoMapper;

using Skoruba.AspNetIdentity.EntityFramework;
using Skoruba.AspNetIdentity.Resources;
using Skoruba.AspNetIdentity.EntityFramework.Repositories;
using Skoruba.AspNetIdentity.EntityFramework.Mappers;

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

        public static IServiceCollection AddAspNetIdentityServices<TDbContext,TKey>( this IServiceCollection services)//, HashSet<Type> profileTypes)
            where TDbContext : AdminIdentityDbContext<TKey>
            where TKey : IEquatable<TKey>
 
        {
            //Repositories
            services.AddTransient<IdentityRepository<TDbContext,TKey>>();
            //services.AddTransient<IPersistedGrantAspNetIdentityRepository, PersistedGrantAspNetIdentityRepository<TKey>>();
            
            //Resources
            services.AddScoped<IIdentityServiceResources, IdentityServiceResources>();
            services.AddScoped<IPersistedGrantAspNetIdentityServiceResources, PersistedGrantAspNetIdentityServiceResources>();

            //Register mapping
            //services.AddAspNetIdentityMapping()
                //.UseIdentityMappingProfile<TKey>()
                //.AddProfilesType(profileTypes)
                ;

            return services;
        }
    }
}
