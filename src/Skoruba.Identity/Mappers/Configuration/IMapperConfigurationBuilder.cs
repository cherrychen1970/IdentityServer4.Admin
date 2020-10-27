using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Skoruba.Admin.BusinessLogic.Identity.Dtos.Identity;

namespace Skoruba.Admin.BusinessLogic.Identity.Mappers.Configuration
{
    public interface IMapperConfigurationBuilder
    {
        HashSet<Type> ProfileTypes { get; }

        IMapperConfigurationBuilder AddProfilesType(HashSet<Type> profileTypes);

        IMapperConfigurationBuilder UseIdentityMappingProfile<TUser,TKey>();

    }
}
