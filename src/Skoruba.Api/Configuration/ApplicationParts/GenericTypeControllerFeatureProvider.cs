using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Skoruba.AspNetIdentity.Dtos;

namespace Skoruba.Admin.Api.Configuration.ApplicationParts
{
    public class GenericTypeControllerFeatureProvider<TUserDto, RoleDto<TKey>, TUser, IdentityRole<TKey>, TKey, IdentityUserClaim<TKey>, IdentityUserRole<TKey>, IdentityUserLogin<TKey>, IdentityRoleClaim<TKey>, IdentityUserToken<TKey>,
        TUsersDto, IdentityRolesDto<TKey>, IdentityUserRole<TKey>sDto, IdentityUserClaim<TKey>sDto,
        UserProviderDto<TKey>, TUserProvidersDto, UserChangePasswordDto<TKey>, IdentityRoleClaim<TKey>sDto, IdentityUserClaim<TKey>Dto, IdentityRoleClaim<TKey>Dto> : IApplicationFeatureProvider<ControllerFeature>
        where TUserDto : UserDto<TKey>, new()
        where RoleDto<TKey> : RoleDto<TKey>, new()
        where TUser : IdentityUser<TKey>
        where IdentityRole<TKey> : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
        where IdentityUserClaim<TKey> : IdentityUserClaim<TKey>
        where IdentityUserRole<TKey> : IdentityUserRole<TKey>
        where IdentityUserLogin<TKey> : IdentityUserLogin<TKey>
        where IdentityRoleClaim<TKey> : IdentityRoleClaim<TKey>
        where IdentityUserToken<TKey> : IdentityUserToken<TKey>


        where TUsersDto : UsersDto<TUserDto, TKey>
        where IdentityRolesDto<TKey> : RolesDto<RoleDto<TKey>, TKey>
        where IdentityUserRole<TKey>sDto : UserRolesDto<RoleDto<TKey>, TKey>
        where IdentityUserClaim<TKey>sDto : UserClaimsDto<IdentityUserClaim<TKey>Dto, TKey>
        where UserProviderDto<TKey> : UserProviderDto<TKey>
        where TUserProvidersDto : UserProvidersDto<UserProviderDto<TKey>, TKey>
        where UserChangePasswordDto<TKey> : UserChangePasswordDto<TKey>
        where IdentityRoleClaim<TKey>sDto : RoleClaimsDto<IdentityRoleClaim<TKey>Dto, TKey>
        where IdentityUserClaim<TKey>Dto : UserClaimDto<TKey>
        where IdentityRoleClaim<TKey>Dto : RoleClaimDto<TKey>
    {
        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            var currentAssembly = typeof(GenericTypeControllerFeatureProvider<TUserDto, RoleDto<TKey>, TUser, IdentityRole<TKey>, TKey, IdentityUserClaim<TKey>, IdentityUserRole<TKey>, IdentityUserLogin<TKey>, IdentityRoleClaim<TKey>, IdentityUserToken<TKey>,
                TUsersDto, IdentityRolesDto<TKey>, IdentityUserRole<TKey>sDto, IdentityUserClaim<TKey>sDto,
                UserProviderDto<TKey>, TUserProvidersDto, UserChangePasswordDto<TKey>, IdentityRoleClaim<TKey>sDto, IdentityUserClaim<TKey>Dto, IdentityRoleClaim<TKey>Dto>).Assembly;
            var controllerTypes = currentAssembly.GetExportedTypes()
                                                 .Where(t => typeof(ControllerBase).IsAssignableFrom(t) && t.IsGenericTypeDefinition)
                                                 .Select(t => t.GetTypeInfo());

            var type = this.GetType();
            var genericType = type.GetGenericTypeDefinition().GetTypeInfo();
            var parameters = genericType.GenericTypeParameters
                                        .Select((p, i) => new { p.Name, Index = i })
                                        .ToDictionary(a => a.Name, a => type.GenericTypeArguments[a.Index]);

            foreach (var controllerType in controllerTypes)
            {
                var typeArguments = controllerType.GenericTypeParameters
                                                  .Select(p => parameters[p.Name])
                                                  .ToArray();

                feature.Controllers.Add(controllerType.MakeGenericType(typeArguments).GetTypeInfo());
            }
        }
    }
}