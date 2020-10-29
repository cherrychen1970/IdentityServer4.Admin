using AutoMapper;
using Skoruba.AuditLogging.EntityFramework.Entities;
using Skoruba.IdentityServer4.Models.Log;
using Skoruba.IdentityServer4.EntityFramework.Entities;
using Skoruba.Models;

namespace Skoruba.IdentityServer4.EntityFramework.Mappers
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
