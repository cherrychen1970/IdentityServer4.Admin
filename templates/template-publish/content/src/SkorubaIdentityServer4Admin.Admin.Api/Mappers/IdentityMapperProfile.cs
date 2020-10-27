using AutoMapper;
using SkorubaIdentityServer4Admin.Admin.Api.Dtos.Roles;
using SkorubaIdentityServer4Admin.Admin.Api.Dtos.Users;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity;

namespace SkorubaIdentityServer4Admin.Admin.Api.Mappers
{
    public class IdentityMapperProfile<RoleDto<TKey>, IdentityUserRole<TKey>sDto, TKey, IdentityUserClaim<TKey>sDto, IdentityUserClaim<TKey>Dto, UserProviderDto<TKey>, TUserProvidersDto, UserChangePasswordDto<TKey>, IdentityRoleClaim<TKey>Dto, IdentityRoleClaim<TKey>sDto> : Profile
        where IdentityUserClaim<TKey>sDto : UserClaimsDto<IdentityUserClaim<TKey>Dto, TKey>
        where IdentityUserClaim<TKey>Dto : UserClaimDto<TKey>
        where RoleDto<TKey> : RoleDto<TKey>
        where IdentityUserRole<TKey>sDto : UserRolesDto<RoleDto<TKey>, TKey>
        where UserProviderDto<TKey> : UserProviderDto<TKey>
        where TUserProvidersDto : UserProvidersDto<UserProviderDto<TKey>,TKey>
        where UserChangePasswordDto<TKey> : UserChangePasswordDto<TKey>
        where IdentityRoleClaim<TKey>sDto : RoleClaimsDto<IdentityRoleClaim<TKey>Dto, TKey>
        where IdentityRoleClaim<TKey>Dto : RoleClaimDto<TKey>
    {
        public IdentityMapperProfile()
        {
            // entity to model
            CreateMap<IdentityUserClaim<TKey>sDto, UserClaimsApiDto<TKey>>(MemberList.Destination);
            CreateMap<IdentityUserClaim<TKey>sDto, UserClaimApiDto<TKey>>(MemberList.Destination);

            CreateMap<UserClaimApiDto<TKey>, IdentityUserClaim<TKey>sDto>(MemberList.Source);
            CreateMap<IdentityUserClaim<TKey>Dto, UserClaimApiDto<TKey>>(MemberList.Destination);

            CreateMap<IdentityUserRole<TKey>sDto, UserRolesApiDto<RoleDto<TKey>>>(MemberList.Destination);
            CreateMap<UserRoleApiDto<TKey>, IdentityUserRole<TKey>sDto>(MemberList.Destination);

            CreateMap<UserProviderDto<TKey>, UserProviderApiDto<TKey>>(MemberList.Destination);
            CreateMap<TUserProvidersDto, UserProvidersApiDto<TKey>>(MemberList.Destination);
            CreateMap<UserProviderDeleteApiDto<TKey>, UserProviderDto<TKey>>(MemberList.Source);

            CreateMap<UserChangePasswordApiDto<TKey>, UserChangePasswordDto<TKey>>(MemberList.Destination);

            CreateMap<RoleClaimsApiDto<TKey>, IdentityRoleClaim<TKey>sDto>(MemberList.Source);
            CreateMap<RoleClaimApiDto<TKey>, IdentityRoleClaim<TKey>Dto>(MemberList.Destination);
            CreateMap<RoleClaimApiDto<TKey>, IdentityRoleClaim<TKey>sDto>(MemberList.Destination);

            CreateMap<IdentityRoleClaim<TKey>sDto, RoleClaimsApiDto<TKey>>(MemberList.Source);
            CreateMap<IdentityRoleClaim<TKey>Dto, RoleClaimApiDto<TKey>>(MemberList.Destination);
            CreateMap<IdentityRoleClaim<TKey>sDto, RoleClaimApiDto<TKey>>(MemberList.Destination);
        }
    }
}





