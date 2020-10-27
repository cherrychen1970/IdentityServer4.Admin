using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Skoruba.Identity.Dtos.Identity.Base;
using Skoruba.Identity.Dtos.Identity.Interfaces;

namespace Skoruba.Identity.Dtos
{
    public class UserRoleDto<TKey> : IdentityUserRole<TKey> where TKey : IEquatable<TKey>
    {      
    }
}