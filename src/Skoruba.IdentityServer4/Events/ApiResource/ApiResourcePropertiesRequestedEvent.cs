using Skoruba.AuditLogging.Events;
using Skoruba.IdentityServer4.Dtos.Configuration;

namespace Skoruba.IdentityServer4.Events.ApiResource
{
    public class ApiResourcePropertiesRequestedEvent : AuditEvent
    {
        public ApiResourcePropertiesRequestedEvent(int apiResourceId, ApiResourcePropertiesDto apiResourceProperties)
        {
            ApiResourceId = apiResourceId;
            ApiResourceProperties = apiResourceProperties;
        }

        public int ApiResourceId { get; set; }
        public ApiResourcePropertiesDto ApiResourceProperties { get; }
    }
}