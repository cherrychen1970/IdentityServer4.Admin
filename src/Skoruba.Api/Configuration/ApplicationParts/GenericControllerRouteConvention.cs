using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Skoruba.Admin.Api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Skoruba.Admin.Api.Configuration.ApplicationParts
{
    /*
    public class GenericControllerRouteConvention : IControllerModelConvention
    {
        
        public void Apply(ControllerModel controller)
        {
            if (controller.ControllerType.IsGenericType)
            {
                // this change is required because some of the controllers have generic parameters
                // and require resolution that will remove arity from the type 
                // as well as remove the 'Controller' at the end of string

                var name = controller.ControllerType.Name;
                var nameWithoutArity = name.Substring(0, name.IndexOf('`'));
                controller.ControllerName = nameWithoutArity.Substring(0, nameWithoutArity.LastIndexOf("Controller"));

                if (controller.ControllerType.GetGenericTypeDefinition() == typeof(RestController<,,>))
                {
                    var modelType = controller.ControllerType.GenericTypeArguments[1];
                    var route = $"api/{modelType.Name}s";
                    controller.Selectors.Add(new SelectorModel
                    {
                        AttributeRouteModel = new AttributeRouteModel(new RouteAttribute(route)),
                    });
                    Serilog.Log.Information($"dynamic api: {route}");
                }
            }
        }
    }
    */
}