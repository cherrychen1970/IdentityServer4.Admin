using System;
using System.Threading.Tasks;
using Skoruba.Admin.BusinessLogic.Dtos.Log;

namespace Skoruba.Admin.BusinessLogic.Services.Interfaces
{
    public interface ILogService
    {
        Task<LogsDto> GetLogsAsync(string search, int page = 1, int pageSize = 10);

        Task DeleteLogsOlderThanAsync(DateTime deleteOlderThan);
    }
}