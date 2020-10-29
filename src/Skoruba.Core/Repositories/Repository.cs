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
    abstract public class Repository<TDbContext,TModel,TEntity,TKey> : EntityRepository<TDbContext,TModel,TKey>
        where TDbContext : DbContext
        where TEntity : class
        where TModel : class
    {
        protected TDbContext DbContext { get; }
        protected IMapper _mapper { get; }

        //protected readonly IAuditEventLogger AuditEventLogger;
        public bool AutoSaveChanges { get; set; } = true;
        public readonly ILogger _logger;


        public Repository(TDbContext context, IMapper mapper, ILogger logger 
        //, IAuditEventLogger auditEventLogger
        ) : base(context,logger)
        {
            DbContext = context;
            _mapper = mapper;
            _logger = logger;
            
            _logger.LogError(_mapper.ConfigurationProvider.Binders.Count().ToString());
            //AuditEventLogger = auditEventLogger;
        }

        virtual async protected Task<TEntity> FindAsync(TKey id)
             => await DbContext.Set<TEntity>().FindAsync(id);//.Map<TModel>(_mapper);

        virtual protected IQueryable<TEntity> OnSelect(DbSet<TEntity> set)
        {
            return set;
        }

        virtual public async Task<IPagedList<TModel>> GetMany(int skip = 0, int take = 10, string orderby = null, bool asc = true)
        {
            var set = DbContext.Set<TEntity>();
            var query = OnSelect(set);
            var total = await query.CountAsync();
            var entities = query.ToList();
            var list =  entities.Select(x => _mapper.Map<TModel>(x));
            //await AuditEventLogger.LogEventAsync(new CommonEvent());
            return list.ToPagedList(10, total);
        }
        virtual public async Task<IPagedList<TModel>> GetMany(IDictionary<string, object> filter, int skip = 0, int take = 10, string orderby = null, bool asc = true)
        {
            var set = DbContext.Set<TEntity>();
            var query = OnSelect(set);
            var entities = query.ToList();
            var total = await query.CountAsync();
            var list = entities.Select(x => _mapper.Map<TModel>(x));
            return list.ToPagedList(10, total);
        }
        virtual public async Task<TModel> GetOne(TKey id)
        {            
            var set = DbContext.Set<TEntity>().AsNoTracking();
            var item = await FindAsync(id);
            var model = _mapper.Map<TModel>(item);
            return model;
        }

        virtual public async Task<TModel> Create(TModel model)
        {
            var entity = _mapper.Map<TEntity>(model);
            var set = DbContext.Set<TEntity>();
            await set.AddAsync(entity);
            if (AutoSaveChanges)
                DbContext.SaveChanges();
            return model;
        }

        virtual public async Task<TModel> Update(TKey id, TModel model)
        {
            var entity = _mapper.Map<TEntity>(model);
            var item = await FindAsync(id);
            DbContext.Entry(item).CurrentValues.SetValues(entity);
            if (AutoSaveChanges)
                DbContext.SaveChanges();
            return model;
        }

        virtual public async Task<TModel> Patch(TKey id, IDictionary<string, object> model)
        {
            var item = await FindAsync(id);
            // fix case sensitivity problem
            var values = model.Select(x => new { Name = GetProp<TEntity>(x.Key), Value = x.Value }).ToDictionary(x => x.Name, y => y.Value);
            DbContext.Entry(item).CurrentValues.SetValues(values);
            if (AutoSaveChanges)
                DbContext.SaveChanges();
            return _mapper.Map<TModel>(item);
        }

        virtual public async Task<TModel> Delete(TKey id)
        {
            var item = await FindAsync(id);
            var model = _mapper.Map<TModel>(item);
            DbContext.Set<TEntity>().Remove(item);
            if (AutoSaveChanges)
                DbContext.SaveChanges();
            return model;
        }

        virtual public async Task<TModel> Delete(TModel model)
        {
            var item = _mapper.Map<TEntity>(model);
            DbContext.Set<TEntity>().Remove(item);
            if (AutoSaveChanges)
                DbContext.SaveChanges();
            return model;
        }
        public virtual async Task<int> SaveChangesAsync()
        {
            return await DbContext.SaveChangesAsync();
        }

        /////
        public static string GetProp<T>(string key)
        {
            var itemType = typeof(T);
            return itemType.GetProperty(key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance).Name;
        }
    }
}