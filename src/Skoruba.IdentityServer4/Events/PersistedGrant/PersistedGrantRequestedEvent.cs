using Skoruba.AuditLogging.Events;
using Skoruba.IdentityServer4.Dtos.Grant;

namespace Skoruba.IdentityServer4.Events.PersistedGrant
{
    public class PersistedGrantRequestedEvent : AuditEvent
    {
        public PersistedGrantDto PersistedGrant { get; set; }

        public PersistedGrantRequestedEvent(PersistedGrantDto persistedGrant)
        {
            PersistedGrant = persistedGrant;
        }
    }
}