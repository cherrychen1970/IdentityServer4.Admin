using Skoruba.AuditLogging.Events;
using Skoruba.Admin.BusinessLogic.Dtos.Configuration;

namespace Skoruba.Admin.BusinessLogic.Events.IdentityResource
{
    public class IdentityResourcePropertyDeletedEvent : AuditEvent
    {
        public IdentityResourcePropertiesDto IdentityResourceProperty { get; set; }

        public IdentityResourcePropertyDeletedEvent(IdentityResourcePropertiesDto identityResourceProperty)
        {
            IdentityResourceProperty = identityResourceProperty;
        }
    }
}