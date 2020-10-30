using Microsoft.AspNetCore.Identity;

namespace Skoruba.AspNetIdentity.EntityFramework.Models
{
    // string key mapping
    public class User : IdentityUser<string>   {}
    public class Role : IdentityRole<string>   {}
    public class UserClaim : IdentityUserClaim<string>   {}
    public class UserRole : IdentityUserRole<string>   {}
    public class RoleClaim : IdentityRoleClaim<string>   {}
    public class UserLogin : IdentityUserLogin<string>   {}
    public class UserToken : IdentityUserToken<string>   {}

}