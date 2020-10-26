using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.EntityFramework.DbContexts;
using Skoruba.Admin.Api.ExceptionHandling;
using Skoruba.Admin.Api.Configuration.Constants;

namespace Skoruba.Admin.Api.Controllers
{
    // TEST CHERRY : disabled authentication
    //[AllowAnonymous]
    //[Authorize(Roles="superuser")]    
    //[Authorize(Policy=PolicyConstants.LocalApi)]    
    //[DevOnlyRouteConstraint]
    //[Route("_api/[controller]")]
    [Route("api/[controller]")]    
    [TypeFilter(typeof(ControllerExceptionFilterAttribute))]
    [Produces("application/json", "application/problem+json")]
    [Authorize(Policy = AuthorizationConsts.AdministrationPolicy)]    
    [ApiController]
    abstract public partial class BaseController : ControllerBase
    {

    }
}