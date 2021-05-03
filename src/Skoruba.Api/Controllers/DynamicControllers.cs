using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using IdentityServer4.EntityFramework.Entities;

using Bluebird.Repositories;

using Skoruba.IdentityServer4.EntityFramework.Repositories;
using Skoruba.IdentityServer4.Models;
using Skoruba.AspNetIdentity.EntityFramework.Repositories;
using Skoruba.AspNetIdentity.EntityFramework.Models;
using Skoruba.AspNetIdentity.Models;


namespace Skoruba.Admin.Api.Controllers
{
    static public class Extensions
    {
        static public void AddDynamicControllers(this ControllerFeature feature)
        {
            feature.AddDynamicController<ClientRepository,  ClientDto,Client,  int>();
            //feature.AddDynamicController<ClientSecretRepository, ClientSecretDto, int>();
            //feature.AddDynamicController<ClientScopeRepository , ClientScope, int>();
            //feature.AddDynamicController<ClientPropertyRepository,ClientPropertyDto,int>();
            //feature.AddDynamicController<ClientClaimRepository,ClientClaimDto,int>();
            //feature.AddDynamicController<ClientGrantTypeRepository,ClientGrantType,int>();
            //feature.AddDynamicController<ClientRedirectUriRepository,ClientRedirectUri,int>();

            //feature.AddDynamicController<ApiResourceRepository, ApiResourceDto, int>();
            //feature.AddDynamicController<ApiResourceClaimRepository, ApiResourceClaim, int>();
            //feature.AddDynamicController<ApiResourcePropertyRepository, ApiResourcePropertyDto, int>();
            //feature.AddDynamicController<ApiScopeRepository, ApiScopeDto, int>();
            //feature.AddDynamicController<ApiScopeClaimRepository, ApiScopeClaim, int>();
            //feature.AddDynamicController<ApiSecretRepository, ApiSecretDto, int>();

            //feature.AddDynamicController<IdentityResourceRepository, IdentityResourceDto, int>();
            //feature.AddDynamicController<IdentityPropertyRepository, IdentityResourcePropertyDto, int>();
            //feature.AddDynamicController<IdentityClaimRepository, IdentityClaim, int>();           

            ///////////////////////////////////////////////////////////////////////
            // AspNetIdentity 
            
            feature.AddDynamicController<UserRepository, UserDto<string>,User, string>();           
            //feature.AddDynamicController<UserClaimRepository, UserClaim, int>();           
            //feature.AddDynamicController<UserRoleRepository, UserRoleDto<int>, int>();           
            //feature.AddDynamicController<UserLoginRepository, UserLogin, string>();           
            //feature.AddDynamicController<UserTokenRepository, UserToken, string>();           
            //feature.AddDynamicController<RoleRepository, RoleDto<string>, string>();           
            //feature.AddDynamicController<RoleClaimRepository, RoleClaimDto<int>, int>();           
        }

        static public void AddDynamicController<TRepository, TModel,TEntity, TKey>(this ControllerFeature feature)
        where TRepository : IRepository<TEntity, TKey>
        where TModel : class
        where TEntity : class
        where TKey : IEquatable<TKey>
        {
            var type = typeof(RestController<TRepository, TModel, TEntity, TKey>);     
            var route = new RouteAttribute(nameof(TModel));
            
            feature.Controllers.Add(type.GetTypeInfo());            
        }
    }
}