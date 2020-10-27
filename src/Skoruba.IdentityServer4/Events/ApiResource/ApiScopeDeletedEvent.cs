using Skoruba.AuditLogging.Events;
using Skoruba.IdentityServer4.Dtos.Configuration;

namespace Skoruba.IdentityServer4.Events.ApiResource
{
    public class ApiScopeDeletedEvent : AuditEvent
    {
        public ApiScopesDto ApiScope { get; set; }

        public ApiScopeDeletedEvent(ApiScopesDto apiScope)
        {
            ApiScope = apiScope;
        }
    }
}