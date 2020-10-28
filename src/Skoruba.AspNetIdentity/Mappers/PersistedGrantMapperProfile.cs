using AutoMapper;
using IdentityServer4.EntityFramework.Entities;
using Skoruba.AspNetIdentity.Dtos.Grant;
using Skoruba.Core.Dtos.Common;

namespace Skoruba.AspNetIdentity.Mappers
{
    public class PersistedGrantMapperProfile : Profile
    {
        public PersistedGrantMapperProfile()
        {
            // entity to model
            CreateMap<PersistedGrant, PersistedGrantDto>(MemberList.Destination)
                .ReverseMap();
            
        }
    }
}
