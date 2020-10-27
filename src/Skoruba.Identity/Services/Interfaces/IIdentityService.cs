using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Skoruba.Admin.BusinessLogic.Identity.Dtos.Identity;
using Skoruba.Admin.BusinessLogic.Shared.Dtos.Common;
using Skoruba.Admin.EntityFramework.Extensions.Common;

namespace Skoruba.Admin.BusinessLogic.Identity.Services.Interfaces
{
    public interface IIdentityService<TUser,TKey>
        where TUser : IdentityUser<TKey>
        where TKey : IEquatable<TKey>                              
    {
        Task<bool> ExistsUserAsync(string userId);

        Task<bool> ExistsRoleAsync(string roleId);

        Task<PagedList<UserDto<TKey>>> GetUsersAsync(string search, int page = 1, int pageSize = 10);
        Task<PagedList<UserDto<TKey>>> GeIdentityRoleUsersAsync(string roleId, string search, int page = 1, int pageSize = 10);
        Task<PagedList<UserDto<TKey>>> GetClaimUsersAsync(string claimType, string claimValue, int page = 1, int pageSize = 10);

        Task<PagedList<RoleDto<TKey>>> GeIdentityRolesAsync(string search, int page = 1, int pageSize = 10);

        Task<(IdentityResult identityResult, TKey roleId)> CreateRoleAsync(RoleDto<TKey> role);

        Task<RoleDto<TKey>> GeIdentityRoleAsync(string roleId);

        Task<List<RoleDto<TKey>>> GeIdentityRolesAsync();

        Task<(IdentityResult identityResult, TKey roleId)> UpdateRoleAsync(RoleDto<TKey> role);

        Task<UserDto<TKey>> GetUserAsync(string userId);

        Task<(IdentityResult identityResult, TKey userId)> CreateUserAsync(UserDto<TKey> user);

        Task<(IdentityResult identityResult, TKey userId)> UpdateUserAsync(UserDto<TKey> user);

        Task<IdentityResult> DeleteUserAsync(string userId, UserDto<TKey> user);

        Task<IdentityResult> CreateUserRoleAsync(PagedList<RoleDto<TKey>> role);

        Task<PagedList<RoleDto<TKey>>> BuildUserRolesViewModel(TKey id, int? page);

        Task<PagedList<RoleDto<TKey>>> GeIdentityUserRolesAsync(string userId, int page = 1,
            int pageSize = 10);

        Task<IdentityResult> DeleteUserRoleAsync(PagedList<RoleDto<TKey>> role);

        Task<PagedList<UserClaimDto<TKey>>> GeIdentityUserClaimsAsync(string userId, int page = 1,
            int pageSize = 10);

        Task<PagedList<UserClaimDto<TKey>>> GeIdentityUserClaimAsync(string userId, int claimId);

        Task<IdentityResult> CreateUserClaimsAsync(PagedList<UserClaimDto<TKey>> claimsDto);

        Task<IdentityResult> UpdateUserClaimsAsync(PagedList<UserClaimDto<TKey>> claimsDto);

        Task<IdentityResult> DeleteUserClaimAsync(PagedList<UserClaimDto<TKey>> claim);

        Task<PagedList<UserProviderDto<TKey>>> GetUserProvidersAsync(string userId);

        TKey ConvertToKeyFromString(string id);

        Task<IdentityResult> DeleteUserProvidersAsync(UserProviderDto<TKey> provider);

        Task<UserProviderDto<TKey>> GetUserProviderAsync(string userId, string providerKey);

        Task<IdentityResult> UserChangePasswordAsync(UserChangePasswordDto<TKey> userPassword);

        Task<IdentityResult> CreateRoleClaimsAsync(PagedList<RoleClaimDto<TKey>> claimsDto);

        Task<IdentityResult> UpdateRoleClaimsAsync(PagedList<RoleClaimDto<TKey>> claimsDto);

        Task<PagedList<RoleClaimDto<TKey>>> GeIdentityRoleClaimsAsync(string roleId, int page = 1, int pageSize = 10);

        Task<PagedList<RoleClaimDto<TKey>>> GeIdentityUserRoleClaimsAsync(string userId, string claimSearchText, int page = 1, int pageSize = 10);

        Task<PagedList<RoleClaimDto<TKey>>> GeIdentityRoleClaimAsync(string roleId, int claimId);

        Task<IdentityResult> DeleteRoleClaimAsync(PagedList<RoleClaimDto<TKey>> role);

        Task<IdentityResult> DeleteRoleAsync(RoleDto<TKey> role);
    }
}