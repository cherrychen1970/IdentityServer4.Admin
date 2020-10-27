using Skoruba.AuditLogging.Events;
using Skoruba.IdentityServer4.Dtos.Configuration;

namespace Skoruba.IdentityServer4.Events.ApiResource
{
    public class ApiScopesRequestedEvent : AuditEvent
    {
        public ApiScopesDto ApiScope { get; set; }

        public ApiScopesRequestedEvent(ApiScopesDto apiScope)
        {
            ApiScope = apiScope;
        }
    }
}