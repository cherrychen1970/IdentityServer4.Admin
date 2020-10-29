using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

using Skoruba.Repositories;

namespace Skoruba.Admin.Api.Controllers
{
    abstract public class RestController<TRepository, TModel> : BaseController
        where TRepository : IRepository<TModel, int>
        where TModel : class
    {
        protected IRepository<TModel, int> _repository { get; }

        public RestController(IRepository<TModel, int> repository,ILogger<BaseController> logger):base()
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id, int? page)
        {
            if (id == 0) return NotFound();
            var result = await _repository.GetMany(id, page ?? 1);
            return Ok(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Post(TModel model)
        {
            await _repository.Create(model);
            return Ok(model);            
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0) return NotFound();
            var model = await _repository.GetOne(id);
            return Ok(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(TModel model)
        {
            await _repository.Delete(model);
            return Ok(model);            
        }
    }
}