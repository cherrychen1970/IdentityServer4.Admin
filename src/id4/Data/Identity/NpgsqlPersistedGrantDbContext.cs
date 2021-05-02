using Microsoft.EntityFrameworkCore;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore.Design;
using IdentityServer4.EntityFramework.Options;

namespace id4.Data
{
   public class SqliteGrantContextFactory : IDesignTimeDbContextFactory<SqlitePersistedGrantDbContext>
    {
        public SqlitePersistedGrantDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SqlitePersistedGrantDbContext>();
            optionsBuilder.UseSqlite("Data Source=vault.db");

            return new SqlitePersistedGrantDbContext(optionsBuilder.Options);
        }
    }
    public class NpgsqlPersistedGrantDbContext : PersistedGrantDbContext
    {
        public NpgsqlPersistedGrantDbContext(DbContextOptions<PersistedGrantDbContext> options,OperationalStoreOptions storeOptions)
            : base(options,storeOptions)
        {
        }
    }
    public class SqlitePersistedGrantDbContext : PersistedGrantDbContext<SqlitePersistedGrantDbContext>
    {
        public SqlitePersistedGrantDbContext(DbContextOptions<SqlitePersistedGrantDbContext> options)
            : base(options,new OperationalStoreOptions())
        {
        }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=grants.db");
            }
        }
    }
}