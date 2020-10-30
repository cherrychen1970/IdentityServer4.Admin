using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Skoruba.Repositories;

using Skoruba.IdentityServer4.EntityFramework.DbContexts;

namespace Skoruba.IdentityServer4.EntityFramework.Repositories
{
    public class AdminConfigurationRepository<TEntity,TModel> : EntityRepository<AdminConfigurationDbContext, TEntity, int>        
        where TEntity : class        
    {
        public AdminConfigurationRepository(AdminConfigurationDbContext context, IMapper mapper,ILogger logger
        //, IAuditEventLogger auditEventLogger
        ) : base(context,logger)
        {
        }        
    }
}