using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SkorubaIdentityServer4Admin.Admin.EntityFramework.Shared.Constants;
using SkorubaIdentityServer4Admin.Admin.EntityFramework.Shared.Entities.Identity;

namespace SkorubaIdentityServer4Admin.Admin.EntityFramework.Shared.DbContexts
{
    public class AdminIdentityDbContext : IdentityDbContext<UserIdentity, IdentityRole<TKey>, string, IdentityUserClaim<TKey>, IdentityUserRole<TKey>, IdentityUserLogin<TKey>, IdentityRoleClaim<TKey>, IdentityUserToken<TKey>>
    {
        public AdminIdentityDbContext(DbContextOptions<AdminIdentityDbContext> options) : base(options)
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

            builder.Entity<UserIdentity>().ToTable(TableConsts.IdentityUsers);
            builder.Entity<IdentityUserLogin<TKey>>().ToTable(TableConsts.IdentityUserLogins);
            builder.Entity<IdentityUserClaim<TKey>>().ToTable(TableConsts.IdentityUserClaims);
            builder.Entity<IdentityUserToken<TKey>>().ToTable(TableConsts.IdentityUserTokens);
        }
    }
}





