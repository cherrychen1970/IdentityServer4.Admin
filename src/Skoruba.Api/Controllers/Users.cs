using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using IdentityServer4.EntityFramework.Entities;
using Skoruba.IdentityServer4.EntityFramework.Repositories;

using Skoruba.IdentityServer4.Models;

namespace Skoruba.Admin.Api.Controllers
{
    [Route("_api/[controller]")]
    public class UsersController : BaseController
    {
        protected readonly UserManager<IdentityUser<string>> _userManager;
        public UsersController(UserManager<IdentityUser<string>> UserManager, ILogger<BaseController> logger)
        : base()
        {
            _userManager = UserManager;

        }

        [HttpGet]
        public async Task<IActionResult> GetMany(int? page)
        {            
            var result =  _userManager.Users.ToList();
            return Ok(result);
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Post(IdentityUser<string> model)
        {
            model.Id=Guid.NewGuid().ToString();
            await _userManager.CreateAsync(model);
            await _userManager.CreateAsync(model,"secret123.");
            return Ok(model);            
        }


    }
}