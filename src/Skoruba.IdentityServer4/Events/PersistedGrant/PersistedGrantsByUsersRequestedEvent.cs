using Skoruba.AuditLogging.Events;
using Skoruba.IdentityServer4.Dtos.Grant;

namespace Skoruba.IdentityServer4.Events.PersistedGrant
{
    public class PersistedGrantsByUsersRequestedEvent : AuditEvent
    {
        public PersistedGrantsDto PersistedGrants { get; set; }

        public PersistedGrantsByUsersRequestedEvent(PersistedGrantsDto persistedGrants)
        {
            PersistedGrants = persistedGrants;
        }
    }
}