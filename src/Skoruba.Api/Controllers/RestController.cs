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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            if (id == 0) return NotFound();
            var result = await _repository.GetOne(id);
            return Ok();
        }


        [HttpGet]
        public async Task<IActionResult> GetMany(int? page)
        {            
            var result = await _repository.GetMany();
            return Ok(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Post(TModel model)
        {
            await _repository.Create(model);
            return Ok(model);            
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0) return NotFound();
            var model = await _repository.Delete(id);
            return Ok(model);
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(TModel model)
        {
            await _repository.Delete(model);
            return Ok(model);            
        }
    }
}