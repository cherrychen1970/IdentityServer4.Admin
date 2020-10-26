using Skoruba.AuditLogging.Events;
using Skoruba.Admin.BusinessLogic.Dtos.Configuration;

namespace Skoruba.Admin.BusinessLogic.Events.ApiResource
{
    public class ApiResourceAddedEvent : AuditEvent
    {
        public ApiResourceDto ApiResource { get; set; }

        public ApiResourceAddedEvent(ApiResourceDto apiResource)
        {
            ApiResource = apiResource;
        }
    }
}