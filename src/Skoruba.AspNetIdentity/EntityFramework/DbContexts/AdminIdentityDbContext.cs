using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Skoruba.AspNetIdentity.EntityFramework.Models;

namespace Skoruba.AspNetIdentity.EntityFramework
{
    public abstract class AdminIdentityDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken> : IdentityDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken> 
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
        where TUserClaim : IdentityUserClaim<TKey>
        where TUserRole : IdentityUserRole<TKey>
        where TUserLogin : IdentityUserLogin<TKey>
        where TRoleClaim : IdentityRoleClaim<TKey>
        where TUserToken : IdentityUserToken<TKey>
    {        
        public AdminIdentityDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<TUser>().ToTable(TableConsts.IdentityUsers);
            builder.Entity<TRole>().ToTable(TableConsts.IdentityRoles);
            builder.Entity<TRoleClaim>().ToTable(TableConsts.IdentityRoleClaims);
            builder.Entity<TUserRole>().ToTable(TableConsts.IdentityUserRoles);            
            builder.Entity<TUserLogin>().ToTable(TableConsts.IdentityUserLogins);
            builder.Entity<TUserClaim>().ToTable(TableConsts.IdentityUserClaims);
            builder.Entity<TUserToken>().ToTable(TableConsts.IdentityUserTokens);
        }
    }

    // string based context
    public class AdminIdentityDbContext : AdminIdentityDbContext<User,Role,string,UserClaim,UserRole,UserLogin,RoleClaim,UserToken>
    {       
        public AdminIdentityDbContext(DbContextOptions<AdminIdentityDbContext> options) : base(options)
        {
        }    
    }
}