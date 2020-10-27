using System;
using System.Threading.Tasks;
using Skoruba.IdentityServer4.Dtos.Log;

namespace Skoruba.IdentityServer4.Services.Interfaces
{
    public interface IAuditLogService
    {
        Task<AuditLogsDto> GetAsync(AuditLogFilterDto filters);

        Task DeleteLogsOlderThanAsync(DateTime deleteOlderThan);
    }
}
