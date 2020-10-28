using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Skoruba.AspNetIdentity.Dtos.Base;
using Skoruba.AspNetIdentity.Dtos.Interfaces;

namespace Skoruba.AspNetIdentity.Dtos
{
    public class UserRoleDto<TKey> : IdentityUserRole<TKey> where TKey : IEquatable<TKey>
    {      
    }
}