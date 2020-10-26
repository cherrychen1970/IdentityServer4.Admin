using Microsoft.EntityFrameworkCore;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;

namespace id4.Data
{
    public class NpgsqlConfigurationDbContext : ConfigurationDbContext
    {
        public NpgsqlConfigurationDbContext(DbContextOptions<ConfigurationDbContext> options,ConfigurationStoreOptions storeOptions)
            : base(options,storeOptions)
        {
        }
    }
}