using System.Linq;
using Microsoft.AspNetCore.Http;
using Skoruba.AuditLogging.Constants;
using Skoruba.AuditLogging.Events;
using Skoruba.Admin.Api.Configuration;

namespace Skoruba.Admin.Api.AuditLogging
{
    public class MockApiAuditSubject : IAuditSubject
    {
        public MockApiAuditSubject(IHttpContextAccessor accessor, AuditLoggingConfiguration auditLoggingConfiguration)
        {
 
        }

        public string SubjectName { get; set; }

        public string SubjectType { get; set; }

        public object SubjectAdditionalData { get; set; }

        public string SubjectIdentifier { get; set; }
    }
}
