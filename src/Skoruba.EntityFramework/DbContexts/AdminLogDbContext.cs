using Microsoft.EntityFrameworkCore;
using Skoruba.Admin.EntityFramework.Constants;
using Skoruba.Admin.EntityFramework.Entities;
using Skoruba.Admin.EntityFramework.Interfaces;

namespace Skoruba.Admin.EntityFramework.Shared.DbContexts
{
    public class AdminLogDbContext : DbContext
    {
        public DbSet<Log> Logs { get; set; }

        public AdminLogDbContext(DbContextOptions<AdminLogDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ConfigureLogContext(builder);
        }

        private void ConfigureLogContext(ModelBuilder builder)
        {
            builder.Entity<Log>(log =>
            {
                log.ToTable(TableConsts.Logging);
                log.HasKey(x => x.Id);
                log.Property(x => x.Level).HasMaxLength(128);
            });
        }
    }
}
