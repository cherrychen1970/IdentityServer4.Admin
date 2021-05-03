using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.Extensions.Logging;

using Bluebird.Entity;
using Bluebird.Repositories;

using Skoruba.IdentityServer4.EntityFramework.DbContexts;

namespace Skoruba.IdentityServer4.EntityFramework.Repositories
{
    public class AdminConfigurationRepository<TEntity> : EntityRepository<AdminConfigurationDbContext, TEntity, int>
        where TEntity : class//, IEntity<int>
    {
        public AdminConfigurationRepository(AdminConfigurationDbContext context, IMapper mapper, ILogger logger
        //, IAuditEventLogger auditEventLogger
        ) : base(context, mapper, logger)
        {
        }
    }
}