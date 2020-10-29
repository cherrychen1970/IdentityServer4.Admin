using Microsoft.AspNetCore.Identity;

namespace Skoruba.AspNetIdentity.EntityFramework.Models
{
    public class AspNetIdentityUser<TKey> : IdentityUser<TKey> where TKey : System.IEquatable<TKey>
    {

    }
}