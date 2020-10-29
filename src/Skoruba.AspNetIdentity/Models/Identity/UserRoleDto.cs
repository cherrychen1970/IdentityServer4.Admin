using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Skoruba.AspNetIdentity.Models.Base;
using Skoruba.AspNetIdentity.Models.Interfaces;

namespace Skoruba.AspNetIdentity.Models
{
    public class UserRoleDto<TKey> : IdentityUserRole<TKey> where TKey : IEquatable<TKey>
    {      
    }
}