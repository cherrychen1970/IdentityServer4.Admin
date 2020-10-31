using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using IdentityModel;
//using IdentityServer4.Models;
using IdentityServer4.EntityFramework.Entities;
using Skoruba.Repositories;
//using Skoruba.IdentityServer4.Models;
using Skoruba.IdentityServer4.EntityFramework.Repositories;
using Skoruba.IdentityServer4.Services;
using Skoruba.Admin.Configuration.Constants;
using Skoruba.Admin.ExceptionHandling;

namespace Skoruba.Admin.Controllers
{
    [Authorize(Policy = AuthorizationConsts.AdministrationPolicy)]
    [TypeFilter(typeof(ControllerExceptionFilterAttribute))]
    public class IdentityResourceController : BaseController
    {
        private readonly IRepository<IdentityResource, int> _identityResourceService;
        private readonly IStringLocalizer<IdentityResourceController> _localizer;

        public IdentityResourceController(
            IRepository<IdentityResource, int> identityResourceService,
            IStringLocalizer<IdentityResourceController> localizer,
            ILogger<IdentityResourceController> logger)
            : base(logger)
        {
            _identityResourceService = identityResourceService;
            _localizer = localizer;
        }



        [HttpGet]
        public async Task<IActionResult> IdentityResourceProperties(int id, int? page)
        {
            if (id == 0) return NotFound();

            var properties = await _identityResourceService.GetIdentityResourcePropertiesAsync(id, page ?? 1);

            return View(properties);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IdentityResourceProperties(IdentityResourcePropertiesDto identityResourceProperty)
        {
            if (!ModelState.IsValid)
            {
                return View(identityResourceProperty);
            }

            await _identityResourceService.AddIdentityResourcePropertyAsync(identityResourceProperty);
            SuccessNotification(string.Format(_localizer["SuccessAddIdentityResourceProperty"], identityResourceProperty.Value, identityResourceProperty.IdentityResourceName), _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(IdentityResourceProperties), new { Id = identityResourceProperty.IdentityResourceId });
        }

 
 
        [HttpGet]
        public async Task<IActionResult> IdentityResourcePropertyDelete(int id)
        {
            if (id == 0) return NotFound();

            var identityResourceProperty = await _identityResourceService.GetIdentityResourcePropertyAsync(id);

            return View(nameof(IdentityResourcePropertyDelete), identityResourceProperty);
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IdentityResourcePropertyDelete(IdentityResourcePropertiesDto identityResourceProperty)
        {
            await _identityResourceService.DeleteIdentityResourcePropertyAsync(identityResourceProperty);
            SuccessNotification(_localizer["SuccessDeleteIdentityResourceProperty"], _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(IdentityResourceProperties), new { Id = identityResourceProperty.IdentityResourceId });
        }





        [HttpGet]
        public async Task<IActionResult> SearchScopes(string scope, int limit = 0)
        {
            var scopes = await _clientService.GetScopesAsync(scope, limit);

            return Ok(scopes);
        }

        [HttpGet]
        public IActionResult SearchClaims(string claim, int limit = 0)
        {
            var claims = _clientService.GetStandardClaims(claim, limit);

            return Ok(claims);
        }

        [HttpGet]
        public IActionResult SearchGrantTypes(string grant, int limit = 0)
        {
            var grants = _clientService.GetGrantTypes(grant, limit);

            return Ok(grants);
        }

        [HttpGet]
        public async Task<IActionResult> IdentityResourceDelete(int id)
        {
            if (id == 0) return NotFound();

            var identityResource = await _identityResourceService.GetIdentityResourceAsync(id);

            return View(identityResource);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IdentityResourceDelete(IdentityResourceDto identityResource)
        {
            await _identityResourceService.DeleteIdentityResourceAsync(identityResource);
            SuccessNotification(_localizer["SuccessDeleteIdentityResource"], _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(IdentityResources));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IdentityResource(IdentityResourceDto identityResource)
        {
            if (!ModelState.IsValid)
            {
                return View(identityResource);
            }

            identityResource = _identityResourceService.BuildIdentityResourceViewModel(identityResource);

            int identityResourceId;

            if (identityResource.Id == 0)
            {
                identityResourceId = await _identityResourceService.AddIdentityResourceAsync(identityResource);
            }
            else
            {
                identityResourceId = identityResource.Id;
                await _identityResourceService.UpdateIdentityResourceAsync(identityResource);
            }

            SuccessNotification(string.Format(_localizer["SuccessAddIdentityResource"], identityResource.Name), _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(IdentityResource), new { Id = identityResourceId });
        }



        [HttpGet]
        public async Task<IActionResult> IdentityResources(int? page, string search)
        {
            ViewBag.Search = search;
            var identityResourcesDto = await _identityResourceService.GetIdentityResourcesAsync(search, page ?? 1);

            return View(identityResourcesDto);
        }



        [HttpGet]
        [Route("[controller]/[action]")]
        [Route("[controller]/[action]/{id:int}")]
        public async Task<IActionResult> IdentityResource(int id)
        {
            if (id == 0)
            {
                var identityResourceDto = new IdentityResourceDto();
                return View(identityResourceDto);
            }

            var identityResource = await _identityResourceService.GetIdentityResourceAsync(id);

            return View(identityResource);
        }
    }
}