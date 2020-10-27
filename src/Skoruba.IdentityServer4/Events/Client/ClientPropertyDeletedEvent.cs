using Skoruba.AuditLogging.Events;
using Skoruba.IdentityServer4.Dtos.Configuration;

namespace Skoruba.IdentityServer4.Events.Client
{
    public class ClientPropertyDeletedEvent : AuditEvent
    {
        public ClientPropertiesDto ClientProperty { get; set; }

        public ClientPropertyDeletedEvent(ClientPropertiesDto clientProperty)
        {
            ClientProperty = clientProperty;
        }
    }
}