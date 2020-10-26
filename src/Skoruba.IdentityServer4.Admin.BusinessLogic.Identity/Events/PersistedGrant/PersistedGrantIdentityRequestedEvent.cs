using Skoruba.AuditLogging.Events;
using Skoruba.Admin.BusinessLogic.Identity.Dtos.Grant;

namespace Skoruba.Admin.BusinessLogic.Identity.Events.PersistedGrant
{
    public class PersistedGrantIdentityRequestedEvent : AuditEvent
    {
        public PersistedGrantDto PersistedGrant { get; set; }

        public PersistedGrantIdentityRequestedEvent(PersistedGrantDto persistedGrant)
        {
            PersistedGrant = persistedGrant;
        }
    }
}