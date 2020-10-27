using System.ComponentModel.DataAnnotations;
using Skoruba.Identity.Dtos.Identity.Base;
using Skoruba.Identity.Dtos.Identity.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Skoruba.Identity.Dtos.Identity
{
    public class UserClaimDto<TKey> : IdentityUserClaim<TKey> where TKey : System.IEquatable<TKey>
    {
    }
}