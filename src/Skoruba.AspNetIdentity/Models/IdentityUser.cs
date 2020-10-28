using Microsoft.AspNetCore.Identity;

namespace Skoruba.AspNetIdentity.Models
{
public class AspNetIdentityUser<TKey> : IdentityUser<TKey> where TKey : System.IEquatable<TKey>
{

}
}