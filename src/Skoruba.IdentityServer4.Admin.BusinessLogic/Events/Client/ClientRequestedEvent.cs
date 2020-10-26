using Skoruba.AuditLogging.Events;
using Skoruba.Admin.BusinessLogic.Dtos.Configuration;

namespace Skoruba.Admin.BusinessLogic.Events.Client
{
    public class ClientRequestedEvent : AuditEvent
    {
        public ClientDto ClientDto { get; set; }

        public ClientRequestedEvent(ClientDto clientDto)
        {
            ClientDto = clientDto;
        }
    }
}