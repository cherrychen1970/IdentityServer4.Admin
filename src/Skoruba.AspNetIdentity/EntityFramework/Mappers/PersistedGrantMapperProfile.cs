using AutoMapper;
using IdentityServer4.EntityFramework.Entities;
using Skoruba.AspNetIdentity.Models;
using Skoruba.Models;

namespace Skoruba.AspNetIdentity.EntityFramework.Mappers
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
