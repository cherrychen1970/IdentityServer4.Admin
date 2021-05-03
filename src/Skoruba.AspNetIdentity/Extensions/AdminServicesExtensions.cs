using System;
using AutoMapper;

using Bluebird.Repositories;

using Skoruba.AspNetIdentity.Models;
using Skoruba.AspNetIdentity.EntityFramework.Models;
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
        public static IServiceCollection AddAspNetIdentityServices(this IServiceCollection services)//, HashSet<Type> profileTypes)
        {
            //Repositories
            services.AddTransient<IdentityRepository>();
            //services.AddTransient<IPersistedGrantAspNetIdentityRepository, PersistedGrantAspNetIdentityRepository<TKey>>();

            services.AddScoped<IRepository<User,string>,UserRepository>();
            services.AddScoped<IRepository<UserClaim,int>,UserClaimRepository>();
            services.AddScoped<IRepository<UserRole,int>,UserRoleRepository>();
            services.AddScoped<IRepository<UserLogin,string>,UserLoginRepository>();
            services.AddScoped<IRepository<UserToken,string>, UserTokenRepository>();
            services.AddScoped<IRepository<Role,string>,RoleRepository>();
            services.AddScoped<IRepository<RoleClaim,int>,RoleClaimRepository>();
            
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
