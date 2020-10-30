using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Skoruba.AspNetIdentity.Models;

namespace Skoruba.Admin.Api.Configuration.ApplicationParts
{
    public class DynamicControllerProvider: IApplicationFeatureProvider<ControllerFeature>
    {        
        private readonly Action<ControllerFeature> _func;
        public DynamicControllerProvider(Action<ControllerFeature> func)
        {
            _func = func;
        }

        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            _func(feature);
        }
    }
}