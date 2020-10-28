using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Skoruba.AspNetIdentity.Dtos;

namespace Skoruba.AspNetIdentity.Mappers.Configuration
{
    public interface IMapperConfigurationBuilder
    {
        HashSet<Type> ProfileTypes { get; }

        IMapperConfigurationBuilder AddProfilesType(HashSet<Type> profileTypes);

        IMapperConfigurationBuilder UseIdentityMappingProfile<TKey>() where TKey:IEquatable<TKey>;

    }
}
