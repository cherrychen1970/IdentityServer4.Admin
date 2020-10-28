using System.Collections.Generic;
using Skoruba.AuditLogging.Events;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Skoruba.Core.Events
{
    public class CommonEvent : AuditEvent
    {        
        dynamic Something;

        public CommonEvent(dynamic a, [CallerMemberName] string callerName = "")
        {
            Something=a;            
        }
    }
}