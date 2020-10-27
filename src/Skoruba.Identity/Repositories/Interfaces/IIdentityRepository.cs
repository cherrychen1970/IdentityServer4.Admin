using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Skoruba.Admin.EntityFramework.Extensions.Common;

namespace Skoruba.Admin.EntityFramework.Identity.Repositories.Interfaces
{
    public interface IIdentityRepository<TUser, TKey>
        where TUser : IdentityUser<TKey>        
        where TKey : IEquatable<TKey>                        
       {
        Task<bool> ExistsUserAsync(string userId);

        Task<bool> ExistsRoleAsync(string roleId);

        Task<PagedList<TUser>> GetUsersAsync(string search, int page = 1, int pageSize = 10);

        Task<PagedList<TUser>> GeIdentityRoleUsersAsync(string roleId, string search, int page = 1, int pageSize = 10);

        Task<PagedList<TUser>> GetClaimUsersAsync(string claimType, string claimValue, int page = 1, int pageSize = 10);

        Task<PagedList<IdentityRole<TKey>>> GeIdentityRolesAsync(string search, int page = 1, int pageSize = 10);

        Task<(IdentityResult identityResult, TKey roleId)> CreateRoleAsync(IdentityRole<TKey> role);

        Task<IdentityRole<TKey>> GeIdentityRoleAsync(TKey roleId);

        Task<List<IdentityRole<TKey>>> GeIdentityRolesAsync();

        Task<(IdentityResult identityResult, TKey roleId)> UpdateRoleAsync(IdentityRole<TKey> role);

        Task<TUser> GetUserAsync(string userId);

        Task<(IdentityResult identityResult, TKey userId)> CreateUserAsync(TUser user);

        Task<(IdentityResult identityResult, TKey userId)> UpdateUserAsync(TUser user);

        Task<IdentityResult> DeleteUserAsync(string userId);

        Task<IdentityResult> CreateUserRoleAsync(string userId, string roleId);

        Task<PagedList<IdentityRole<TKey>>> GeIdentityUserRolesAsync(string userId, int page = 1, int pageSize = 10);

        Task<IdentityResult> DeleteUserRoleAsync(string userId, string roleId);

        Task<PagedList<IdentityUserClaim<TKey>>> GeIdentityUserClaimsAsync(string userId, int page = 1, int pageSize = 10);

        Task<IdentityUserClaim<TKey>> GeIdentityUserClaimAsync(string userId, int claimId);

        Task<IdentityResult> CreateUserClaimsAsync(IdentityUserClaim<TKey> claims);

        Task<IdentityResult> UpdateUserClaimsAsync(IdentityUserClaim<TKey> claims);

        Task<IdentityResult> DeleteUserClaimAsync(string userId, int claimId);

        Task<List<UserLoginInfo>> GetUserProvidersAsync(string userId);

        Task<IdentityResult> DeleteUserProvidersAsync(string userId, string providerKey, string loginProvider);

        Task<IdentityUserLogin<TKey>> GetUserProviderAsync(string userId, string providerKey);

        Task<IdentityResult> UserChangePasswordAsync(string userId, string password);

        Task<IdentityResult> CreateRoleClaimsAsync(IdentityRoleClaim<TKey> claims);

        Task<IdentityResult> UpdateRoleClaimsAsync(IdentityRoleClaim<TKey> claims);

        Task<PagedList<IdentityRoleClaim<TKey>>> GeIdentityRoleClaimsAsync(string roleId, int page = 1, int pageSize = 10);

        Task<PagedList<IdentityRoleClaim<TKey>>> GeIdentityUserRoleClaimsAsync(string userId, string claimSearchText, int page = 1, int pageSize = 10);

        Task<IdentityRoleClaim<TKey>> GeIdentityRoleClaimAsync(string roleId, int claimId);

        Task<IdentityResult> DeleteRoleClaimAsync(string roleId, int claimId);

        Task<IdentityResult> DeleteRoleAsync(IdentityRole<TKey> role);

        bool AutoSaveChanges { get; set; }

        Task<int> SaveAllChangesAsync();
    }
}