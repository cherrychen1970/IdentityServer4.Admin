using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Skoruba.Admin.BusinessLogic.Identity.Dtos.Identity;

namespace Skoruba.Admin.BusinessLogic.Identity.Mappers.Configuration
{
    public class MapperConfigurationBuilder : IMapperConfigurationBuilder
    {
        public HashSet<Type> ProfileTypes { get; } = new HashSet<Type>();

        public IMapperConfigurationBuilder AddProfilesType(HashSet<Type> profileTypes)
        {
            if (profileTypes == null) return this;

            foreach (var profileType in profileTypes)
            {
                ProfileTypes.Add(profileType);
            }

            return this;
        }

        public IMapperConfigurationBuilder UseIdentityMappingProfile<TUser,TKey>()
        {
            ProfileTypes.Add(typeof(IdentityMapperProfile<TUser,TKey>));
            return this;
        }
    }
}