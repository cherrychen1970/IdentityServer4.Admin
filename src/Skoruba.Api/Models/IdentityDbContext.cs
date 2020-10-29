using System;
using Microsoft.EntityFrameworkCore;
using Skoruba.AspNetIdentity.EntityFramework;

namespace Skoruba.Admin.Api.EntityModels
{
    public class AdminIdentityDbContext : AdminIdentityDbContext<Guid>                
    {
        public AdminIdentityDbContext(DbContextOptions options) : base(options)
        {
        }        
    }
}