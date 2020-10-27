using System;
using System.Threading.Tasks;
using Skoruba.IdentityServer4.Dtos.Log;

namespace Skoruba.IdentityServer4.Services.Interfaces
{
    public interface ILogService
    {
        Task<LogsDto> GetLogsAsync(string search, int page = 1, int pageSize = 10);

        Task DeleteLogsOlderThanAsync(DateTime deleteOlderThan);
    }
}