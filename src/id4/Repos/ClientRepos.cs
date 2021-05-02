using Microsoft.Extensions.Logging;

using Id4Models = IdentityServer4.Models;
//using Id4Entities = IdentityServer4.EntityFramework.Entities;
//using IdentityServer4.EntityFramework.DbContexts;
using Bluebird.Repositories;
using AutoMapper;


namespace id4
{
    public class ClientRepo : EntityRepository<Data.ConfigurationDbContext, Models.Entities.Client, int>
    {
        public ClientRepo(Data.ConfigurationDbContext context, IMapper mapper, ILogger<ClientRepo> logger
        ) : base(context,mapper, logger)
        {

        }
    }

}