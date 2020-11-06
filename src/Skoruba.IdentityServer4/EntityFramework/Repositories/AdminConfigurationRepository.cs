using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Bluebird.Repositories.EntityFramework;

using Skoruba.IdentityServer4.EntityFramework.DbContexts;

namespace Skoruba.IdentityServer4.EntityFramework.Repositories
{
    public class AdminConfigurationRepository<TEntity,TModel> : Repository<AdminConfigurationDbContext, TEntity,TModel, int>        
        where TEntity : class     
        where TModel : class   
    {
        public AdminConfigurationRepository(AdminConfigurationDbContext context, IMapper mapper,ILogger logger
        //, IAuditEventLogger auditEventLogger
        ) : base(context,mapper,logger)
        {
        }        
    }
}