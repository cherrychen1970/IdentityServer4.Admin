using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
//using Skoruba.AuditLogging.Services;
using Skoruba.Models;
using AutoMapper;

namespace Skoruba.Repositories
{
    abstract public class Repository<TDbContext,TEntity,TModel,TKey> 
        where TDbContext : DbContext
        where TEntity : class
        where TModel : class
    {
        //protected TDbContext DbContext { get; }
        protected IMapper _mapper { get; }

        //protected readonly IAuditEventLogger AuditEventLogger;
        public bool AutoSaveChanges { get; set; } = true;
        public readonly ILogger _logger;
        
        EntityRepository<TDbContext,TEntity,TKey> _entityRepository;

        public Repository(TDbContext context, IMapper mapper, ILogger logger 
        //, IAuditEventLogger auditEventLogger
        ) 
        {
            //DbContext = context;
            _mapper = mapper;
            _logger = logger;
            _entityRepository = new EntityRepository<TDbContext,TEntity,TKey>(context,logger);
            
            _logger.LogError(_mapper.ConfigurationProvider.Binders.Count().ToString());
            //AuditEventLogger = auditEventLogger;
        }

        virtual async protected Task<TEntity> FindAsync(TKey id)
             => await _entityRepository.FindAsync(id);//.Map<TModel>(_mapper);

        virtual protected IQueryable<TEntity> OnSelect(DbSet<TEntity> set)
        {
            return set;
        }

        virtual public async Task<IPagedList<TModel>> GetMany(int skip = 0, int take = 10, string orderby = null, bool asc = true)
        {
            var entities = await _entityRepository.GetMany(skip,take,orderby,asc);
            return entities.Select(x => _mapper.Map<TModel>(x)).ToPagedList(entities.PageSize,entities.TotalCount);
            //await AuditEventLogger.LogEventAsync(new CommonEvent());
        }
        virtual public async Task<IPagedList<TModel>> GetMany(IDictionary<string, object> filter, int skip = 0, int take = 10, string orderby = null, bool asc = true)
        {
            var entities = await _entityRepository.GetMany(filter,skip,take,orderby,asc);
            var list = entities.Select(x => _mapper.Map<TModel>(x));
            return list.ToPagedList(10, entities.TotalCount);
        }
        virtual public async Task<TModel> GetOne(TKey id)
        {            
            var item = await _entityRepository.GetOne(id);
            var model = _mapper.Map<TModel>(item);
            return model;
        }

        virtual public async Task<TModel> Create(TModel model)
        {
            var entity = _mapper.Map<TEntity>(model);
            await _entityRepository.Create(entity);
            return model;
        }

        virtual public async Task<TModel> Update(TKey id, TModel model)
        {
            var entity = _mapper.Map<TEntity>(model);
            await _entityRepository.Update(id,entity);
            return model;
        }

        virtual public async Task<TModel> Patch(TKey id, IDictionary<string, object> model)
        {
            var item = await FindAsync(id);
            // fix case sensitivity problem
            var values = model.Select(x => new { Name = GetProp<TEntity>(x.Key), Value = x.Value }).ToDictionary(x => x.Name, y => y.Value);
            await _entityRepository.Patch(id,values);
            return _mapper.Map<TModel>(item);
        }

        virtual public async Task<TModel> Delete(TKey id)
        {
            var item =await _entityRepository.Delete(id);
            var model = _mapper.Map<TModel>(item);
            return model;
        }

        virtual public async Task<TModel> Delete(TModel model)
        {
            var item = _mapper.Map<TEntity>(model);
            await _entityRepository.Delete(item);
            return model;
        }
        public virtual async Task<int> SaveChangesAsync()
        {
            return await _entityRepository.SaveChangesAsync();
        }

        /////
        public static string GetProp<T>(string key)
        {
            var itemType = typeof(T);
            return itemType.GetProperty(key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance).Name;
        }
    }
}