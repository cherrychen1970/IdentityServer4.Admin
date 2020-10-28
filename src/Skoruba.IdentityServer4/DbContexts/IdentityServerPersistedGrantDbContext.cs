using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Skoruba.EntityFramework.Interfaces;

namespace Skoruba.EntityFramework.Shared.DbContexts
{
    public class IdentityServerPersistedGrantDbContext : PersistedGrantDbContext<IdentityServerPersistedGrantDbContext>
    {
        public IdentityServerPersistedGrantDbContext(DbContextOptions<IdentityServerPersistedGrantDbContext> options, OperationalStoreOptions storeOptions)
            : base(options, storeOptions)
        {
        }
    }
}