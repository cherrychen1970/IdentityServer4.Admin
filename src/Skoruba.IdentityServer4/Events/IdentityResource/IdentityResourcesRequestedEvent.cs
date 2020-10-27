using Skoruba.AuditLogging.Events;
using Skoruba.IdentityServer4.Dtos.Configuration;

namespace Skoruba.IdentityServer4.Events.IdentityResource
{
    public class IdentityResourcesRequestedEvent : AuditEvent
    {
        public IdentityResourcesDto IdentityResources { get; }

        public IdentityResourcesRequestedEvent(IdentityResourcesDto identityResources)
        {
            IdentityResources = identityResources;
        }
    }
}