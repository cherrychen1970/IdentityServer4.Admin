using System;
using System.Threading.Tasks;
using Skoruba.Admin.EntityFramework.Entities;
using Skoruba.Admin.EntityFramework.Extensions.Common;

namespace Skoruba.Admin.EntityFramework.Repositories.Interfaces
{
    public interface ILogRepository
    {
        Task<PagedList<Log>> GetLogsAsync(string search, int page = 1, int pageSize = 10);

        Task DeleteLogsOlderThanAsync(DateTime deleteOlderThan);

        bool AutoSaveChanges { get; set; }
    }
}