using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Skoruba.AuditLogging.Services;
using Skoruba.EntityFramework.Extensions.Common;
using Skoruba.Identity.Dtos.Identity;
using Skoruba.Identity.Resources;
using Skoruba.Identity.Services.Interfaces;
using Skoruba.Core.Dtos.Common;
using Skoruba.Core.ExceptionHandling;
using Skoruba.EntityFramework.Identity.Repositories.Interfaces;
using Skoruba.Core.Events;
using Skoruba.Identity.Dtos;

namespace Skoruba.Identity.Services
{
    public class IdentityService<TKey> //: IIdentityService<TKey>
        where TKey : IEquatable<TKey>
    {
        protected readonly IIdentityRepository<TKey> IdentityRepository;
        protected readonly IIdentityServiceResources IdentityServiceResources;
        protected readonly IMapper Mapper;
        protected readonly IAuditEventLogger AuditEventLogger;

        public IdentityService(IIdentityRepository<TKey> identityRepository,
            IIdentityServiceResources identityServiceResources,
            IMapper mapper,
            IAuditEventLogger auditEventLogger)
        {
            IdentityRepository = identityRepository;
            IdentityServiceResources = identityServiceResources;
            Mapper = mapper;
            AuditEventLogger = auditEventLogger;
        }

        public virtual async Task<PagedList<UserDto<TKey>>> GetUsersAsync(string search, int page = 1, int pageSize = 10)
        {
            var pagedList = await IdentityRepository.GetUsersAsync(search, page, pageSize);
            var usersDto = pagedList.Select(x=> Mapper.Map<UserDto<TKey>>(x));
            //var usersDto = Mapper.Map<PagedList<UserDto<TKey>>>(pagedList);

            await AuditEventLogger.LogEventAsync(new CommonEvent(usersDto));

            return usersDto;
        }

        public virtual async Task<PagedList<UserDto<TKey>>> GetRoleUsersAsync(string roleId, string search, int page = 1, int pageSize = 10)
        {
            var roleKey = ConvertToKeyFromString(roleId);

            var userIdentityRole = await IdentityRepository.GetRoleAsync(roleKey);
            if (userIdentityRole == null) throw new UserFriendlyErrorPageException(string.Format(IdentityServiceResources.RoleDoesNotExist().Description, roleId), IdentityServiceResources.RoleDoesNotExist().Description);

            var pagedList = await IdentityRepository.GetRoleUsersAsync(roleId, search, page, pageSize);
            var usersDto = Mapper.Map<PagedList<UserDto<TKey>>>(pagedList);

            await AuditEventLogger.LogEventAsync(new CommonEvent(usersDto));

            return usersDto;
        }

        public virtual async Task<PagedList<UserDto<TKey>>> GetClaimUsersAsync(string claimType, string claimValue, int page = 1, int pageSize = 10)
        {
            var pagedList = await IdentityRepository.GetClaimUsersAsync(claimType, claimValue, page, pageSize);
            var usersDto = Mapper.Map<PagedList<UserDto<TKey>>>(pagedList);

            await AuditEventLogger.LogEventAsync(new CommonEvent(usersDto));

            return usersDto;
        }
        public virtual async Task<PagedList<RoleDto<TKey>>> SearchRolesAsync(string search, int page = 1, int pageSize = 10)
        {
            PagedList<IdentityRole<TKey>> pagedList = await IdentityRepository.GetRolesAsync(search, page, pageSize);
            var rolesDto = pagedList.Select(x=> Mapper.Map<RoleDto<TKey>>(x));

            await AuditEventLogger.LogEventAsync(new CommonEvent(rolesDto));
            return rolesDto;
        }


        public virtual async Task<(IdentityResult identityResult, TKey roleId)> CreateRoleAsync(RoleDto<TKey> role)
        {
            var roleEntity = Mapper.Map<IdentityRole<TKey>>(role);
            var (identityResult, roleId) = await IdentityRepository.CreateRoleAsync(roleEntity);
            var handleIdentityError = HandleIdentityError(identityResult, IdentityServiceResources.RoleCreateFailed().Description, IdentityServiceResources.IdentityErrorKey().Description, role);

            await AuditEventLogger.LogEventAsync(new CommonEvent(role));

            return (handleIdentityError, roleId);
        }

        private IdentityResult HandleIdentityError(IdentityResult identityResult, string errorMessage, string errorKey, object model)
        {
            if (!identityResult.Errors.Any()) return identityResult;
            var viewErrorMessages = Mapper.Map<List<ViewErrorMessage>>(identityResult.Errors);

            throw new UserFriendlyViewException(errorMessage, errorKey, viewErrorMessages, model);
        }

        public virtual async Task<RoleDto<TKey>> GetRoleAsync(string roleId)
        {
            var roleKey = ConvertToKeyFromString(roleId);

            var userIdentityRole = await IdentityRepository.GetRoleAsync(roleKey);
            if (userIdentityRole == null) throw new UserFriendlyErrorPageException(string.Format(IdentityServiceResources.RoleDoesNotExist().Description, roleId), IdentityServiceResources.RoleDoesNotExist().Description);

            var roleDto = Mapper.Map<RoleDto<TKey>>(userIdentityRole);

            await AuditEventLogger.LogEventAsync(new CommonEvent(roleDto));

            return roleDto;
        }

        public virtual async Task<List<RoleDto<TKey>>> GetRolesAsync()
        {
            var roles = await IdentityRepository.GetRolesAsync();
            var roleDtos = Mapper.Map<List<RoleDto<TKey>>>(roles);

            await AuditEventLogger.LogEventAsync(new CommonEvent(roleDtos));

            return roleDtos;
        }

        public virtual async Task<(IdentityResult identityResult, TKey roleId)> UpdateRoleAsync(RoleDto<TKey> role)
        {
            var userIdentityRole = Mapper.Map<IdentityRole<TKey>>(role);

            var originalRole = await GetRoleAsync(role.Id.ToString());

            var (identityResult, roleId) = await IdentityRepository.UpdateRoleAsync(userIdentityRole);

            await AuditEventLogger.LogEventAsync(new CommonEvent(new {originalRole, role}));

            var handleIdentityError = HandleIdentityError(identityResult, IdentityServiceResources.RoleUpdateFailed().Description, IdentityServiceResources.IdentityErrorKey().Description, role);

            return (handleIdentityError, roleId);
        }

        public virtual async Task<UserDto<TKey>> GetUserAsync(string userId)
        {
            var identity = await IdentityRepository.GetUserAsync(userId);
            if (identity == null) throw new UserFriendlyErrorPageException(string.Format(IdentityServiceResources.UserDoesNotExist().Description, userId), IdentityServiceResources.UserDoesNotExist().Description);

            var userDto = Mapper.Map<UserDto<TKey>>(identity);

            await AuditEventLogger.LogEventAsync(new CommonEvent(userDto));

            return userDto;
        }

        public virtual async Task<(IdentityResult identityResult, TKey userId)> CreateUserAsync(UserDto<TKey> user)
        {
            var userIdentity = Mapper.Map<IdentityUser<TKey>>(user);
            var (identityResult, userId) = await IdentityRepository.CreateUserAsync(userIdentity);

            var handleIdentityError = HandleIdentityError(identityResult, IdentityServiceResources.UserCreateFailed().Description, IdentityServiceResources.IdentityErrorKey().Description, user);

            await AuditEventLogger.LogEventAsync(new CommonEvent(user));

            return (handleIdentityError, userId);
        }

        /// <summary>
        /// Updates the specified user, but without updating the password hash value
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual async Task<(IdentityResult identityResult, TKey userId)> UpdateUserAsync(UserDto<TKey> user)
        {
            var userIdentity = Mapper.Map<IdentityUser<TKey>>(user);
            await MapOriginalPasswordHashAsync(userIdentity);

            var originalUser = await GetUserAsync(user.Id.ToString());

            var (identityResult, userId) = await IdentityRepository.UpdateUserAsync(userIdentity);
            var handleIdentityError = HandleIdentityError(identityResult, IdentityServiceResources.UserUpdateFailed().Description, IdentityServiceResources.IdentityErrorKey().Description, user);

            await AuditEventLogger.LogEventAsync(new CommonEvent( new {originalUser, user}));

            return (handleIdentityError, userId);
        }

        /// <summary>
        /// Get original password hash and map password hash to user
        /// </summary>
        /// <param name="userIdentity"></param>
        /// <returns></returns>
        private async Task MapOriginalPasswordHashAsync(IdentityUser<TKey> userIdentity)
        {
            var identity = await IdentityRepository.GetUserAsync(userIdentity.Id.ToString());
            userIdentity.PasswordHash = identity.PasswordHash;
        }

        public virtual async Task<IdentityResult> DeleteUserAsync(string userId, UserDto<TKey> user)
        {
            var identityResult = await IdentityRepository.DeleteUserAsync(userId);

            await AuditEventLogger.LogEventAsync(new CommonEvent(user));

            return HandleIdentityError(identityResult, IdentityServiceResources.UserDeleteFailed().Description, IdentityServiceResources.IdentityErrorKey().Description, user);
        }

        public virtual async Task<IdentityResult> CreateUserRoleAsync(UserRoleDto<TKey> role)
        {
            var identityResult = await IdentityRepository.CreateUserRoleAsync(role.UserId.ToString(), role.RoleId.ToString());

            await AuditEventLogger.LogEventAsync(new CommonEvent(role));

            if (!identityResult.Errors.Any()) return identityResult;

            var userRolesDto = await BuildUserRolesViewModel(role.UserId, 1);

            return HandleIdentityError(identityResult, IdentityServiceResources.UserRoleCreateFailed().Description, IdentityServiceResources.IdentityErrorKey().Description, userRolesDto);
        }


        public virtual async Task<RoleDto<TKey>> BuildUserRolesViewModel(TKey id, int? page)
        {
            return default(RoleDto<TKey>);
#if false
            var roles = await GetRolesAsync();
            var userRoles = await GetRolesAsync(id.ToString(), page ?? 1);
            userRoles.UserId = id;
            userRoles.RolesList = roles.Select(x => new SelectItemDto(x.Id.ToString(), x.Name)).ToList();

            return userRoles;
#endif
        }

        public virtual async Task<PagedList<RoleDto<TKey>>> GetRolesAsync(string userId, int page = 1, int pageSize = 10)
        {
            var userExists = await IdentityRepository.ExistsUserAsync(userId);
            if (!userExists) throw new UserFriendlyErrorPageException(string.Format(IdentityServiceResources.UserDoesNotExist().Description, userId), IdentityServiceResources.UserDoesNotExist().Description);

            var userIdentityRoles = await IdentityRepository.GetRolesAsync(userId, page, pageSize);
            var roleDtos = Mapper.Map<PagedList<RoleDto<TKey>>>(userIdentityRoles);

            var user = await IdentityRepository.GetUserAsync(userId);

            await AuditEventLogger.LogEventAsync(new CommonEvent(roleDtos));

            return roleDtos;
        }

        public virtual async Task<IdentityResult> DeleteUserRoleAsync(UserRoleDto<TKey> role)
        {
            var identityResult = await IdentityRepository.DeleteUserRoleAsync(role.UserId.ToString(), role.RoleId.ToString());

            await AuditEventLogger.LogEventAsync(new CommonEvent(role));

            return HandleIdentityError(identityResult, IdentityServiceResources.UserRoleDeleteFailed().Description, IdentityServiceResources.IdentityErrorKey().Description, role);
        }

        public virtual async Task<PagedList<UserClaimDto<TKey>>> GetUserClaimsAsync(string userId, int page = 1, int pageSize = 10)
        {
            var userExists = await IdentityRepository.ExistsUserAsync(userId);
            if (!userExists) throw new UserFriendlyErrorPageException(string.Format(IdentityServiceResources.UserDoesNotExist().Description, userId), IdentityServiceResources.UserDoesNotExist().Description);

            var claims = await IdentityRepository.GetUserClaimsAsync(userId, page, pageSize);
            var claimDtos = Mapper.Map<PagedList<UserClaimDto<TKey>>>(claims);
            var user = await IdentityRepository.GetUserAsync(userId);
            // CHERRY
            // .UserName=

            await AuditEventLogger.LogEventAsync(new CommonEvent(claimDtos));

            return claimDtos;
        }

        public virtual async Task<PagedList<UserClaimDto<TKey>>> GetUserClaimAsync(string userId, int claimId)
        {
            var userExists = await IdentityRepository.ExistsUserAsync(userId);
            if (!userExists) throw new UserFriendlyErrorPageException(string.Format(IdentityServiceResources.UserDoesNotExist().Description, userId), IdentityServiceResources.UserDoesNotExist().Description);

            var identityUserClaim = await IdentityRepository.GetUserClaimAsync(userId, claimId);
            if (identityUserClaim == null) throw new UserFriendlyErrorPageException(string.Format(IdentityServiceResources.UserClaimDoesNotExist().Description, userId), IdentityServiceResources.UserClaimDoesNotExist().Description);

            var userClaimsDto = Mapper.Map<PagedList<UserClaimDto<TKey>>>(identityUserClaim);

            await AuditEventLogger.LogEventAsync(new CommonEvent(userClaimsDto));

            return userClaimsDto;
        }

        public virtual async Task<IdentityResult> CreateUserClaimsAsync(PagedList<UserClaimDto<TKey>> claimsDto)
        {
            var userIdentityUserClaim = Mapper.Map<IdentityUserClaim<TKey>>(claimsDto);
            var identityResult = await IdentityRepository.CreateUserClaimsAsync(userIdentityUserClaim);

            await AuditEventLogger.LogEventAsync(new CommonEvent(claimsDto));

            return HandleIdentityError(identityResult, IdentityServiceResources.UserClaimsCreateFailed().Description, IdentityServiceResources.IdentityErrorKey().Description, claimsDto);
        }

        public virtual async Task<IdentityResult> UpdateUserClaimsAsync(PagedList<UserClaimDto<TKey>> claimsDto)
        {
            var userIdentityUserClaim = Mapper.Map<IdentityUserClaim<TKey>>(claimsDto);
            var identityResult = await IdentityRepository.UpdateUserClaimsAsync(userIdentityUserClaim);

            await AuditEventLogger.LogEventAsync(new CommonEvent(claimsDto));

            return HandleIdentityError(identityResult, IdentityServiceResources.UserClaimsUpdateFailed().Description, IdentityServiceResources.IdentityErrorKey().Description, claimsDto);
        }

        public virtual async Task<IdentityResult> DeleteUserClaimAsync(UserClaimDto<TKey> claim)
        {
            var deleted = await IdentityRepository.DeleteUserClaimAsync(claim.UserId.ToString(), claim.Id);

            await AuditEventLogger.LogEventAsync(new CommonEvent(claim));

            return deleted;
        }

        public virtual TKey ConvertToKeyFromString(string id)
        {
            if (id == null)
            {
                return default(TKey);
            }
            return (TKey)TypeDescriptor.GetConverter(typeof(TKey)).ConvertFromInvariantString(id);
        }

        public virtual async Task<IEnumerable<UserProviderDto<TKey>>> GetUserProvidersAsync(string userId)
        {
            var userExists = await IdentityRepository.ExistsUserAsync(userId);
            if (!userExists) throw new UserFriendlyErrorPageException(string.Format(IdentityServiceResources.UserDoesNotExist().Description, userId), IdentityServiceResources.UserDoesNotExist().Description);

            var userLoginInfos = await IdentityRepository.GetUserProvidersAsync(userId);
            var providersDto =  userLoginInfos.Select(x=>Mapper.Map<UserProviderDto<TKey>>(x));

            var user = await IdentityRepository.GetUserAsync(userId);
            //providersDto.UserName = user.UserName;

            await AuditEventLogger.LogEventAsync(new CommonEvent(providersDto));

            return providersDto;
        }

        public virtual async Task<IdentityResult> DeleteUserProvidersAsync(UserProviderDto<TKey> provider)
        {
            var identityResult = await IdentityRepository.DeleteUserProvidersAsync(provider.UserId.ToString(), provider.ProviderKey, provider.LoginProvider);

            await AuditEventLogger.LogEventAsync(new CommonEvent(provider));

            return HandleIdentityError(identityResult, IdentityServiceResources.UserProviderDeleteFailed().Description, IdentityServiceResources.IdentityErrorKey().Description, provider);
        }

        public virtual async Task<UserProviderDto<TKey>> GetUserProviderAsync(string userId, string providerKey)
        {
            var userExists = await IdentityRepository.ExistsUserAsync(userId);
            if (!userExists) throw new UserFriendlyErrorPageException(string.Format(IdentityServiceResources.UserDoesNotExist().Description, userId), IdentityServiceResources.UserDoesNotExist().Description);

            var identityUserLogin = await IdentityRepository.GetUserProviderAsync(userId, providerKey);
            if (identityUserLogin == null) throw new UserFriendlyErrorPageException(string.Format(IdentityServiceResources.UserProviderDoesNotExist().Description, providerKey), IdentityServiceResources.UserProviderDoesNotExist().Description);

            var userProviderDto = Mapper.Map<UserProviderDto<TKey>>(identityUserLogin);
            var user = await GetUserAsync(userId);
            userProviderDto.UserName = user.UserName;

            await AuditEventLogger.LogEventAsync(new CommonEvent(userProviderDto));

            return userProviderDto;
        }

        public virtual async Task<IdentityResult> UserChangePasswordAsync(UserChangePasswordDto<TKey> userPassword)
        {
            var userExists = await IdentityRepository.ExistsUserAsync(userPassword.UserId.ToString());
            if (!userExists) throw new UserFriendlyErrorPageException(string.Format(IdentityServiceResources.UserDoesNotExist().Description, userPassword.UserId), IdentityServiceResources.UserDoesNotExist().Description);

            var identityResult = await IdentityRepository.UserChangePasswordAsync(userPassword.UserId.ToString(), userPassword.Password);

            await AuditEventLogger.LogEventAsync(new CommonEvent(userPassword.UserName));

            return HandleIdentityError(identityResult, IdentityServiceResources.UserChangePasswordFailed().Description, IdentityServiceResources.IdentityErrorKey().Description, userPassword);
        }

        public virtual async Task<IdentityResult> CreateRoleClaimsAsync(PagedList<RoleClaimDto<TKey>> claimsDto)
        {
            var identityRoleClaim = Mapper.Map<IdentityRoleClaim<TKey>>(claimsDto);
            var identityResult = await IdentityRepository.CreateRoleClaimsAsync(identityRoleClaim);

            await AuditEventLogger.LogEventAsync(new CommonEvent(claimsDto));

            return HandleIdentityError(identityResult, IdentityServiceResources.RoleClaimsCreateFailed().Description, IdentityServiceResources.IdentityErrorKey().Description, claimsDto);
        }

        public virtual async Task<IdentityResult> UpdateRoleClaimsAsync(PagedList<RoleClaimDto<TKey>> claimsDto)
        {
            var identityRoleClaim = Mapper.Map<IdentityRoleClaim<TKey>>(claimsDto);
            var identityResult = await IdentityRepository.UpdateRoleClaimsAsync(identityRoleClaim);

            await AuditEventLogger.LogEventAsync(new CommonEvent(claimsDto));

            return HandleIdentityError(identityResult, IdentityServiceResources.RoleClaimsUpdateFailed().Description, IdentityServiceResources.IdentityErrorKey().Description, claimsDto);
        }

        public virtual async Task<PagedList<RoleClaimDto<TKey>>> GetRoleClaimsAsync(string roleId, int page = 1, int pageSize = 10)
        {
            var roleExists = await IdentityRepository.ExistsRoleAsync(roleId);
            if (!roleExists) throw new UserFriendlyErrorPageException(string.Format(IdentityServiceResources.RoleDoesNotExist().Description, roleId), IdentityServiceResources.RoleDoesNotExist().Description);

            var identityRoleClaims = await IdentityRepository.GetRoleClaimsAsync(roleId, page, pageSize);
            var roleClaimDtos = Mapper.Map<PagedList<RoleClaimDto<TKey>>>(identityRoleClaims);
            var roleDto = await GetRoleAsync(roleId);
            //roleClaimDtos.RoleName = roleDto.Name;

            await AuditEventLogger.LogEventAsync(new CommonEvent(roleClaimDtos));

            return roleClaimDtos;
        }

        public virtual async Task<PagedList<RoleClaimDto<TKey>>> GetUserRoleClaimsAsync(string userId, string claimSearchText, int page = 1, int pageSize = 10)
        {
            var userExists = await IdentityRepository.ExistsUserAsync(userId);
            if (!userExists) throw new UserFriendlyErrorPageException(string.Format(IdentityServiceResources.UserDoesNotExist().Description, userId), IdentityServiceResources.UserDoesNotExist().Description);

            var identityRoleClaims = await IdentityRepository.GetUserRoleClaimsAsync(userId, claimSearchText, page, pageSize);
            var roleClaimDtos = Mapper.Map<PagedList<RoleClaimDto<TKey>>>(identityRoleClaims);

            return roleClaimDtos;
        }

        public virtual async Task<PagedList<RoleClaimDto<TKey>>> GetRoleClaimAsync(string roleId, int claimId)
        {
            var roleExists = await IdentityRepository.ExistsRoleAsync(roleId);
            if (!roleExists) throw new UserFriendlyErrorPageException(string.Format(IdentityServiceResources.RoleDoesNotExist().Description, roleId), IdentityServiceResources.RoleDoesNotExist().Description);

            var identityRoleClaim = await IdentityRepository.GetRoleClaimAsync(roleId, claimId);
            if (identityRoleClaim == null) throw new UserFriendlyErrorPageException(string.Format(IdentityServiceResources.RoleClaimDoesNotExist().Description, claimId), IdentityServiceResources.RoleClaimDoesNotExist().Description);
            var roleClaimsDto = Mapper.Map<PagedList<RoleClaimDto<TKey>>>(identityRoleClaim);
            var roleDto = await GetRoleAsync(roleId);
            //roleClaimsDto.RoleName = roleDto.Name;

            await AuditEventLogger.LogEventAsync(new CommonEvent(roleClaimsDto));

            return roleClaimsDto;
        }

        public virtual async Task<IdentityResult> DeleteRoleClaimAsync(RoleClaimDto<TKey> role)
        {
            var deleted = await IdentityRepository.DeleteRoleClaimAsync(role.RoleId.ToString(), role.Id);

            await AuditEventLogger.LogEventAsync(new CommonEvent(role));

            return deleted;
        }

        public virtual async Task<IdentityResult> DeleteRoleAsync(RoleDto<TKey> role)
        {
            var userIdentityRole = Mapper.Map<IdentityRole<TKey>>(role);
            var identityResult = await IdentityRepository.DeleteRoleAsync(userIdentityRole);

            await AuditEventLogger.LogEventAsync(new CommonEvent(role));

            return HandleIdentityError(identityResult, IdentityServiceResources.RoleDeleteFailed().Description, IdentityServiceResources.IdentityErrorKey().Description, role);
        }
    }
}