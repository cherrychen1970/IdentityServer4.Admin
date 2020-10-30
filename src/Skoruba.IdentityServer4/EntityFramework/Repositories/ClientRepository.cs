using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using IdentityServer4.EntityFramework.Entities;
using AutoMapper;

using Skoruba.Linq.Extensions;
using Skoruba.Repositories;
using Skoruba.IdentityServer4.Models;
using Skoruba.IdentityServer4.EntityFramework.DbContexts;
using ApiResource = IdentityServer4.EntityFramework.Entities.ApiResource;


namespace Skoruba.IdentityServer4.EntityFramework.Repositories
{
    public class ClientRepository
        : AdminConfigurationRepository<Client,ClientDto>
    {
        public ClientRepository(AdminConfigurationDbContext dbContext, IMapper mapper, ILogger<ClientRepository> logger) 
         : base(dbContext,mapper, logger)
        {
        }
      
        protected override IQueryable<Client> OnSelect(DbSet<Client> set)
        {
            return DbContext.Clients
                .Include(x => x.AllowedGrantTypes)
                .Include(x => x.RedirectUris)
                .Include(x => x.PostLogoutRedirectUris)
                .Include(x => x.AllowedScopes)
                .Include(x => x.ClientSecrets)
                .Include(x => x.Claims)
                .Include(x => x.IdentityProviderRestrictions)
                .Include(x => x.AllowedCorsOrigins)
                .Include(x => x.Properties);
        }
        
        public virtual async Task<List<string>> SearchScopesAsync(string scope, int limit = 0)
        {
            var identityResources = await DbContext.IdentityResources
                .WhereIf(!string.IsNullOrEmpty(scope), x => x.Name.Contains(scope))
                .TakeIf(x => x.Id, limit > 0, limit)
                .Select(x => x.Name).ToListAsync();

            var apiScopes = await DbContext.ApiScopes
                .WhereIf(!string.IsNullOrEmpty(scope), x => x.Name.Contains(scope))
                .TakeIf(x => x.Id, limit > 0, limit)
                .Select(x => x.Name).ToListAsync();

            var scopes = identityResources.Concat(apiScopes).TakeIf(x => x, limit > 0, limit).ToList();

            return scopes;
        }        
    }

    

    public class ClientSecretRepository
     : AdminConfigurationRepository<ClientSecret, ClientSecretDto>
    {
        public ClientSecretRepository(AdminConfigurationDbContext dbContext, IMapper mapper,ILogger<ClientSecretRepository> logger) 
        : base(dbContext, mapper,logger)
        {
        }
    }

    public class ClientScopeRepository
     : AdminConfigurationRepository<ClientScope, ClientScope>
    {
        public ClientScopeRepository(AdminConfigurationDbContext dbContext, IMapper mapper,ILogger<ClientScopeRepository> logger) 
        : base(dbContext, mapper,logger)
        {
        }
    }
    public class ClientPropertyRepository
     : AdminConfigurationRepository<ClientProperty, ClientPropertyDto>
    {
        public ClientPropertyRepository(AdminConfigurationDbContext dbContext, IMapper mapper,ILogger<ClientPropertyRepository> logger) 
        : base(dbContext, mapper,logger)
        {
        }
    }
    
        public class ClientClaimRepository
     : AdminConfigurationRepository<ClientClaim, ClientClaim>
    {
        public ClientClaimRepository(AdminConfigurationDbContext dbContext, IMapper mapper,ILogger<ClientClaimRepository> logger) 
        : base(dbContext, mapper,logger)
        {
        }
    }

        public class ClientRedirectUriRepository
        : AdminConfigurationRepository<ClientRedirectUri, ClientRedirectUri>
        {
            public ClientRedirectUriRepository(AdminConfigurationDbContext dbContext, IMapper mapper,ILogger<ClientRedirectUriRepository> logger) 
            : base(dbContext, mapper,logger)
            {
            }
        }
        public class ClientGrantTypeRepository
        : AdminConfigurationRepository<ClientGrantType, ClientGrantType>
        {
            public ClientGrantTypeRepository(AdminConfigurationDbContext dbContext, IMapper mapper,ILogger<ClientGrantTypeRepository> logger) 
            : base(dbContext, mapper,logger)
            {
            }
        }
}