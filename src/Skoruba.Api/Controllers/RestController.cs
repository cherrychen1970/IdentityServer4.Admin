using System;
using System.Linq;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

using Skoruba.Helpers;
using Bluebird.Repositories;

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
            var result = _repository.GetOne(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetMany(int? page)
        {            
            var result = _repository.GetMany();
            return Ok(result);
        }

        [HttpGet("{id}/{child}")]
        public async Task<IActionResult> GetManyFromNested(TKey id,string child,int? page,string search)
        {
            var item = _repository.GetOne(id,new[] {"id",child});                       
            return Ok(item);
        }

        [HttpPut("{id}/{child}")]
        public async Task<IActionResult> UpdateNested(TKey id,ExpandoObject input)
        {
            var item = _repository.GetOne(id);
            var values = input.ToDictionary(x=>x.Key,y=>y.Value,StringComparer.OrdinalIgnoreCase);
            _repository.Patch(id,values);
                                    
            return Ok(input);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Post(TModel model)
        {
            _repository.Create(model);
            return Ok(model);            
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(TKey id)
        {            
            _repository.Delete(id);
            return Ok();
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(TModel model)
        {
             _repository.Delete(model);
            return Ok(model);            
        }
    }
}