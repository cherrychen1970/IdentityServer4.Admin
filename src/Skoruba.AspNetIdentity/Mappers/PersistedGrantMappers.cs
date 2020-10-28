using AutoMapper;
using IdentityServer4.EntityFramework.Entities;
using Skoruba.AspNetIdentity.Dtos.Grant;
using Skoruba.Core.Dtos.Common;

namespace Skoruba.AspNetIdentity.Mappers
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