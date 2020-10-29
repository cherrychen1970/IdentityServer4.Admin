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
        Task<TModel> Delete(TModel model);
    }
}