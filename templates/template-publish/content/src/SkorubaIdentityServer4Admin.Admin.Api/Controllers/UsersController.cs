using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using IdentityModel;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SkorubaIdentityServer4Admin.Admin.Api.Configuration.Constants;
using SkorubaIdentityServer4Admin.Admin.Api.Dtos.Roles;
using SkorubaIdentityServer4Admin.Admin.Api.Dtos.Users;
using SkorubaIdentityServer4Admin.Admin.Api.ExceptionHandling;
using SkorubaIdentityServer4Admin.Admin.Api.Helpers.Localization;
using SkorubaIdentityServer4Admin.Admin.Api.Resources;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Services.Interfaces;

namespace SkorubaIdentityServer4Admin.Admin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(ControllerExceptionFilterAttribute))]
    [Produces("application/json", "application/problem+json")]
    [Authorize(Policy = AuthorizationConsts.AdministrationPolicy)]
    public class UsersController<TUserDto, RoleDto<TKey>, TUser, IdentityRole<TKey>, TKey, IdentityUserClaim<TKey>, IdentityUserRole<TKey>, IdentityUserLogin<TKey>, IdentityRoleClaim<TKey>, IdentityUserToken<TKey>,
            TUsersDto, IdentityRolesDto<TKey>, IdentityUserRole<TKey>sDto, IdentityUserClaim<TKey>sDto,
            UserProviderDto<TKey>, TUserProvidersDto, UserChangePasswordDto<TKey>, IdentityRoleClaim<TKey>sDto, IdentityUserClaim<TKey>Dto, IdentityRoleClaim<TKey>Dto> : ControllerBase
        where TUserDto : UserDto<TKey>, new()
        where RoleDto<TKey> : RoleDto<TKey>, new()
        where TUser : IdentityUser<TKey>
        where IdentityRole<TKey> : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
        where IdentityUserClaim<TKey> : IdentityUserClaim<TKey>
        where IdentityUserRole<TKey> : IdentityUserRole<TKey>
        where IdentityUserLogin<TKey> : IdentityUserLogin<TKey>
        where IdentityRoleClaim<TKey> : IdentityRoleClaim<TKey>
        where IdentityUserToken<TKey> : IdentityUserToken<TKey>
        where TUsersDto : UsersDto<TUserDto, TKey>
        where IdentityRolesDto<TKey> : RolesDto<RoleDto<TKey>, TKey>
        where IdentityUserRole<TKey>sDto : UserRolesDto<RoleDto<TKey>, TKey>
        where IdentityUserClaim<TKey>sDto : UserClaimsDto<IdentityUserClaim<TKey>Dto, TKey>, new()
        where UserProviderDto<TKey> : UserProviderDto<TKey>
        where TUserProvidersDto : UserProvidersDto<UserProviderDto<TKey>, TKey>
        where UserChangePasswordDto<TKey> : UserChangePasswordDto<TKey>
        where IdentityRoleClaim<TKey>sDto : RoleClaimsDto<IdentityRoleClaim<TKey>Dto, TKey>
        where IdentityUserClaim<TKey>Dto : UserClaimDto<TKey>
        where IdentityRoleClaim<TKey>Dto : RoleClaimDto<TKey>
    {
        private readonly IIdentityService<TUserDto, RoleDto<TKey>, TUser, IdentityRole<TKey>, TKey, IdentityUserClaim<TKey>, IdentityUserRole<TKey>, IdentityUserLogin<TKey>, IdentityRoleClaim<TKey>, IdentityUserToken<TKey>,
            TUsersDto, IdentityRolesDto<TKey>, IdentityUserRole<TKey>sDto, IdentityUserClaim<TKey>sDto,
            UserProviderDto<TKey>, TUserProvidersDto, UserChangePasswordDto<TKey>, IdentityRoleClaim<TKey>sDto, IdentityUserClaim<TKey>Dto, IdentityRoleClaim<TKey>Dto> _identityService;
        private readonly IGenericControllerLocalizer<UsersController<TUserDto, RoleDto<TKey>, TUser, IdentityRole<TKey>, TKey, IdentityUserClaim<TKey>, IdentityUserRole<TKey>, IdentityUserLogin<TKey>, IdentityRoleClaim<TKey>, IdentityUserToken<TKey>,
            TUsersDto, IdentityRolesDto<TKey>, IdentityUserRole<TKey>sDto, IdentityUserClaim<TKey>sDto,
            UserProviderDto<TKey>, TUserProvidersDto, UserChangePasswordDto<TKey>, IdentityRoleClaim<TKey>sDto, IdentityUserClaim<TKey>Dto, IdentityRoleClaim<TKey>Dto>> _localizer;

        private readonly IMapper _mapper;
        private readonly IApiErrorResources _errorResources;

        public UsersController(IIdentityService<TUserDto, RoleDto<TKey>, TUser, IdentityRole<TKey>, TKey, IdentityUserClaim<TKey>, IdentityUserRole<TKey>, IdentityUserLogin<TKey>, IdentityRoleClaim<TKey>, IdentityUserToken<TKey>,
                TUsersDto, IdentityRolesDto<TKey>, IdentityUserRole<TKey>sDto, IdentityUserClaim<TKey>sDto,
                UserProviderDto<TKey>, TUserProvidersDto, UserChangePasswordDto<TKey>, IdentityRoleClaim<TKey>sDto, IdentityUserClaim<TKey>Dto, IdentityRoleClaim<TKey>Dto> identityService,
            IGenericControllerLocalizer<UsersController<TUserDto, RoleDto<TKey>, TUser, IdentityRole<TKey>, TKey, IdentityUserClaim<TKey>, IdentityUserRole<TKey>, IdentityUserLogin<TKey>, IdentityRoleClaim<TKey>, IdentityUserToken<TKey>,
                TUsersDto, IdentityRolesDto<TKey>, IdentityUserRole<TKey>sDto, IdentityUserClaim<TKey>sDto,
                UserProviderDto<TKey>, TUserProvidersDto, UserChangePasswordDto<TKey>, IdentityRoleClaim<TKey>sDto, IdentityUserClaim<TKey>Dto, IdentityRoleClaim<TKey>Dto>> localizer, IMapper mapper, IApiErrorResources errorResources)
        {
            _identityService = identityService;
            _localizer = localizer;
            _mapper = mapper;
            _errorResources = errorResources;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TUserDto>> Get(TKey id)
        {
            var user = await _identityService.GetUserAsync(id.ToString());

            return Ok(user);
        }

        [HttpGet]
        public async Task<ActionResult<TUsersDto>> Get(string searchText, int page = 1, int pageSize = 10)
        {
            var usersDto = await _identityService.GetUsersAsync(searchText, page, pageSize);

            return Ok(usersDto);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<TUserDto>> Post([FromBody]TUserDto user)
        {
            if (!EqualityComparer<TKey>.Default.Equals(user.Id, default))
            {
                return BadRequest(_errorResources.CannotSetId());
            }

            var (identityResult, userId) = await _identityService.CreateUserAsync(user);
            var createdUser = await _identityService.GetUserAsync(userId.ToString());

            return CreatedAtAction(nameof(Get), new { id = createdUser.Id }, createdUser);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]TUserDto user)
        {
            await _identityService.GetUserAsync(user.Id.ToString());
            await _identityService.UpdateUserAsync(user);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(TKey id)
        {
            if (IsDeleteForbidden(id))
                return StatusCode((int)System.Net.HttpStatusCode.Forbidden);

            var user = new TUserDto { Id = id };

            await _identityService.GetUserAsync(user.Id.ToString());
            await _identityService.DeleteUserAsync(user.Id.ToString(), user);

            return Ok();
        }

        private bool IsDeleteForbidden(TKey id)
        {
            var userId = User.FindFirst(JwtClaimTypes.Subject);

            return userId == null ? false : userId.Value == id.ToString();
        }

        [HttpGet("{id}/Roles")]
        public async Task<ActionResult<UserRolesApiDto<RoleDto<TKey>>>> GeIdentityUserRole<TKey>s(TKey id, int page = 1, int pageSize = 10)
        {
            var userRoles = await _identityService.GeIdentityUserRolesAsync(id.ToString(), page, pageSize);
            var userRolesApiDto = _mapper.Map<UserRolesApiDto<RoleDto<TKey>>>(userRoles);

            return Ok(userRolesApiDto);
        }

        [HttpPost("Roles")]
        public async Task<IActionResult> PosIdentityUserRole<TKey>s([FromBody]UserRoleApiDto<TKey> role)
        {
            var userRolesDto = _mapper.Map<IdentityUserRole<TKey>sDto>(role);

            await _identityService.GetUserAsync(userRolesDto.UserId.ToString());
            await _identityService.GeIdentityRoleAsync(userRolesDto.RoleId.ToString());
            
            await _identityService.CreateUserRoleAsync(userRolesDto);

            return Ok();
        }

        [HttpDelete("Roles")]
        public async Task<IActionResult> DeleteUserRoles([FromBody]UserRoleApiDto<TKey> role)
        {
            var userRolesDto = _mapper.Map<IdentityUserRole<TKey>sDto>(role);

            await _identityService.GetUserAsync(userRolesDto.UserId.ToString());
            await _identityService.GeIdentityRoleAsync(userRolesDto.RoleId.ToString());

            await _identityService.DeleteUserRoleAsync(userRolesDto);

            return Ok();
        }

        [HttpGet("{id}/Claims")]
        public async Task<ActionResult<UserClaimsApiDto<TKey>>> GeIdentityUserClaim<TKey>s(TKey id, int page = 1, int pageSize = 10)
        {
            var claims = await _identityService.GeIdentityUserClaim<TKey>sAsync(id.ToString(), page, pageSize);

            var userClaimsApiDto = _mapper.Map<UserClaimsApiDto<TKey>>(claims);

            return Ok(userClaimsApiDto);
        }

        [HttpPost("Claims")]
        public async Task<IActionResult> PosIdentityUserClaim<TKey>s([FromBody]UserClaimApiDto<TKey> claim)
        {
            var userClaimDto = _mapper.Map<IdentityUserClaim<TKey>sDto>(claim);

            if (!userClaimDto.ClaimId.Equals(default))
            {
                return BadRequest(_errorResources.CannotSetId());
            }

            await _identityService.CreateUserClaimsAsync(userClaimDto);

            return Ok();
        }

        [HttpPut("Claims")]
        public async Task<IActionResult> PuIdentityUserClaim<TKey>s([FromBody]UserClaimApiDto<TKey> claim)
        {
            var userClaimDto = _mapper.Map<IdentityUserClaim<TKey>sDto>(claim);

            if (!userClaimDto.ClaimId.Equals(default))
            {
                return BadRequest(_errorResources.CannotSetId());
            }

            await _identityService.UpdateUserClaimsAsync(userClaimDto);

            return Ok();
        }

        [HttpDelete("{id}/Claims")]
        public async Task<IActionResult> DeleteUserClaims([FromRoute]TKey id, int claimId)
        {
            var userClaimsDto = new IdentityUserClaim<TKey>sDto
            {
                ClaimId = claimId,
                UserId = id
            };

            await _identityService.GeIdentityUserClaimAsync(id.ToString(), claimId);
            await _identityService.DeleteUserClaimAsync(userClaimsDto);

            return Ok();
        }

        [HttpGet("{id}/Providers")]
        public async Task<ActionResult<UserProvidersApiDto<TKey>>> GetUserProviders(TKey id)
        {
            var userProvidersDto = await _identityService.GetUserProvidersAsync(id.ToString());
            var userProvidersApiDto = _mapper.Map<UserProvidersApiDto<TKey>>(userProvidersDto);

            return Ok(userProvidersApiDto);
        }

        [HttpDelete("Providers")]
        public async Task<IActionResult> DeleteUserProviders([FromBody]UserProviderDeleteApiDto<TKey> provider)
        {
            var providerDto = _mapper.Map<UserProviderDto<TKey>>(provider);

            await _identityService.GetUserProviderAsync(providerDto.UserId.ToString(), providerDto.ProviderKey);
            await _identityService.DeleteUserProvidersAsync(providerDto);

            return Ok();
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> PostChangePassword([FromBody]UserChangePasswordApiDto<TKey> password)
        {
            var userChangePasswordDto = _mapper.Map<UserChangePasswordDto<TKey>>(password);

            await _identityService.UserChangePasswordAsync(userChangePasswordDto);

            return Ok();
        }

		[HttpGet("{id}/RoleClaims")]
		public async Task<ActionResult<RoleClaimsApiDto<TKey>>> GeIdentityRoleClaim<TKey>s(TKey id, string claimSearchText, int page = 1, int pageSize = 10)
		{
			var roleClaimsDto = await _identityService.GeIdentityUserRoleClaimsAsync(id.ToString(), claimSearchText, page, pageSize);
			var roleClaimsApiDto = _mapper.Map<RoleClaimsApiDto<TKey>>(roleClaimsDto);

			return Ok(roleClaimsApiDto);
		}

        [HttpGet("ClaimType/{claimType}/ClaimValue/{claimValue}")]
        public async Task<ActionResult<TUsersDto>> GetClaimUsers(string claimType, string claimValue, int page = 1, int pageSize = 10)
        {
            var usersDto = await _identityService.GetClaimUsersAsync(claimType, claimValue, page, pageSize);

            return Ok(usersDto);
        }

        [HttpGet("ClaimType/{claimType}")]
        public async Task<ActionResult<TUsersDto>> GetClaimUsers(string claimType, int page = 1, int pageSize = 10)
        {
            var usersDto = await _identityService.GetClaimUsersAsync(claimType, null, page, pageSize);

            return Ok(usersDto);
        }
    }
}





