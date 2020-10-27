using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Skoruba.Identity.Dtos.Identity.Base;
using Skoruba.Identity.Dtos.Identity.Interfaces;

namespace Skoruba.Identity.Dtos.Identity
{
    public class RoleClaimDto<TKey> : IdentityRoleClaim<TKey> where TKey : System.IEquatable<TKey>
    {
        [Required]
        override public string ClaimType { get; set; }


        [Required]
        override public string ClaimValue { get; set; }
    }
}
