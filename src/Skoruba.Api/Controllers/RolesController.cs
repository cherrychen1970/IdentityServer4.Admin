using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Skoruba.Admin.Api.Configuration.Constants;
using Skoruba.Admin.Api.Dtos.Roles;
using Skoruba.Admin.Api.ExceptionHandling;
using Skoruba.Admin.Api.Helpers.Localization;
using Skoruba.Admin.Api.Resources;
using Skoruba.Identity.Dtos.Identity;
using Skoruba.Identity.Services.Interfaces;

namespace Skoruba.Admin.Api.Controllers
{
    public class RolesController<TUserDto, RoleDto<TKey>, TUser, IdentityRole<TKey>, TKey, IdentityUserClaim<TKey>, IdentityUserRole<TKey>, IdentityUserLogin<TKey>, IdentityRoleClaim<TKey>, IdentityUserToken<TKey>,
            TUsersDto, IdentityRolesDto<TKey>, IdentityUserRole<TKey>sDto, IdentityUserClaim<TKey>sDto,
            UserProviderDto<TKey>, TUserProvidersDto, UserChangePasswordDto<TKey>, IdentityRoleClaim<TKey>sDto, IdentityUserClaim<TKey>Dto, IdentityRoleClaim<TKey>Dto> : BaseController
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
        where IdentityRoleClaim<TKey>sDto : RoleClaimsDto<IdentityRoleClaim<TKey>Dto, TKey>, new()
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

        public RolesController(IIdentityService<TUserDto, RoleDto<TKey>, TUser, IdentityRole<TKey>, TKey, IdentityUserClaim<TKey>, IdentityUserRole<TKey>, IdentityUserLogin<TKey>, IdentityRoleClaim<TKey>, IdentityUserToken<TKey>,
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
        public async Task<ActionResult<RoleDto<TKey>>> Get(TKey id)
        {
            var role = await _identityService.GeIdentityRoleAsync(id.ToString());

            return Ok(role);
        }

        [HttpGet]
        public async Task<ActionResult<IdentityRolesDto<TKey>>> Get(string searchText, int page = 1, int pageSize = 10)
        {
            var rolesDto = await _identityService.GeIdentityRolesAsync(searchText, page, pageSize);

            return Ok(rolesDto);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<RoleDto<TKey>>> Post([FromBody]RoleDto<TKey> role)
        {
            if (!EqualityComparer<TKey>.Default.Equals(role.Id, default))
            {
                return BadRequest(_errorResources.CannotSetId());
            }
 
            var (identityResult, roleId) = await _identityService.CreateRoleAsync(role);
            var createdRole = await _identityService.GeIdentityRoleAsync(roleId.ToString());

            return CreatedAtAction(nameof(Get), new { id = createdRole.Id }, createdRole);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]RoleDto<TKey> role)
        {
            await _identityService.GeIdentityRoleAsync(role.Id.ToString());
            await _identityService.UpdateRoleAsync(role);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(TKey id)
        {
            var roleDto = new RoleDto<TKey> { Id = id };

            await _identityService.GeIdentityRoleAsync(id.ToString());
            await _identityService.DeleteRoleAsync(roleDto);

            return Ok();
        }

        [HttpGet("{id}/Users")]
        public async Task<ActionResult<IdentityRolesDto<TKey>>> GeIdentityRole<TKey>Users(string id, string searchText, int page = 1, int pageSize = 10)
        {
            var usersDto = await _identityService.GeIdentityRoleUsersAsync(id, searchText, page, pageSize);

            return Ok(usersDto);
        }

        [HttpGet("{id}/Claims")]
        public async Task<ActionResult<RoleClaimsApiDto<TKey>>> GeIdentityRoleClaim<TKey>s(string id, int page = 1, int pageSize = 10)
        {
            var roleClaimsDto = await _identityService.GeIdentityRoleClaimsAsync(id, page, pageSize);
            var roleClaimsApiDto = _mapper.Map<RoleClaimsApiDto<TKey>>(roleClaimsDto);

            return Ok(roleClaimsApiDto);
        }

        [HttpPost("Claims")]
        public async Task<IActionResult> PosIdentityRoleClaim<TKey>s([FromBody]RoleClaimApiDto<TKey> roleClaims)
        {
            var roleClaimsDto = _mapper.Map<IdentityRoleClaim<TKey>sDto>(roleClaims);

            if (!roleClaimsDto.ClaimId.Equals(default))
            {
                return BadRequest(_errorResources.CannotSetId());
            }

            await _identityService.CreateRoleClaimsAsync(roleClaimsDto);

            return Ok();
        }

        [HttpPut("Claims")]
        public async Task<IActionResult> PuIdentityRoleClaim<TKey>s([FromBody]RoleClaimApiDto<TKey> roleClaims)
        {
            var roleClaimsDto = _mapper.Map<IdentityRoleClaim<TKey>sDto>(roleClaims);

            if (!roleClaimsDto.ClaimId.Equals(default))
            {
                return BadRequest(_errorResources.CannotSetId());
            }

            await _identityService.UpdateRoleClaimsAsync(roleClaimsDto);

            return Ok();
        }

        [HttpDelete("{id}/Claims")]
        public async Task<IActionResult> DeleteRoleClaims(TKey id, int claimId)
        {
            var roleDto = new IdentityRoleClaim<TKey>sDto { ClaimId = claimId, RoleId = id };

            await _identityService.GeIdentityRoleClaimAsync(roleDto.RoleId.ToString(), roleDto.ClaimId);
            await _identityService.DeleteRoleClaimAsync(roleDto);

            return Ok();
        }
    }
}