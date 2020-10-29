using Microsoft.AspNetCore.Identity;

namespace Skoruba.Admin.EntityModels
{
    public class AdminIdentityUser : IdentityUser<System.Guid>
    {

    }

    public class AdminIdentityRole : IdentityRole<System.Guid>
    {

    }    
}