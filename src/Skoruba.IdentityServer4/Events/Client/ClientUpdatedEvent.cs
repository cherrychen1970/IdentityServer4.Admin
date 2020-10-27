using Skoruba.AuditLogging.Events;
using Skoruba.IdentityServer4.Dtos.Configuration;

namespace Skoruba.IdentityServer4.Events.Client
{
    public class ClientUpdatedEvent: AuditEvent
    {
        public ClientDto OriginalClient { get; set; }
        public ClientDto Client { get; set; }

        public ClientUpdatedEvent(ClientDto originalClient, ClientDto client)
        {
            OriginalClient = originalClient;
            Client = client;
        }
    }
}