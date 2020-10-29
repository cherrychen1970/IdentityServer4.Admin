using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Skoruba.AspNetIdentity.EntityFramework
{
    public class IdentityDbContext<TUser, TKey> : IdentityDbContext<
        TUser, IdentityRole<TKey>, TKey, IdentityUserClaim<TKey>, IdentityUserRole<TKey>,
        IdentityUserLogin<TKey>, IdentityRoleClaim<TKey>, IdentityUserToken<TKey>>
            where TUser : IdentityUser<TKey>
            where TKey : IEquatable<TKey>
    {
        public IdentityDbContext(DbContextOptions options) : base(options)
        {
        }
    }
    public class AdminIdentityDbContext<TKey> : IdentityDbContext<IdentityUser<TKey>, TKey>
                where TKey : IEquatable<TKey>
    {
        public AdminIdentityDbContext(DbContextOptions options) : base(options)
        {
        }
        public AdminIdentityDbContext(DbContextOptions<AdminIdentityDbContext<TKey>> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ConfigureIdentityContext(builder);
        }

        private void ConfigureIdentityContext(ModelBuilder builder)
        {
            builder.Entity<IdentityRole<TKey>>().ToTable(TableConsts.IdentityRoles);
            builder.Entity<IdentityRoleClaim<TKey>>().ToTable(TableConsts.IdentityRoleClaims);
            builder.Entity<IdentityUserRole<TKey>>().ToTable(TableConsts.IdentityUserRoles);
            builder.Entity<IdentityUser<TKey>>().ToTable(TableConsts.IdentityUsers);
            builder.Entity<IdentityUserLogin<TKey>>().ToTable(TableConsts.IdentityUserLogins);
            builder.Entity<IdentityUserClaim<TKey>>().ToTable(TableConsts.IdentityUserClaims);
            builder.Entity<IdentityUserToken<TKey>>().ToTable(TableConsts.IdentityUserTokens);
        }
    }

    public class AdminIdentityDbContext : AdminIdentityDbContext<string>
    {
        public AdminIdentityDbContext(DbContextOptions<AdminIdentityDbContext> options) : base(options)
        {
        }
    }
}