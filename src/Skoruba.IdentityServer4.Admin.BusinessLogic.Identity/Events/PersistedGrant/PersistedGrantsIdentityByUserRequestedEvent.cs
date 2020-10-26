using Skoruba.AuditLogging.Events;
using Skoruba.Admin.BusinessLogic.Identity.Dtos.Grant;

namespace Skoruba.Admin.BusinessLogic.Identity.Events.PersistedGrant
{
    public class PersistedGrantsIdentityByUserRequestedEvent : AuditEvent
    {
        public PersistedGrantsDto PersistedGrants { get; set; }

        public PersistedGrantsIdentityByUserRequestedEvent(PersistedGrantsDto persistedGrants)
        {
            PersistedGrants = persistedGrants;
        }
    }
}