using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Skoruba.Identity.Dtos.Identity;

namespace Skoruba.Identity.Mappers.Configuration
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

        public IMapperConfigurationBuilder UseIdentityMappingProfile<TKey>() where TKey:IEquatable<TKey>
        {
            ProfileTypes.Add(typeof(IdentityMapperProfile<TKey>));
            return this;
        }
    }
}