using System.ComponentModel.DataAnnotations;
using Skoruba.AspNetIdentity.Models.Base;
using Skoruba.AspNetIdentity.Models.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Skoruba.AspNetIdentity.Models
{
    public class UserClaimDto<TKey> : IdentityUserClaim<TKey> where TKey : System.IEquatable<TKey>
    {
    }
}