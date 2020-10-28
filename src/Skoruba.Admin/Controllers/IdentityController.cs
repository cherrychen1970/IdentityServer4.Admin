using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Skoruba.AspNetIdentity.Dtos;
using Skoruba.AspNetIdentity.Services.Interfaces;
using Skoruba.Core.Dtos.Common;
using Skoruba.Admin.Configuration.Constants;
using Skoruba.Admin.ExceptionHandling;
using Skoruba.Admin.Helpers.Localization;
using Skoruba.Admin.ViewModels;

namespace Skoruba.Admin.Controllers
{
    [Authorize(Policy = AuthorizationConsts.AdministrationPolicy)]
    [TypeFilter(typeof(ControllerExceptionFilterAttribute))]
    public class IdentityController<TKey> : BaseController where TKey : IEquatable<TKey>
    {

        private readonly IIdentityService<TKey> _identityService;
        private readonly IGenericControllerLocalizer<IdentityController<TKey>> _localizer;

        public IdentityController(IIdentityService<TKey> identityService,
            ILogger<ConfigurationController> logger,
            IGenericControllerLocalizer<IdentityController<TKey>> localizer) : base(logger)
        {
            _identityService = identityService;
            _localizer = localizer;
        }

        [HttpGet]
        public async Task<IActionResult> Roles(int? page, string search)
        {
            ViewBag.Search = search;
            var roles = await _identityService.GetRolesAsync(search, page ?? 1);

            return View(roles);
        }

