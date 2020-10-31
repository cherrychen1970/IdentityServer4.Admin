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
    public class ApiResourceController : BaseController
    {
        private readonly IRepository<ApiResource, int> _apiResourceService;
        private readonly IStringLocalizer<ApiResourceController> _localizer;

        public ApiResourceController(
            IRepository<ApiResource, int> apiResourceService,
            IStringLocalizer<ApiResourceController> localizer,
            ILogger<ApiResourceController> logger)
            : base(logger)
        {
            _apiResourceService = apiResourceService;
            _localizer = localizer;
        }

        [HttpGet]
        public async Task<IActionResult> ApiResourceProperties(int id, int? page)
        {
            if (id == 0) return NotFound();

            var api = await _apiResourceService.GetOne(id);
            var properties = api.Properties;

            return View(properties);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApiResourceProperties(ApiResourcePropertiesDto apiResourceProperty)
        {
            if (!ModelState.IsValid)
            {
                return View(apiResourceProperty);
            }

            await _apiResourceService.AddApiResourcePropertyAsync(apiResourceProperty);
            SuccessNotification(string.Format(_localizer["SuccessAddApiResourceProperty"], apiResourceProperty.Key, apiResourceProperty.ApiResourceName), _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(ApiResourceProperties), new { Id = apiResourceProperty.ApiResourceId });
        }

       [HttpGet]
        public async Task<IActionResult> ApiResourcePropertyDelete(int id)
        {
            if (id == 0) return NotFound();

            var apiResourceProperty = await _apiResourceService.GetApiResourcePropertyAsync(id);

            return View(nameof(ApiResourcePropertyDelete), apiResourceProperty);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApiResourcePropertyDelete(ApiResourcePropertiesDto apiResourceProperty)
        {
            await _apiResourceService.DeleteApiResourcePropertyAsync(apiResourceProperty);
            SuccessNotification(_localizer["SuccessDeleteApiResourceProperty"], _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(ApiResourceProperties), new { Id = apiResourceProperty.ApiResourceId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApiResource(ApiResourceDto apiResource)
        {
            if (!ModelState.IsValid)
            {
                return View(apiResource);
            }

            ComboBoxHelpers.PopulateValuesToList(apiResource.UserClaimsItems, apiResource.UserClaims);

            int apiResourceId;

            if (apiResource.Id == 0)
            {
                apiResourceId = await _apiResourceService.AddApiResourceAsync(apiResource);
            }
            else
            {
                apiResourceId = apiResource.Id;
                await _apiResourceService.UpdateApiResourceAsync(apiResource);
            }

            SuccessNotification(string.Format(_localizer["SuccessAddApiResource"], apiResource.Name), _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(ApiResource), new { Id = apiResourceId });
        }

        [HttpGet]
        public async Task<IActionResult> ApiResourceDelete(int id)
        {
            if (id == 0) return NotFound();

            var apiResource = await _apiResourceService.GetApiResourceAsync(id);

            return View(apiResource);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApiResourceDelete(ApiResourceDto apiResource)
        {
            await _apiResourceService.DeleteApiResourceAsync(apiResource);
            SuccessNotification(_localizer["SuccessDeleteApiResource"], _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(ApiResources));
        }

        [HttpGet]
        [Route("[controller]/[action]")]
        [Route("[controller]/[action]/{id:int}")]
        public async Task<IActionResult> ApiResource(int id)
        {
            if (id == 0)
            {
                var apiResourceDto = new ApiResourceDto();
                return View(apiResourceDto);
            }

            var apiResource = await _apiResourceService.GetApiResourceAsync(id);

            return View(apiResource);
        }

        [HttpGet]
        public async Task<IActionResult> ApiSecrets(int id, int? page)
        {
            if (id == 0) return NotFound();

            var apiSecrets = await _apiResourceService.GetApiSecretsAsync(id, page ?? 1);
            _apiResourceService.BuildApiSecretsViewModel(apiSecrets);

            return View(apiSecrets);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApiSecrets(ApiSecretsDto apiSecret)
        {
            if (!ModelState.IsValid)
            {
                return View(apiSecret);
            }

            await _apiResourceService.AddApiSecretAsync(apiSecret);
            SuccessNotification(_localizer["SuccessAddApiSecret"], _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(ApiSecrets), new { Id = apiSecret.ApiResourceId });
        }

        [HttpGet]
        public async Task<IActionResult> ApiScopes(int id, int? page, int? scope)
        {
            if (id == 0 || !ModelState.IsValid) return NotFound();

            if (scope == null)
            {
                var apiScopesDto = await _apiResourceService.GetApiScopesAsync(id, page ?? 1);

                return View(apiScopesDto);
            }
            else
            {
                var apiScopesDto = await _apiResourceService.GetApiScopeAsync(id, scope.Value);
                return View(apiScopesDto);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApiScopes(ApiScopesDto apiScope)
        {
            if (!ModelState.IsValid)
            {
                return View(apiScope);
            }

            _apiResourceService.BuildApiScopeViewModel(apiScope);

            int apiScopeId;

            if (apiScope.ApiScopeId == 0)
            {
                apiScopeId = await _apiResourceService.AddApiScopeAsync(apiScope);
            }
            else
            {
                apiScopeId = apiScope.ApiScopeId;
                await _apiResourceService.UpdateApiScopeAsync(apiScope);
            }

            SuccessNotification(string.Format(_localizer["SuccessAddApiScope"], apiScope.Name), _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(ApiScopes), new { Id = apiScope.ApiResourceId, Scope = apiScopeId });
        }

        [HttpGet]
        public async Task<IActionResult> ApiScopeDelete(int id, int scope)
        {
            if (id == 0 || scope == 0) return NotFound();

            var apiScope = await _apiResourceService.GetApiScopeAsync(id, scope);

            return View(nameof(ApiScopeDelete), apiScope);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApiScopeDelete(ApiScopesDto apiScope)
        {
            await _apiResourceService.DeleteApiScopeAsync(apiScope);
            SuccessNotification(_localizer["SuccessDeleteApiScope"], _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(ApiScopes), new { Id = apiScope.ApiResourceId });
        }

        [HttpGet]
        public async Task<IActionResult> ApiResources(int? page, string search)
        {
            ViewBag.Search = search;
            var apiResources = await _apiResourceService.GetApiResourcesAsync(search, page ?? 1);

            return View(apiResources);
        }

        [HttpGet]
        public async Task<IActionResult> ApiSecretDelete(int id)
        {
            if (id == 0) return NotFound();

            var clientSecret = await _apiResourceService.GetApiSecretAsync(id);

            return View(nameof(ApiSecretDelete), clientSecret);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApiSecretDelete(ApiSecretsDto apiSecret)
        {
            await _apiResourceService.DeleteApiSecretAsync(apiSecret);
            SuccessNotification(_localizer["SuccessDeleteApiSecret"], _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(ApiSecrets), new { Id = apiSecret.ApiResourceId });
        }
    }
}