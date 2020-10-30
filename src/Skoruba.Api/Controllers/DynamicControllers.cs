using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using IdentityServer4.EntityFramework.Entities;

using Skoruba.Repositories;

using Skoruba.IdentityServer4.EntityFramework.Repositories;
using Skoruba.AspNetIdentity.EntityFramework.Repositories;
using Skoruba.AspNetIdentity.EntityFramework.Models;


namespace Skoruba.Admin.Api.Controllers
{
    static public class Extensions
    {
        static public void AddDynamicControllers(this ControllerFeature feature)
        {
            feature.AddDynamicController<ClientRepository, Client, int>();
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

            ///////////////////////////////////////////////////////////////////////
            // AspNetIdentity 
            
            feature.AddDynamicController<UserRepository, User, string>();           
            //feature.AddDynamicController<UserClaimRepository, UserClaim, int>();           
            feature.AddDynamicController<UserRoleRepository, UserRole, int>();           
            feature.AddDynamicController<UserLoginRepository, UserLogin, string>();           
            feature.AddDynamicController<UserTokenRepository, UserToken, string>();           
            feature.AddDynamicController<RoleRepository, Role, string>();           
            feature.AddDynamicController<RoleClaimRepository, RoleClaim, int>();           
        }

        static public void AddDynamicController<TRepository, TModel, TKey>(this ControllerFeature feature)
        where TRepository : IRepository<TModel, TKey>
        where TModel : class
        where TKey : IEquatable<TKey>
        {
            var type = typeof(RestController<TRepository, TModel, TKey>);     
            var route = new RouteAttribute(nameof(TModel));
            
            feature.Controllers.Add(type.GetTypeInfo());            
        }
    }
}