        [HttpGet]
        [Route("[controller]/[action]")]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> Role(TKey id)
        {
            if (EqualityComparer<TKey>.Default.Equals(id, default))
            {
                return View(new RoleDto<TKey>());
            }

            var role = await _identityService.GetRoleAsync(id.ToString());

            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Role(RoleDto<TKey> role)
        {
            if (!ModelState.IsValid)
            {
                return View(role);
            }

            TKey roleId;

            if (EqualityComparer<TKey>.Default.Equals(role.Id, default))
            {
                var roleData = await _identityService.CreateRoleAsync(role);
                roleId = roleData.roleId;
            }
            else
            {
                var roleData = await _identityService.UpdateRoleAsync(role);
                roleId = roleData.roleId;
            }

            SuccessNotification(string.Format(_localizer["SuccessCreateRole"], role.Name), _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(Role), new { Id = roleId });
        }

        [HttpGet]
        public async Task<IActionResult> Users(int? page, string search)
        {
            ViewBag.Search = search;
            var usersDto = await _identityService.GetUsersAsync(search, page ?? 1);

            return View(usersDto);
        }

        [HttpGet]
        public async Task<IActionResult> RoleUsers(string id, int? page, string search)
        {
            ViewBag.Search = search;
            var roleUsers = await _identityService.GetRoleUsersAsync(id, search, page ?? 1);

            var roleDto = await _identityService.GetRoleAsync(id);
            ViewData["RoleName"] = roleDto.Name;

            return View(roleUsers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserProfile(UserDto<TKey> user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            TKey userId;

            if (EqualityComparer<TKey>.Default.Equals(user.Id, default))
            {
                var userData = await _identityService.CreateUserAsync(user);
                userId = userData.userId;
            }
            else
            {
                var userData = await _identityService.UpdateUserAsync(user);
                userId = userData.userId;
            }

            SuccessNotification(string.Format(_localizer["SuccessCreateUser"], user.UserName), _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(UserProfile), new { Id = userId });
        }

        [HttpGet]
        public IActionResult UserProfile()
        {
            var newUser = new UserDto<TKey>();

            return View("UserProfile", newUser);
        }

        [HttpGet]
        [Route("[controller]/UserProfile/{id}")]
        public async Task<IActionResult> UserProfile(TKey id)
        {
            var user = await _identityService.GetUserAsync(id.ToString());
            if (user == null) return NotFound();

            return View("UserProfile", user);
        }

        [HttpGet]
        public async Task<IActionResult> UserRoles(TKey id, int page = 1)
        {
            if (EqualityComparer<TKey>.Default.Equals(id, default)) return NotFound();

            var userRoles = await _identityService.GetUserRolesAsync(id.ToString(), page);
            var roles = (await _identityService.GetRolesAsync())
                        .Select(x => new { Id = x.Id.ToString(), x.Name });

            var resp = new { Item = id, List = roles };



            return View(userRoles);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserRoles(UserRoleDto<TKey> role)
        {
            await _identityService.CreateUserRoleAsync(role);
            SuccessNotification(_localizer["SuccessCreateUserRole"], _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(UserRoles), new { Id = role.UserId });
        }

        [HttpGet]
        public async Task<IActionResult> UserRolesDelete(TKey id, TKey roleId)
        {
            var userDto = await _identityService.GetUserAsync(id.ToString());
            var roles = await _identityService.GetRolesAsync();

            var rolesDto = new
            {
                UserId = id,
                RolesList = roles.Select(x => new { x.Id, x.Name }).ToList(),
                RoleId = roleId,
                UserName = userDto.UserName
            };

            return View(rolesDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserRolesDelete(UserRoleDto<TKey> role)
        {
            await _identityService.DeleteUserRoleAsync(role);
            SuccessNotification(_localizer["SuccessDeleteUserRole"], _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(UserRoles), new { Id = role.UserId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserClaims(IModelDto<UserDto<TKey>, List<UserClaimDto<TKey>>> request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            await _identityService.CreateUserClaimsAsync(request.Body);
            SuccessNotification(string.Format(_localizer["SuccessCreateUserClaims"]), _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(UserClaims), new { Id = request.Subject.Id });
        }

        [HttpGet]
        public async Task<IActionResult> UserClaims(TKey id, int? page)
        {
            if (EqualityComparer<TKey>.Default.Equals(id, default)) return NotFound();

            var claims = await _identityService.GetUserClaimsAsync(id.ToString(), page ?? 1);
            var resp = new { Item = id, Data = claims } as IModelDto<TKey, PagedList<UserClaimDto<TKey>>>;

            return View(claims);
        }

        [HttpGet]
        public async Task<IActionResult> UserClaimsDelete(TKey id, int claimId)
        {
            if (EqualityComparer<TKey>.Default.Equals(id, default)
            || EqualityComparer<int>.Default.Equals(claimId, default)) return NotFound();

            var claim = await _identityService.GetUserClaimAsync(id.ToString(), claimId);
            if (claim == null) return NotFound();

            var userDto = await _identityService.GetUserAsync(id.ToString());
            //claim.UserName = userDto.UserName;

            return View(claim);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserClaimsDelete(IModelDto<TKey, UserClaimDto<TKey>> request)
        {
            await _identityService.DeleteUserClaimAsync(request.Body);
            SuccessNotification(_localizer["SuccessDeleteUserClaims"], _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(UserClaims), new { Id = request.Subject });
        }

        [HttpGet]
        public async Task<IActionResult> UserProviders(TKey id)
        {
            if (EqualityComparer<TKey>.Default.Equals(id, default)) return NotFound();

            var providers = await _identityService.GetUserProvidersAsync(id.ToString());

            return View(providers);
        }

        [HttpGet]
        public async Task<IActionResult> UserProvidersDelete(TKey id, string providerKey)
        {
            if (EqualityComparer<TKey>.Default.Equals(id, default) || string.IsNullOrEmpty(providerKey)) return NotFound();

            var provider = await _identityService.GetUserProviderAsync(id.ToString(), providerKey);
            if (provider == null) return NotFound();

            return View(provider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserProvidersDelete(UserProviderDto<TKey> provider)
        {
            await _identityService.DeleteUserProvidersAsync(provider);
            SuccessNotification(_localizer["SuccessDeleteUserProviders"], _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(UserProviders), new { Id = provider.UserId });
        }

        [HttpGet]
        public async Task<IActionResult> UserChangePassword(TKey id)
        {
            if (EqualityComparer<TKey>.Default.Equals(id, default)) return NotFound();

            var user = await _identityService.GetUserAsync(id.ToString());
            var userDto = new UserChangePasswordDto<TKey> { UserId = id, UserName = user.UserName };

            return View(userDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserChangePassword(UserChangePasswordDto<TKey> userPassword)
        {
            if (!ModelState.IsValid)
            {
                return View(userPassword);
            }

            var identityResult = await _identityService.UserChangePasswordAsync(userPassword);

            if (!identityResult.Errors.Any())
            {
                SuccessNotification(_localizer["SuccessUserChangePassword"], _localizer["SuccessTitle"]);

                return RedirectToAction("UserProfile", new { Id = userPassword.UserId });
            }

            foreach (var error in identityResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(userPassword);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RoleClaims(IModelDto<TKey, List<RoleClaimDto<TKey>>> request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            await _identityService.CreateRoleClaimsAsync(request.Body);
            SuccessNotification(string.Format(_localizer["SuccessCreateRoleClaims"]), _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(RoleClaims), new { Id = request.Subject });
        }

        [HttpGet]
        public async Task<IActionResult> RoleClaims(TKey id, int? page)
        {
            if (EqualityComparer<TKey>.Default.Equals(id, default)) return NotFound();

            var claims = await _identityService.GetRoleClaimsAsync(id.ToString(), page ?? 1);            
            var resp = new {Subject=id, Body=claims};
            return View(resp);
        }

        [HttpGet]
        public async Task<IActionResult> RoleClaimsDelete(TKey id, int claimId)
        {
            if (EqualityComparer<TKey>.Default.Equals(id, default) ||
                EqualityComparer<int>.Default.Equals(claimId, default)) return NotFound();

            var claim = await _identityService.GetRoleClaimAsync(id.ToString(), claimId);

            return View(claim);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RoleClaimsDelete(IModelDto<TKey, RoleClaimDto<TKey>> request)
        {
            await _identityService.DeleteRoleClaimAsync(request.Body);
            SuccessNotification(_localizer["SuccessDeleteRoleClaims"], _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(RoleClaims), new { Id = request.Subject });
        }

        [HttpGet]
        public async Task<IActionResult> RoleDelete(TKey id)
        {
            if (EqualityComparer<TKey>.Default.Equals(id, default)) return NotFound();

            var roleDto = await _identityService.GetRoleAsync(id.ToString());
            if (roleDto == null) return NotFound();

            return View(roleDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RoleDelete(RoleDto<TKey> role)
        {
            await _identityService.DeleteRoleAsync(role);
            SuccessNotification(_localizer["SuccessDeleteRole"], _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(Roles));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserDelete(UserDto<TKey> user)
        {
            var currentUserId = User.GetSubjectId();
            if (user.Id.ToString() == currentUserId)
            {
                CreateNotification(Helpers.NotificationHelpers.AlertType.Warning, _localizer["ErrorDeleteUser_CannotSelfDelete"]);
                return RedirectToAction(nameof(UserDelete), user.Id);
            }
            else
            {
                await _identityService.DeleteUserAsync(user.Id.ToString(), user);
                SuccessNotification(_localizer["SuccessDeleteUser"], _localizer["SuccessTitle"]);

                return RedirectToAction(nameof(Users));
            }
        }

        [HttpGet]
        public async Task<IActionResult> UserDelete(TKey id)
        {
            if (EqualityComparer<TKey>.Default.Equals(id, default)) return NotFound();

            var user = await _identityService.GetUserAsync(id.ToString());
            if (user == null) return NotFound();

            return View(user);
        }
    }
}