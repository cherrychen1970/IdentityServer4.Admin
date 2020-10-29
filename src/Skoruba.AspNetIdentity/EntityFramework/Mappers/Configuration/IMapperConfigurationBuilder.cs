using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Skoruba.AspNetIdentity.Models;

namespace Skoruba.AspNetIdentity.EntityFramework.Mappers
{
    public interface IMapperConfigurationBuilder
    {
        HashSet<Type> ProfileTypes { get; }

        IMapperConfigurationBuilder AddProfilesType(HashSet<Type> profileTypes);

        IMapperConfigurationBuilder UseIdentityMappingProfile<TKey>() where TKey:IEquatable<TKey>;

    }
}
