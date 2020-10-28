using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Dynamic;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.EntityFramework.DbContexts;
//using Skoruba.AuditLogging.Services;
using Skoruba.Core.Dtos.Common;


using AutoMapper;

namespace Skoruba.Core.Repositories
{

    public static class Extension
    {
        public static T Map<T>(this object item, IMapper mapper)
        {
            return mapper.Map<T>(item);

        }
    }
    public interface IRepository<TModel, TKey>
    {
        bool AutoSaveChanges { get; set; }
        Task<IPagedList<TModel>> GetMany(int skip = 0, int take = 10, string orderby = null, bool asc = true);
        Task<IPagedList<TModel>> GetMany(IDictionary<string, object> filter, int skip = 0, int take = 10, string orderBy = null, bool asc = true);
        Task<TModel> GetOne(TKey id);
        Task<TModel> Create(TModel model);
        Task<TModel> Update(TKey id, TModel model);
        Task<TModel> Patch(TKey id, IDictionary<string, object> model);
        Task<TModel> Delete(TKey id);
    }


    public class Repository<TDbContext, TEntity, TModel, TKey> : IRepository<TModel, TKey>
        where TDbContext : DbContext
        where TEntity : class
        where TModel : class
    {
        protected TDbContext DbContext { get; }
        protected IMapper Mapper { get; }

        //protected readonly IAuditEventLogger AuditEventLogger;
        public bool AutoSaveChanges { get; set; } = true;


        public Repository(TDbContext context, IMapper mapper
        //, IAuditEventLogger auditEventLogger
        )
        {
            DbContext = context;
            Mapper = mapper;
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
            var list = entities.Select(x => Mapper.Map<TModel>(x));
            //await AuditEventLogger.LogEventAsync(new CommonEvent());
            return list.ToPagedList(10, total);
        }
        virtual public async Task<IPagedList<TModel>> GetMany(IDictionary<string, object> filter, int skip = 0, int take = 10, string orderby = null, bool asc = true)
        {
            var set = DbContext.Set<TEntity>();
            var query = OnSelect(set);
            var entities = query.ToList();
            var total = await query.CountAsync();
            var list = entities.Select(x => Mapper.Map<TModel>(x));
            return list.ToPagedList(10, total);
        }
        virtual public async Task<TModel> GetOne(TKey id)
        {
            var set = DbContext.Set<TEntity>().AsNoTracking();
            var item = await FindAsync(id);
            var model = Mapper.Map<TModel>(item);
            return model;
        }

        virtual public async Task<TModel> Create(TModel model)
        {
            var entity = Mapper.Map<TEntity>(model);
            var set = DbContext.Set<TEntity>();
            await set.AddAsync(entity);
            if (AutoSaveChanges)
                DbContext.SaveChanges();
            return model;
        }

        virtual public async Task<TModel> Update(TKey id, TModel model)
        {
            var entity = Mapper.Map<TEntity>(model);
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
            return Mapper.Map<TModel>(item);
        }

        virtual public async Task<TModel> Delete(TKey id)
        {
            var item = await FindAsync(id);
            var model = Mapper.Map<TModel>(item);
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

    public class Repository<TDbContext, TEntity> : Repository<TDbContext, TEntity, TEntity, int>
        where TDbContext : DbContext
        where TEntity : class
    {
        public Repository(TDbContext context, IMapper mapper) : base(context, mapper)


        {

        }
    }
}