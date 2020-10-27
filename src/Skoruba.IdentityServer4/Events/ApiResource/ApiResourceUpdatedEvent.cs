using Skoruba.AuditLogging.Events;
using Skoruba.IdentityServer4.Dtos.Configuration;

namespace Skoruba.IdentityServer4.Events.ApiResource
{
    public class ApiResourceUpdatedEvent : AuditEvent
    {
        public ApiResourceDto OriginalApiResource { get; set; }
        public ApiResourceDto ApiResource { get; set; }

        public ApiResourceUpdatedEvent(ApiResourceDto originalApiResource, ApiResourceDto apiResource)
        {
            OriginalApiResource = originalApiResource;
            ApiResource = apiResource;
        }
    }
}