using Microsoft.EntityFrameworkCore;
using IdDbContexts=IdentityServer4.EntityFramework.DbContexts;
using Entities=IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore.Design;
using IdentityServer4.EntityFramework.Options;

namespace id4.Data
{
   public class SqliteConfContextFactory : IDesignTimeDbContextFactory<SqliteConfigurationDbContext>
    {
        public SqliteConfigurationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SqliteConfigurationDbContext>();
            optionsBuilder.UseSqlite("Data Source=vault.db");

            return new SqliteConfigurationDbContext(optionsBuilder.Options);
        }
    }
    public class NpgsqlConfigurationDbContext : IdDbContexts.ConfigurationDbContext<NpgsqlConfigurationDbContext>
    {
        public NpgsqlConfigurationDbContext(DbContextOptions<NpgsqlConfigurationDbContext> options)
            : base(options, new ConfigurationStoreOptions())
        {
        }
    }

    public class SqliteConfigurationDbContext :  IdDbContexts.ConfigurationDbContext<SqliteConfigurationDbContext>
    {
        public DbSet<Entities.ClientGrantType> ClientGrantTypes {get;set;}
        public DbSet<Entities.ClientSecret> ClientSecrets {get;set;}
        public DbSet<Entities.ClientRedirectUri> ClientRedirectUris {get;set;}
        public DbSet<Entities.ClientScope> clientScopes {get;set;}


        public SqliteConfigurationDbContext(DbContextOptions<SqliteConfigurationDbContext> options)
            : base(options, new ConfigurationStoreOptions())
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=id4.db");
            }
        }
    }
}