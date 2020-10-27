// Based on the IdentityServer4.EntityFramework - authors - Brock Allen & Dominick Baier.
// https://github.com/IdentityServer/IdentityServer4.EntityFramework

// Modified by Jan Škoruba

using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Skoruba.Admin.BusinessLogic.Identity.Dtos.Identity;
using Skoruba.Core.ExceptionHandling;
using Skoruba.Admin.EntityFramework.Extensions.Common;

namespace Skoruba.Admin.BusinessLogic.Identity.Mappers
{
    public class IdentityMapperProfile<TUser,TKey>
        : Profile
        where TUser : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
    {
        public IdentityMapperProfile()
        {
            // entity to model
            CreateMap<TUser, UserDto<TKey>>(MemberList.Destination);

            CreateMap<UserLoginInfo, UserProviderDto<TKey>>(MemberList.Destination);

            CreateMap<IdentityError, ViewErrorMessage>(MemberList.Destination)
                .ForMember(x => x.ErrorKey, opt => opt.MapFrom(src => src.Code))
                .ForMember(x => x.ErrorMessage, opt => opt.MapFrom(src => src.Description));

            // entity to model
            CreateMap<IdentityRole<TKey>, RoleDto<TKey>>(MemberList.Destination);

            CreateMap<TUser, TUser>(MemberList.Destination)
                .ForMember(x => x.SecurityStamp, opt => opt.Ignore());

            CreateMap<IdentityRole<TKey>, IdentityRole<TKey>>(MemberList.Destination);

            CreateMap<PagedList<TUser>, PagedList<UserDto<TKey>>>(MemberList.Destination)
                .ForMember(x => x.Users,
                    opt => opt.MapFrom(src => src.Data));

            CreateMap<IdentityUserClaim<TKey>, IdentityUserClaim<TKey>Dto>(MemberList.Destination)
                .ForMember(x => x.ClaimId, opt => opt.MapFrom(src => src.Id));

            CreateMap<IdentityUserClaim<TKey>, PagedList<UserClaimDto<TKey>>>(MemberList.Destination)
                .ForMember(x => x.ClaimId, opt => opt.MapFrom(src => src.Id));

            CreateMap<PagedList<IdentityRole<TKey>>, IdentityRolesDto<TKey>>(MemberList.Destination)
                .ForMember(x => x.Roles,
                    opt => opt.MapFrom(src => src.Data));

            CreateMap<PagedList<IdentityRole<TKey>>, PagedList<UserRoleDto<TKey>>>(MemberList.Destination)
                .ForMember(x => x.Roles,
                    opt => opt.MapFrom(src => src.Data));

            CreateMap<PagedList<IdentityUserClaim<TKey>>, PagedList<UserClaimDto<TKey>>>(MemberList.Destination)
                .ForMember(x => x.Claims,
                    opt => opt.MapFrom(src => src.Data));
            
            CreateMap<PagedList<IdentityRoleClaim<TKey>>, PagedList<RoleClaimDto<TKey>>>(MemberList.Destination)
                .ForMember(x => x.Claims,
                    opt => opt.MapFrom(src => src.Data));

            CreateMap<List<UserLoginInfo>, TUserProvidersDto>(MemberList.Destination)
                .ForMember(x => x.Providers, opt => opt.MapFrom(src => src));

            CreateMap<UserLoginInfo, UserProviderDto<TKey>>(MemberList.Destination);

            CreateMap<IdentityRoleClaim<TKey>, IdentityRoleClaim<TKey>Dto>(MemberList.Destination)
                .ForMember(x => x.ClaimId, opt => opt.MapFrom(src => src.Id));

            CreateMap<IdentityRoleClaim<TKey>, PagedList<RoleClaimDto<TKey>>>(MemberList.Destination)
                .ForMember(x => x.ClaimId, opt => opt.MapFrom(src => src.Id));

            CreateMap<IdentityUserLogin<TKey>, UserProviderDto<TKey>>(MemberList.Destination);

            // model to entity
            CreateMap<RoleDto<TKey>, IdentityRole<TKey>>(MemberList.Source)
                .ForMember(dest => dest.Id, opt => opt.Condition(srs => srs.Id != null)); ;

            CreateMap<PagedList<RoleClaimDto<TKey>>, IdentityRoleClaim<TKey>>(MemberList.Source);

            CreateMap<PagedList<UserClaimDto<TKey>>, IdentityUserClaim<TKey>>(MemberList.Source)
                .ForMember(x => x.Id,
                    opt => opt.MapFrom(src => src.ClaimId));

            // model to entity
            CreateMap<UserDto<TKey>, TUser>(MemberList.Source)
                .ForMember(dest => dest.Id, opt => opt.Condition(srs => srs.Id != null)); ;
        }
    }
}