using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Skoruba.AspNetIdentity.Models.Base;
using Skoruba.AspNetIdentity.Models.Interfaces;

namespace Skoruba.AspNetIdentity.Models
{
    public class RoleClaimDto<TKey> : IdentityRoleClaim<TKey> where TKey : System.IEquatable<TKey>
    {
        [Required]
        override public string ClaimType { get; set; }


        [Required]
        override public string ClaimValue { get; set; }
    }
}
