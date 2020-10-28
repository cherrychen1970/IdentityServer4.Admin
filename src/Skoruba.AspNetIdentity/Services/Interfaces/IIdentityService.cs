using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Skoruba.AspNetIdentity.Dtos;
using Skoruba.Core.Dtos.Common;
using Skoruba.Core.Dtos.Common;

namespace Skoruba.AspNetIdentity.Services.Interfaces
{
    public interface IIdentityService<TKey>
        where TKey : IEquatable<TKey>                              
    {
        //Task<bool> ExistsUserAsync(string userId);

       // Task<bool> ExistsRoleAsync(string roleId);

        Task<IPagedList<UserDto<TKey>>> GetUsersAsync(string search, int page = 1, int pageSize = 10);
        Task<IPagedList<UserDto<TKey>>> GetRoleUsersAsync(string roleId, string search, int page = 1, int pageSize = 10);
        Task<IPagedList<UserDto<TKey>>> GetClaimUsersAsync(string claimType, string claimValue, int page = 1, int pageSize = 10);

        Task<IPagedList<RoleDto<TKey>>> GetRolesAsync(string search, int page = 1, int pageSize = 10);

        Task<(IdentityResult identityResult, TKey roleId)> CreateRoleAsync(RoleDto<TKey> role);

        Task<RoleDto<TKey>> GetRoleAsync(string roleId);

        Task<List<RoleDto<TKey>>> GetRolesAsync();

        Task<(IdentityResult identityResult, TKey roleId)> UpdateRoleAsync(RoleDto<TKey> role);

        Task<UserDto<TKey>> GetUserAsync(string userId);

        Task<(IdentityResult identityResult, TKey userId)> CreateUserAsync(UserDto<TKey> user);

        Task<(IdentityResult identityResult, TKey userId)> UpdateUserAsync(UserDto<TKey> user);

        Task<IdentityResult> DeleteUserAsync(string userId, UserDto<TKey> user);

        Task<IdentityResult> CreateUserRoleAsync(UserRoleDto<TKey> role);

        //Task<IPagedList<RoleDto<TKey>>> BuildUserRolesViewModel(TKey id, int? page);

        Task<IPagedList<UserRoleDto<TKey>>> GetUserRolesAsync(string userId, int page = 1,int pageSize = 10);

        Task<IdentityResult> DeleteUserRoleAsync(UserRoleDto<TKey> role);

        Task<IPagedList<UserClaimDto<TKey>>> GetUserClaimsAsync(string userId, int page = 1, int pageSize = 10);

        Task<IPagedList<UserClaimDto<TKey>>> GetUserClaimAsync(string userId, int claimId);

        Task<IdentityResult> CreateUserClaimsAsync(IEnumerable<UserClaimDto<TKey>> claimsDto);

        Task<IdentityResult> UpdateUserClaimsAsync(IEnumerable<UserClaimDto<TKey>> claimsDto);

        Task<IdentityResult> DeleteUserClaimAsync(UserClaimDto<TKey> claim);

        Task<IEnumerable<UserProviderDto<TKey>>> GetUserProvidersAsync(string userId);

        TKey ConvertToKeyFromString(string id);

        Task<IdentityResult> DeleteUserProvidersAsync(UserProviderDto<TKey> provider);

        Task<UserProviderDto<TKey>> GetUserProviderAsync(string userId, string providerKey);

        Task<IdentityResult> UserChangePasswordAsync(UserChangePasswordDto<TKey> userPassword);

        Task<IdentityResult> CreateRoleClaimsAsync(IEnumerable<RoleClaimDto<TKey>> claimsDto);

        Task<IdentityResult> UpdateRoleClaimsAsync(IEnumerable<RoleClaimDto<TKey>> claimsDto);

        Task<IPagedList<RoleClaimDto<TKey>>> GetRoleClaimsAsync(string roleId, int page = 1, int pageSize = 10);

        Task<IPagedList<RoleClaimDto<TKey>>> GetUserRoleClaimsAsync(string userId, string claimSearchText, int page = 1, int pageSize = 10);

        Task<IPagedList<RoleClaimDto<TKey>>> GetRoleClaimAsync(string roleId, int claimId);

        Task<IdentityResult> DeleteRoleClaimAsync(RoleClaimDto<TKey> role);

        Task<IdentityResult> DeleteRoleAsync(RoleDto<TKey> role);
    }
}