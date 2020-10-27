using Skoruba.AuditLogging.Events;
using Skoruba.IdentityServer4.Dtos.Configuration;

namespace Skoruba.IdentityServer4.Events.Client
{
    public class ClientClonedEvent : AuditEvent
    {
        public ClientCloneDto Client { get; set; }

        public ClientClonedEvent(ClientCloneDto client)
        {
            Client = client;
        }
    }
}