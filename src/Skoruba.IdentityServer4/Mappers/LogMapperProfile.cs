using AutoMapper;
using Skoruba.AuditLogging.EntityFramework.Entities;
using Skoruba.IdentityServer4.Dtos.Log;
using Skoruba.EntityFramework.Entities;
using Skoruba.Core.Dtos.Common;

namespace Skoruba.IdentityServer4.Mappers
{
    public class LogMapperProfile : Profile
    {
        public LogMapperProfile()
        {
            CreateMap<Log, LogDto>(MemberList.Destination)
                .ReverseMap();
            
            CreateMap<AuditLog, AuditLogDto>(MemberList.Destination)
                .ReverseMap();
        }
    }
}
