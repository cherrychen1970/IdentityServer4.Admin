using AutoMapper;
using IdentityServer4.EntityFramework.Entities;
using Skoruba.IdentityServer4.Dtos.Grant;
using Skoruba.EntityFramework.Entities;
using Skoruba.Core.Dtos.Common;

namespace Skoruba.IdentityServer4.Mappers
{
    public static class PersistedGrantMappers
    {
        static PersistedGrantMappers()
        {
            Mapper = new MapperConfiguration(cfg =>cfg.AddProfile<PersistedGrantMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }
        public static PersistedGrantDto ToModel(this PersistedGrant grant)
        {
            return grant == null ? null : Mapper.Map<PersistedGrantDto>(grant);
        }
    }
}