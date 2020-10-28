using System.ComponentModel.DataAnnotations;
using Skoruba.AspNetIdentity.Dtos.Base;
using Skoruba.AspNetIdentity.Dtos.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Skoruba.AspNetIdentity.Dtos
{
    public class UserClaimDto<TKey> : IdentityUserClaim<TKey> where TKey : System.IEquatable<TKey>
    {
    }
}