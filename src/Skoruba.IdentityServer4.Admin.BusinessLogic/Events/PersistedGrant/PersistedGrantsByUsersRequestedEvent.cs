using Skoruba.AuditLogging.Events;
using Skoruba.Admin.BusinessLogic.Dtos.Grant;

namespace Skoruba.Admin.BusinessLogic.Events.PersistedGrant
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