using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

using Skoruba.Helpers;
using Skoruba.Repositories;

namespace Skoruba.Admin.Api.Controllers
{
    public class RestController<TRepository, TModel, TKey> : BaseController
        where TRepository : IRepository<TModel, TKey>
        where TModel : class
        where TKey : IEquatable<TKey>
    {
        protected IRepository<TModel, TKey> _repository { get; }
        
        public RestController(IRepository<TModel, TKey> repository,ILogger<BaseController> logger):base()
        {
            _repository = repository;
        }
   
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(TKey id)
        {            
            var result = await _repository.GetOne(id);
            return Ok(result);
        }


        [HttpGet]
        public async Task<IActionResult> GetMany(int? page)
        {            
            var result = await _repository.GetMany();
            return Ok(result);
        }

        [HttpGet("{id}/{child}")]
        public async Task<IActionResult> GetManyFromNested(TKey id,string child,int? page,string search)
        {
            var item = await _repository.GetOne(id);

            var p = typeof(TModel).GetProp(child);
            var result = p.GetValue(item);
            
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
        public async Task<IActionResult> Delete(TKey id)
        {            
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