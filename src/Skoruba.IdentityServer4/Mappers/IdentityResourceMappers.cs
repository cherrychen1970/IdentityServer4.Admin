using System.Collections.Generic;
using AutoMapper;
using IdentityServer4.EntityFramework.Entities;
using Skoruba.IdentityServer4.Dtos.Configuration;
using Skoruba.Core.Dtos.Common;

namespace Skoruba.IdentityServer4.Mappers
{
    public static class IdentityResourceMappers
    {
        static IdentityResourceMappers()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<IdentityResourceMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static IdentityResourceDto ToModel(this IdentityResource resource)
        {
            return resource == null ? null : Mapper.Map<IdentityResourceDto>(resource);
        }

        public static List<IdentityResourceDto> ToModel(this List<IdentityResource> resource)
        {
            return resource == null ? null : Mapper.Map<List<IdentityResourceDto>>(resource);
        }

        public static IdentityResource ToEntity(this IdentityResourceDto resource)
        {
            return resource == null ? null : Mapper.Map<IdentityResource>(resource);
        }

        public static IdentityResourcePropertiesDto ToModel(this IdentityResourceProperty identityResourceProperty)
        {
            return Mapper.Map<IdentityResourcePropertiesDto>(identityResourceProperty);
        }

        public static List<IdentityResource> ToEntity(this List<IdentityResourceDto> resource)
        {
            return resource == null ? null : Mapper.Map<List<IdentityResource>>(resource);
        }

        public static IdentityResourceProperty ToEntity(this IdentityResourcePropertiesDto identityResourceProperties)
        {
            return Mapper.Map<IdentityResourceProperty>(identityResourceProperties);
        }
    }
}