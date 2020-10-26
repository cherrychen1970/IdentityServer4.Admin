using System;
using System.Threading.Tasks;
using Skoruba.Admin.BusinessLogic.Dtos.Log;

namespace Skoruba.Admin.BusinessLogic.Services.Interfaces
{
    public interface IAuditLogService
    {
        Task<AuditLogsDto> GetAsync(AuditLogFilterDto filters);

        Task DeleteLogsOlderThanAsync(DateTime deleteOlderThan);
    }
}
