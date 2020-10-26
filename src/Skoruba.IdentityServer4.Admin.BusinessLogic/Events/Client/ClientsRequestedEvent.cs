using Skoruba.AuditLogging.Events;
using Skoruba.Admin.BusinessLogic.Dtos.Configuration;

namespace Skoruba.Admin.BusinessLogic.Events.Client
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