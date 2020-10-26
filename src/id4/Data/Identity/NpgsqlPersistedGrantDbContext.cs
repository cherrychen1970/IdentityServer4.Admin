using Microsoft.EntityFrameworkCore;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;

namespace id4.Data
{
    public class NpgsqlPersistedGrantDbContext : PersistedGrantDbContext
    {
        public NpgsqlPersistedGrantDbContext(DbContextOptions<PersistedGrantDbContext> options,OperationalStoreOptions storeOptions)
            : base(options,storeOptions)
        {
        }
    }
}