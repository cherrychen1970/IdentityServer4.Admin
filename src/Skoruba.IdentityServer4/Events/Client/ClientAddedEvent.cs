using Skoruba.AuditLogging.Events;
using Skoruba.IdentityServer4.Dtos.Configuration;

namespace Skoruba.IdentityServer4.Events.Client
{
    public class ClientAddedEvent : AuditEvent
    {
        public ClientDto Client { get; set; }

        public ClientAddedEvent(ClientDto client)
        {
            Client = client;
        }
    }
}