using System;
using System.Linq;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

using Skoruba.Helpers;
using Bluebird.Repositories;

namespace Skoruba.Admin.Api.Controllers
{
    public class RestController<TRepository, TModel, TEntity, TKey> : BaseController
        where TRepository : IRepository<TEntity, TKey>
        where TModel : class
        where TEntity : class
        where TKey : IEquatable<TKey>
    {
        protected IRepository<TEntity, TKey> _repository { get; }
        protected IMapper _mapper;
        protected IConfigurationProvider _mapperProvider=>_mapper.ConfigurationProvider;
        
        public RestController(IRepository<TEntity, TKey> repository, IMapper mapper, ILogger<BaseController> logger):base()
        {
            _repository = repository;
            _mapper = mapper;
        }
   
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(TKey id)
        {            
            var result = _repository.GetOne<TModel>(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetMany(int ids)
        {            
            var result = _repository.GetMany<TModel>(null, new Bluebird.Linq.Paging(), new Bluebird.Linq.Sort());
            return Ok(result);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Post(TEntity model)
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
        public async Task<IActionResult> Delete(TEntity model)
        {
             _repository.Delete(model);
            return Ok(model);            
        }
    }
}