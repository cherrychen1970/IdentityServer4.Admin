using System;
using System.Linq;
using System.Reflection;
using System.Dynamic;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.EntityFramework.DbContexts;

using Bluebird.Linq;

namespace id4.Api.TestControllers
{
    public interface IMapper<TEntity,TModel>{
        TEntity ToEntity(TModel model);        
        TModel ToModel(TEntity entity);
    }
        
    //[Authorize(Policy=PolicyConstants.LocalApi)]    
    [DevOnlyRouteConstraint]
    [Route("_api/[controller]")]
    [ApiController]
    abstract public partial class TestCrudController<TEntity,TModel,TKey> : ControllerBase 
        where TEntity : class
        where TModel : class        
    {
        protected ConfigurationDbContext _context {get;}
        IMapper<TEntity,TModel> _mapper;

        public TestCrudController(ConfigurationDbContext context,IMapper<TEntity,TModel> mapper) 
        {
            _context = context;
            _mapper = mapper;
        }

        virtual protected TEntity Find(TKey id) 
             => _context.Set<TEntity>().Find(id);
            
        virtual protected IQueryable<TEntity> OnSelect(DbSet<TEntity> set) {
            return set;
        }

        [HttpGet]
        virtual public IActionResult Get()    {
            var set = _context.Set<TEntity>();
            var query = OnSelect(set);
            var entities = query.ToList();
            var list = entities.Select(x=>_mapper.ToModel(x));
            return Ok(list);
        }    

        [HttpGet("{id}")]
        virtual public IActionResult Get(TKey id)    {
            var set = _context.Set<TEntity>();            
            var item = Find(id);
            var model = _mapper.ToModel(item);
            return Ok(model);
        } 
          
        [HttpGet("{id}/{nested}")]
        virtual public IActionResult GetManyNested(TKey id,string nested,int? page,string search)
        {
            var list = _context.Set<TEntity>().Select<TEntity>(new[] {"id",nested});
                      
            return Ok(list);
        }

        [HttpPost]
        virtual public IActionResult Post(TModel model)    {
            var entity = _mapper.ToEntity(model);
            var set = _context.Set<TEntity>();
            set.Add(entity);           
            _context.SaveChanges();
            return Ok(model);
        } 

        [HttpPut("{id}")]
        virtual public IActionResult Put(TKey id, TModel model)    {
            var entity = _mapper.ToEntity(model);            
            var item = Find(id);
            _context.Entry(item).CurrentValues.SetValues(entity);
            _context.SaveChanges();
            return Ok(model);
        } 

        [HttpPatch("{id}")]
        virtual public IActionResult Patch(TKey id, ExpandoObject model)    {                                    
            var item = Find(id);
            IDictionary<string, object> values = model;

            // fix case sensitivity problem
            values = values.Select(x=> new {Name= GetProp<TEntity>(x.Key), Value=x.Value}).ToDictionary(x=>x.Name,y=>y.Value);
            _context.Entry(item).CurrentValues.SetValues(values);
            _context.SaveChanges();
            return Ok(model);
        } 

        [HttpDelete("{id}")]
        virtual public IActionResult Delete(TKey id)    {
            var item = Find(id);
            var model = _mapper.ToModel(item);
            _context.Set<TEntity>().Remove(item);
            _context.SaveChanges();
            return Ok(model);
        }   


        /////
        public static string GetProp<T>(string key)
        {
            var itemType = typeof(T);
            return itemType.GetProperty(key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance).Name;
        } 
    }
}