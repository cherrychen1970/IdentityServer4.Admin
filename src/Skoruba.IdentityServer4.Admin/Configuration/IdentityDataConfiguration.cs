using Skoruba.Admin.EntityFramework.Configuration.Identity;
using System.Collections.Generic;

namespace Skoruba.Admin.Configuration
{
    public class IdentityDataConfiguration
    {
       public List<Role> Roles { get; set; }
       public List<User> Users { get; set; }
    }
}
