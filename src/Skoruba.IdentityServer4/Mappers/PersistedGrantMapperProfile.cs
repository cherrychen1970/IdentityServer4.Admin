using AutoMapper;
using IdentityServer4.EntityFramework.Entities;
using Skoruba.IdentityServer4.Dtos.Grant;
using Skoruba.EntityFramework.Entities;
using Skoruba.Core.Dtos.Common;

namespace Skoruba.IdentityServer4.Mappers
{
    public class PersistedGrantMapperProfile : Profile
    {
        public PersistedGrantMapperProfile()
        {
            // entity to model
            CreateMap<PersistedGrant, PersistedGrantDto>(MemberList.Destination)
                .ReverseMap();

            CreateMap<PersistedGrantDataView, PersistedGrantDto>(MemberList.Destination); 
        }
    }
}
