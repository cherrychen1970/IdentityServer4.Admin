using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skoruba.EntityFramework.Entities;
using Skoruba.EntityFramework.Extensions.Common;
using Skoruba.EntityFramework.Extensions.Enums;
using Skoruba.EntityFramework.Extensions.Extensions;
using Skoruba.EntityFramework.Interfaces;
using Skoruba.EntityFramework.Repositories.Interfaces;
using Skoruba.EntityFramework.Shared.DbContexts;

namespace Skoruba.EntityFramework.Repositories
{
    public class LogRepository : ILogRepository        
    {
        protected readonly AdminLogDbContext DbContext;

        public LogRepository(AdminLogDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public virtual async Task DeleteLogsOlderThanAsync(DateTime deleteOlderThan)
        {
            var logsToDelete = await DbContext.Logs.Where(x => x.TimeStamp < deleteOlderThan.Date).ToListAsync();

            if(logsToDelete.Count == 0) return;

            DbContext.Logs.RemoveRange(logsToDelete);

            await AutoSaveChangesAsync();
        }

        public virtual async Task<PagedList<Log>> GetLogsAsync(string search, int page = 1, int pageSize = 10)
        {
            Expression<Func<Log, bool>> searchCondition = x => x.LogEvent.Contains(search) || x.Message.Contains(search) || x.Exception.Contains(search);
            var logs = await DbContext.Logs
                .WhereIf(!string.IsNullOrEmpty(search), searchCondition)                
                .PageBy(x => x.Id, page, pageSize)
                .ToListAsync();

            var pagedList = new PagedList<Log>(logs);            
            pagedList.PageSize = pageSize;
            pagedList.TotalCount = await DbContext.Logs.WhereIf(!string.IsNullOrEmpty(search), searchCondition).CountAsync();

            return pagedList;
        }

        protected virtual async Task<int> AutoSaveChangesAsync()
        {
            return AutoSaveChanges ? await DbContext.SaveChangesAsync() : (int)SavedStatus.WillBeSavedExplicitly;
        }

        public virtual async Task<int> SaveAllChangesAsync()
        {
            return await DbContext.SaveChangesAsync();
        }

        public bool AutoSaveChanges { get; set; } = true;
    }
}