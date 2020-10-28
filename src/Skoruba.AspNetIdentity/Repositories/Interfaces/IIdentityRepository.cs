using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Skoruba.Core.Dtos.Common;

namespace Skoruba.EntityFramework.Identity.Repositories.Interfaces
{
    public interface IIdentityRepository<TKey>
        where TKey : IEquatable<TKey>                        
       {
        Task<bool> ExistsUserAsync(string userId);

        Task<bool> ExistsRoleAsync(string roleId);

        Task<PagedList<IdentityUser<TKey>>> GetUsersAsync(string search, int page = 1, int pageSize = 10);

        Task<PagedList<IdentityUser<TKey>>> GetRoleUsersAsync(string roleId, string search, int page = 1, int pageSize = 10);

        Task<PagedList<IdentityUser<TKey>>> GetClaimUsersAsync(string claimType, string claimValue, int page = 1, int pageSize = 10);

        Task<PagedList<IdentityRole<TKey>>> GetRolesAsync(string search, int page = 1, int pageSize = 10);

        Task<(IdentityResult identityResult, TKey roleId)> CreateRoleAsync(IdentityRole<TKey> role);

        Task<IdentityRole<TKey>> GetRoleAsync(TKey roleId);

        Task<List<IdentityRole<TKey>>> GetRolesAsync();

        Task<(IdentityResult identityResult, TKey roleId)> UpdateRoleAsync(IdentityRole<TKey> role);

        Task<IdentityUser<TKey>> GetUserAsync(string userId);

        Task<(IdentityResult identityResult, TKey userId)> CreateUserAsync(IdentityUser<TKey> user);

        Task<(IdentityResult identityResult, TKey userId)> UpdateUserAsync(IdentityUser<TKey> user);

        Task<IdentityResult> DeleteUserAsync(string userId);

        Task<IdentityResult> CreateUserRoleAsync(string userId, string roleId);

        Task<PagedList<IdentityRole<TKey>>> GetUserRolesAsync(string userId, int page = 1, int pageSize = 10);

        Task<IdentityResult> DeleteUserRoleAsync(string userId, string roleId);

        Task<PagedList<IdentityUserClaim<TKey>>> GetUserClaimsAsync(string userId, int page = 1, int pageSize = 10);

        Task<IdentityUserClaim<TKey>> GetUserClaimAsync(string userId, int claimId);

        Task<IdentityResult> CreateUserClaimsAsync(IdentityUserClaim<TKey> claims);

        Task<IdentityResult> UpdateUserClaimsAsync(IdentityUserClaim<TKey> claims);

        Task<IdentityResult> DeleteUserClaimAsync(string userId, int claimId);

        Task<List<UserLoginInfo>> GetUserProvidersAsync(string userId);

        Task<IdentityResult> DeleteUserProvidersAsync(string userId, string providerKey, string loginProvider);

        Task<IdentityUserLogin<TKey>> GetUserProviderAsync(string userId, string providerKey);

        Task<IdentityResult> UserChangePasswordAsync(string userId, string password);

        Task<IdentityResult> CreateRoleClaimsAsync(IdentityRoleClaim<TKey> claims);

        Task<IdentityResult> UpdateRoleClaimsAsync(IdentityRoleClaim<TKey> claims);

        Task<PagedList<IdentityRoleClaim<TKey>>> GetRoleClaimsAsync(string roleId, int page = 1, int pageSize = 10);

        Task<PagedList<IdentityRoleClaim<TKey>>> GetUserRoleClaimsAsync(string userId, string claimSearchText, int page = 1, int pageSize = 10);

        Task<IdentityRoleClaim<TKey>> GetRoleClaimAsync(string roleId, int claimId);

        Task<IdentityResult> DeleteRoleClaimAsync(string roleId, int claimId);

        Task<IdentityResult> DeleteRoleAsync(IdentityRole<TKey> role);

        bool AutoSaveChanges { get; set; }

        Task<int> SaveAllChangesAsync();
    }
}