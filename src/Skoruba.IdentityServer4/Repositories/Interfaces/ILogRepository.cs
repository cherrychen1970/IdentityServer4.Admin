using System;
using System.Threading.Tasks;
using Skoruba.EntityFramework.Entities;
using Skoruba.EntityFramework.Extensions.Common;

namespace Skoruba.EntityFramework.Repositories.Interfaces
{
    public interface ILogRepository
    {
        Task<PagedList<Log>> GetLogsAsync(string search, int page = 1, int pageSize = 10);

        Task DeleteLogsOlderThanAsync(DateTime deleteOlderThan);

        bool AutoSaveChanges { get; set; }
    }
}