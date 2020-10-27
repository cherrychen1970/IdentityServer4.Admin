using Skoruba.AuditLogging.Events;
using Skoruba.IdentityServer4.Dtos.Configuration;

namespace Skoruba.IdentityServer4.Events.Client
{
    public class ClientsRequestedEvent : AuditEvent
    {
        public ClientsDto ClientsDto { get; set; }

        public ClientsRequestedEvent(ClientsDto clientsDto)
        {
            ClientsDto = clientsDto;
        }
    }
}