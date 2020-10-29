using Microsoft.AspNetCore.Identity;

namespace Skoruba.Admin.Api.EntityModels
{
    public class AdminIdentityUser : IdentityUser<System.Guid>
    {

    }

    public class AdminIdentityRole : IdentityRole<System.Guid>
    {

    }    
}