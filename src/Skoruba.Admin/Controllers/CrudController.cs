using System;
using System.Threading.Tasks;
using System.Linq;
using System.Reflection;
using System.Dynamic;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.EntityFramework.DbContexts;
using Skoruba.Core.Repositories;
using Skoruba.IdentityServer4.Repositories;
using Skoruba.IdentityServer4.Dtos.Configuration;

namespace Skoruba.Admin.Controllers
{
    public class ClientController: RestController<ClientRepository, ClientDto>
    {
        public ClientController(ClientRepository repository,ILogger<BaseController> logger)
        :base(repository, logger)
        {
        }
    }
    
    abstract public class RestController<TRepository, TModel> : BaseController
        where TRepository : IRepository<TModel, int>
        where TModel : class
    {
        protected IRepository<TModel, int> _repository { get; }

        public RestController(IRepository<TModel, int> repository,ILogger<BaseController> logger):base(logger)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id, int? page)
        {
            if (id == 0) return NotFound();
            var result = await _repository.GetMany(id, page ?? 1);
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Post(TModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _repository.Create(model);
            return RedirectToAction(nameof(Post), new { Id = 1 });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0) return NotFound();
            var model = await _repository.GetOne(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(TModel model)
        {
            await _repository.Delete(model);
            return RedirectToAction(nameof(model));
        }
    }
}