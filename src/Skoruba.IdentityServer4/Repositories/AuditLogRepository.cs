﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skoruba.AuditLogging.EntityFramework.DbContexts;
using Skoruba.AuditLogging.EntityFramework.Entities;
using Skoruba.Core.Dtos.Common;
using Skoruba.EntityFramework.Extensions.Extensions;
using Skoruba.EntityFramework.Repositories.Interfaces;
using Skoruba.EntityFramework.Shared.DbContexts;

namespace Skoruba.EntityFramework.Repositories
{
    public class AuditLogRepository<TAuditLog> : IAuditLogRepository<TAuditLog> where TAuditLog : AuditLog
    {
        protected readonly AdminAuditLogDbContext DbContext;

        public AuditLogRepository(AdminAuditLogDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<PagedList<TAuditLog>> GetAsync(string @event, string source, string category, DateTime? created, string subjectIdentifier, string subjectName, int page = 1, int pageSize = 10)
        {
            var auditLogs = await DbContext.AuditLog
                .WhereIf(!string.IsNullOrEmpty(subjectIdentifier), log => log.SubjectIdentifier.Contains(subjectIdentifier))
                .WhereIf(!string.IsNullOrEmpty(subjectName), log => log.SubjectName.Contains(subjectName))
                .WhereIf(!string.IsNullOrEmpty(@event), log => log.Event.Contains(@event))
                .WhereIf(!string.IsNullOrEmpty(source), log => log.Source.Contains(source))
                .WhereIf(!string.IsNullOrEmpty(category), log => log.Category.Contains(category))
                .WhereIf(created.HasValue, log => log.Created.Date == created.Value.Date)
                .PageBy(x => x.Id, page, pageSize)
                .ToListAsync();

            var pagedList = new PagedList<TAuditLog>(auditLogs);            
            pagedList.PageSize = pageSize;
            pagedList.TotalCount = await DbContext.AuditLog
                .WhereIf(!string.IsNullOrEmpty(subjectIdentifier), log => log.SubjectIdentifier.Contains(subjectIdentifier))
                .WhereIf(!string.IsNullOrEmpty(subjectName), log => log.SubjectName.Contains(subjectName))
                .WhereIf(!string.IsNullOrEmpty(@event), log => log.Event.Contains(@event))
                .WhereIf(!string.IsNullOrEmpty(source), log => log.Source.Contains(source))
                .WhereIf(!string.IsNullOrEmpty(category), log => log.Category.Contains(category))
                .WhereIf(created.HasValue, log => log.Created.Date == created.Value.Date)
                .CountAsync();
            
            return pagedList;
        }

        public virtual async Task DeleteLogsOlderThanAsync(DateTime deleteOlderThan)
        {
            var logsToDelete = await DbContext.AuditLog.Where(x => x.Created.Date < deleteOlderThan.Date).ToListAsync();

            if (logsToDelete.Count == 0) return;

            DbContext.AuditLog.RemoveRange(logsToDelete);

            await AutoSaveChangesAsync();
        }

        protected virtual async Task<int> AutoSaveChangesAsync()
        {
            return AutoSaveChanges ? await DbContext.SaveChangesAsync() : (int)SavedStatus.WillBeSavedExplicitly;
        }

        public bool AutoSaveChanges { get; set; } = true;

        public virtual async Task<int> SaveAllChangesAsync()
        {
            return await DbContext.SaveChangesAsync();
        }
    }
}
