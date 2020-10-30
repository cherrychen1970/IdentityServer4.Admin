using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Skoruba.Admin.Api.Configuration.ApplicationParts;
using IdentityServer4.EntityFramework.Entities;
using Skoruba.Repositories;
using Skoruba.IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Skoruba.IdentityServer4.EntityFramework.Repositories;
using Skoruba.IdentityServer4.Models;

using Skoruba.Admin.Api.Controllers;

namespace Skoruba.Admin.Api
{
    static public class ControllerExtensions
    {
        public static IMvcBuilder AddGenericControllers(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.ConfigureApplicationPartManager(m =>
            {
                m.FeatureProviders.Add(new GenericTypeControllerFeatureProvider());
            });
            return mvcBuilder;
        }
        public static IMvcBuilder AddDynamicControllers(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.ConfigureApplicationPartManager(m =>
                {
                    m.FeatureProviders.Add(new DynamicControllerProvider(feature =>
                        {
                            feature.AddDynamicControllers();
                        }
                    ));
                });
            return mvcBuilder;
        }

        static public void AddDynamicControllers(this ControllerFeature feature)
        {
            //feature.AddDynamicController<ClientRepository, Client, int>();
            feature.AddDynamicController<ClientSecretRepository, ClientSecret, int>();
            feature.AddDynamicController<ClientScopeRepository , ClientScope, int>();
            feature.AddDynamicController<ClientPropertyRepository,ClientProperty,int>();
            feature.AddDynamicController<ClientClaimRepository,ClientClaim,int>();
            feature.AddDynamicController<ClientGrantTypeRepository,ClientGrantType,int>();
            feature.AddDynamicController<ClientRedirectUriRepository,ClientRedirectUri,int>();

            feature.AddDynamicController<ApiResourceRepository, ApiResource, int>();
            feature.AddDynamicController<ApiResourceClaimRepository, ApiResourceClaim, int>();
            feature.AddDynamicController<ApiResourcePropertyRepository, ApiResourceProperty, int>();
            feature.AddDynamicController<ApiScopeRepository, ApiScope, int>();
            feature.AddDynamicController<ApiScopeClaimRepository, ApiScopeClaim, int>();
            feature.AddDynamicController<ApiSecretRepository, ApiSecret, int>();

            feature.AddDynamicController<IdentityResourceRepository, IdentityResource, int>();
            feature.AddDynamicController<IdentityPropertyRepository, IdentityResourceProperty, int>();
            feature.AddDynamicController<IdentityClaimRepository, IdentityClaim, int>();           
        }

        static public void AddDynamicController<TRepository, TModel, TKey>(this ControllerFeature feature)
        where TRepository : IRepository<TModel, TKey>
        where TModel : class
        {
            var type = typeof(RestController<TRepository, TModel, TKey>);     
            var route = new RouteAttribute(nameof(TModel));
            
            feature.Controllers.Add(type.GetTypeInfo());            
        }
    }
}