using System;
using System.Threading.Tasks;
using System.Linq;
using System.Reflection;
using System.Dynamic;
using System.Collections.Generic;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.EntityFramework.DbContexts;
using Skoruba.Repositories;
using Skoruba.IdentityServer4.EntityFramework.Repositories;
using Skoruba.IdentityServer4.Models;

namespace Skoruba.Admin.Controllers
{
   
    abstract public class RestController<TRepository, TModel> : BaseController
        where TRepository : IRepository<TModel, int>
        where TModel : class, IPrimaryKey<int>
    {
        protected IRepository<TModel, int> _repository { get; }
       private readonly IStringLocalizer<RestController<TRepository,TModel>> _localizer;

        public RestController(IRepository<TModel, int> repository,ILogger<BaseController> logger):base(logger)
        {
            _repository = repository;
        }

        public void SendSucessNotification(TModel client, string message)
        {
            SuccessNotification(string.Format(_localizer[message]), _localizer["SuccessTitle"]);
        }

        [HttpGet]
        [Route("[controller]/[action]")]
        [Route("[controller]/[action]/{id:int}")]
        public async Task<IActionResult> GetOne(int id)
        {
            if (id == 0)
            {
                //var clientDto = _repository.BuildClientViewModel();
                //return View(clientDto);
                return View();
            }

            var client = await _repository.GetOne(id);
            //client = _clientService.BuildClientViewModel(client);

            return View(client);
        }

        [HttpGet]
        public async Task<IActionResult> Clone(int id)
        {
            if (id == 0) return NotFound();

            var model = await _repository.GetOne(id);
            model.Id=0;
            //var client = _repository.BuildCloneViewModel(id, clientDto);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetMany(int? page, string search)
        {
            var result = await _repository.GetMany();
            return View(result);
        }

        [HttpGet("{child}")]
        public async Task<IActionResult> GetManyFromNested(string child,int? page, int id,string search)
        {
            var item = await _repository.GetOne(id);

            var p = typeof(TModel).GetProperty(child);
            var result = p.GetValue(item);
            
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Post(TModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            //Add new client
            if (model.Id == 0)
            {
                var created = await _repository.Create(model);
                SendSucessNotification(model, "create");
                return RedirectToAction(nameof(GetOne), new { Id = created.Id });
            }

            //Update client
            await _repository.Update(model.Id, model);
            SendSucessNotification(model, "update");

            await _repository.Create(model);
            return RedirectToAction(nameof(GetOne), new { Id = 1 });
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