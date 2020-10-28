using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Skoruba.EntityFramework.Shared.Constants;
using Skoruba.EntityFramework.Shared.DbContexts;

namespace Skoruba.Admin.EntityModels
{
    public class AdminIdentityDbContext : AdminIdentityDbContext<Guid>                
    {
        public AdminIdentityDbContext(DbContextOptions options) : base(options)
        {
        }        
    }
}