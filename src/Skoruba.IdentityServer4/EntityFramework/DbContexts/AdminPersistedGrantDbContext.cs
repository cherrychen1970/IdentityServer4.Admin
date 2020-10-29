using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;

namespace Skoruba.IdentityServer4.EntityFramework.DbContexts
{
    public class AdminPersistedGrantDbContext : PersistedGrantDbContext<AdminPersistedGrantDbContext>
    {
        public AdminPersistedGrantDbContext(DbContextOptions<AdminPersistedGrantDbContext> options, OperationalStoreOptions storeOptions)
            : base(options, storeOptions)
        {
        }
    }
}