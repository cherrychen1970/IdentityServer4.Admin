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
    public class EntityRepository<TDbContext, TEntity, TKey> : IRepository<TEntity, TKey>
        where TDbContext : DbContext
        where TEntity : class        
        where TKey : IEquatable<TKey>
    {
        protected TDbContext DbContext { get; }        

        //protected readonly IAuditEventLogger AuditEventLogger;
        public bool AutoSaveChanges { get; set; } = true;
        public readonly ILogger _logger;

        public EntityRepository(TDbContext context, ILogger logger
        //, IAuditEventLogger auditEventLogger
        )
        {
            DbContext = context;            
            _logger = logger;
                        
            //AuditEventLogger = auditEventLogger;
        }

        virtual async public Task<TEntity> FindAsync(TKey id)
             => await DbContext.Set<TEntity>().FindAsync(id);//.Map<TEntity>(_mapper);

        virtual protected IQueryable<TEntity> OnSelect(DbSet<TEntity> set)
        {
            return set;
        }

        virtual public async Task<IPagedList<TEntity>> GetMany(int skip = 0, int take = 10, string orderby = null, bool asc = true)
        {
            var set = DbContext.Set<TEntity>();
            var query = OnSelect(set);
            var total = await query.CountAsync();
            var list = query.ToList();            
            //await AuditEventLogger.LogEventAsync(new CommonEvent());
            return list.ToPagedList(10, total);
        }
        virtual public async Task<IPagedList<TEntity>> GetMany(IDictionary<string, object> filter, int skip = 0, int take = 10, string orderby = null, bool asc = true)
        {
            var set = DbContext.Set<TEntity>();
            var query = OnSelect(set);
            var list = query.ToList();
            var total = await query.CountAsync();            
            return list.ToPagedList(10, total);
        }
        virtual public async Task<TEntity> GetOne(TKey id)
        {            
            var set = DbContext.Set<TEntity>().AsNoTracking();
            var item = await FindAsync(id);            
            return item;
        }

        virtual public async Task<TEntity> Create(TEntity item)
        {            
            var set = DbContext.Set<TEntity>();
            await set.AddAsync(item);
            if (AutoSaveChanges)
                DbContext.SaveChanges();
            return item;
        }

        virtual public async Task<TEntity> Update(TKey id, TEntity itemInput)
        {            
            var item = await FindAsync(id);
            DbContext.Entry(item).CurrentValues.SetValues(itemInput);
            if (AutoSaveChanges)
                DbContext.SaveChanges();
            return item;
        }

        virtual public async Task<TEntity> Patch(TKey id, IDictionary<string, object> values)
        {
            var item = await FindAsync(id);
            DbContext.Entry(item).CurrentValues.SetValues(values);
            if (AutoSaveChanges)
                DbContext.SaveChanges();
            return item;
        }

        virtual public async Task<TEntity> Delete(TKey id)
        {
            var item = await FindAsync(id);            
            DbContext.Set<TEntity>().Remove(item);
            if (AutoSaveChanges)
                DbContext.SaveChanges();
            return item;
        }

        virtual public async Task<TEntity> Delete(TEntity item)
        {            
            DbContext.Set<TEntity>().Remove(item);
            if (AutoSaveChanges)
                DbContext.SaveChanges();
            return item;
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

    public class EntityRepository<TDbContext, TEntity> : EntityRepository<TDbContext,TEntity, int>
        where TDbContext : DbContext
        where TEntity : class        
    {
        public EntityRepository(TDbContext context, ILogger logger
        //, IAuditEventLogger auditEventLogger
        ) : base(context,logger)
        {
        }        
    }
}