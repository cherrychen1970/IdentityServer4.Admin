// Based on the IdentityServer4.EntityFramework - authors - Brock Allen & Dominick Baier.
// https://github.com/IdentityServer/IdentityServer4.EntityFramework

// Modified by Jan Škoruba

using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Skoruba.AspNetIdentity.Models;
using Skoruba.ExceptionHandling;
using Skoruba.Models;

namespace Skoruba.AspNetIdentity.EntityFramework.Mappers
{
    public class IdentityMapperProfile<TKey>
        : Profile
        where TKey : IEquatable<TKey>
    {
        public IdentityMapperProfile()
        {
            // entity to model
            CreateMap<IdentityUser<TKey>, UserDto<TKey>>(MemberList.Destination);

            CreateMap<UserLoginInfo, UserProviderDto<TKey>>(MemberList.Destination);

            CreateMap<IdentityError, ViewErrorMessage>(MemberList.Destination)
                .ForMember(x => x.ErrorKey, opt => opt.MapFrom(src => src.Code))
                .ForMember(x => x.ErrorMessage, opt => opt.MapFrom(src => src.Description));

            // entity to model
            CreateMap<IdentityRole<TKey>, RoleDto<TKey>>(MemberList.Destination);

            CreateMap<IdentityUser<TKey>, IdentityUser<TKey>>(MemberList.Destination)
                .ForMember(x => x.SecurityStamp, opt => opt.Ignore());

            CreateMap<IdentityRole<TKey>, IdentityRole<TKey>>(MemberList.Destination);

            CreateMap<IdentityUserClaim<TKey>, UserClaimDto<TKey>>(MemberList.Destination);
                //.ForMember(x => x.ClaimId, opt => opt.MapFrom(src => src.Id));

            CreateMap<IdentityUserClaim<TKey>, UserClaimDto<TKey>>(MemberList.Destination);
                //.ForMember(x => x.ClaimId, opt => opt.MapFrom(src => src.Id));

              CreateMap<UserLoginInfo, UserProviderDto<TKey>>(MemberList.Destination);

            CreateMap<IdentityRoleClaim<TKey>, RoleClaimDto<TKey>>(MemberList.Destination);
                //.ForMember(x => x.ClaimId, opt => opt.MapFrom(src => src.Id));

            CreateMap<IdentityUserLogin<TKey>, UserProviderDto<TKey>>(MemberList.Destination);

            // model to entity
            CreateMap<RoleDto<TKey>, IdentityRole<TKey>>(MemberList.Source)
                .ForMember(dest => dest.Id, opt => opt.Condition(srs => srs.Id != null)); ;

            // model to entity
            CreateMap<UserDto<TKey>, IdentityUser<TKey>>(MemberList.Source)
                .ForMember(dest => dest.Id, opt => opt.Condition(srs => srs.Id != null)); ;
        }
    }
}