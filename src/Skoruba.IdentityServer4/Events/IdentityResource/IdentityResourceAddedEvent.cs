using Skoruba.AuditLogging.Events;
using Skoruba.IdentityServer4.Dtos.Configuration;

namespace Skoruba.IdentityServer4.Events.IdentityResource
{
    public class IdentityResourceAddedEvent : AuditEvent
    {
        public IdentityResourceDto IdentityResource { get; set; }

        public IdentityResourceAddedEvent(IdentityResourceDto identityResource)
        {
            IdentityResource = identityResource;
        }
    }
}