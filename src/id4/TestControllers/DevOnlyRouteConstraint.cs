using System;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;


namespace id4.Api.TestControllers
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method,AllowMultiple = false)]
    public class DevOnlyRouteConstraint : ActionMethodSelectorAttribute
    {
        public DevOnlyRouteConstraint()
        {            
        }
        public override bool IsValidForRequest(RouteContext routeContext, ActionDescriptor action)
        {
#if DEBUG            
            return true;
#else
            return false;
#endif            
        }
    }
}