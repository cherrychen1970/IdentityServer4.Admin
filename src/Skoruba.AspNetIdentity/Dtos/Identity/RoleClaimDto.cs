using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Skoruba.AspNetIdentity.Dtos.Base;
using Skoruba.AspNetIdentity.Dtos.Interfaces;

namespace Skoruba.AspNetIdentity.Dtos
{
    public class RoleClaimDto<TKey> : IdentityRoleClaim<TKey> where TKey : System.IEquatable<TKey>
    {
        [Required]
        override public string ClaimType { get; set; }


        [Required]
        override public string ClaimValue { get; set; }
    }
}
