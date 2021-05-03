using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using IdentityServer4.EntityFramework.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;

using Skoruba.Linq.Extensions;
using Bluebird.Repositories;
using Bluebird.Linq;
using Skoruba.IdentityServer4.Models;
using Skoruba.IdentityServer4.EntityFramework.DbContexts;
using ApiResource = IdentityServer4.EntityFramework.Entities.ApiResource;


namespace Skoruba.IdentityServer4.EntityFramework.Repositories
{
    public class ClientRepository
        : AdminConfigurationRepository<Client>
    {
        public ClientRepository(AdminConfigurationDbContext dbContext, IMapper mapper, ILogger<ClientRepository> logger)
         : base(dbContext, mapper, logger)
        {
        }


        public dynamic GetRedirectUris(int id)
        {
            var list = _context.ClientRedirectUris.Where(x=>x.ClientId==id).Select(x=>x.RedirectUri);
            return new {Id=id, RedirectUris=list};

        }

        public dynamic GetScopes(int id)
        {
            var list = _context.ClientScopes.Where(x=>x.ClientId==id).Select(x=>x.Scope);
            return new {Id=id, AllowedScopes=list};
        }

        //allowedGrantTypes
        public dynamic GetGrantTypes(int id)
        {
            var list = _context.ClientGrantTypes.Where(x=>x.ClientId==id).Select(x=>x.GrantType);
            return new {Id=id, AllowedGrantTypes=list};
        }
        /*
            public void UpdateClaims(int id, ClientClaimDto[] claims)
        {
            var claimsInput = _mapper.Map<ClientClaim[]>(claims);
            foreach (var item in claimsInput)
                item.ClientId = id;

            var oldClaims = _context.ClientClaims.Where(x => x.ClientId == id).ToHashSet();
            _context.UpdateCollection<ClientClaim>(oldClaims, claimsInput, (x, y) => x.Id == y.Id);
        }
        public void UpdateRedirectUris(int id, string[] redirectUris)
        {
            var input = redirectUris.Select(x => new ClientRedirectUri { RedirectUri = x, ClientId = id, Id=0 }).ToArray();
            var old = _context.ClientRedirectUris.Where(x => x.ClientId == id).ToHashSet();
            _context.UpdateCollection<ClientRedirectUri>(old, input, (x, y) => x.RedirectUri == y.RedirectUri,false);
        }
        public void UpdateScopes(int id, string[] scopes)
        {
            var input = scopes.Select(x => new ClientScope { Scope = x, ClientId = id, Id=0 }).ToArray();
            var old = _context.ClientScopes.Where(x => x.ClientId == id).ToHashSet();
            _context.UpdateCollection<ClientScope>(old, input, (x, y) => x.Scope == y.Scope,false);
        }
        public void UpdateGrantTypes(int id, string[] grantTypes)
        {
            var input = grantTypes.Select(x => new ClientGrantType { GrantType = x, ClientId = id, Id=0 }).ToArray();
            var old = _context.ClientGrantTypes.Where(x => x.ClientId == id).ToHashSet();
            _context.UpdateCollection<ClientGrantType>(old, input, (x, y) => x.GrantType == y.GrantType,false);
        }
        */
    }

    public class ClientPropertyRepository
     : AdminConfigurationRepository<ClientProperty>
    {
        public ClientPropertyRepository(AdminConfigurationDbContext dbContext, IMapper mapper, ILogger<ClientPropertyRepository> logger)
        : base(dbContext, mapper, logger)
        {
        }
    }

    public class ClientClaimRepository
 : AdminConfigurationRepository<ClientClaim>
    {
        public ClientClaimRepository(AdminConfigurationDbContext dbContext, IMapper mapper, ILogger<ClientClaimRepository> logger)
        : base(dbContext, mapper, logger)
        {
        }
    }

    public class ClientRedirectUriRepository
    : AdminConfigurationRepository<ClientRedirectUri>
    {
        public ClientRedirectUriRepository(AdminConfigurationDbContext dbContext, IMapper mapper, ILogger<ClientRedirectUriRepository> logger)
        : base(dbContext, mapper, logger)
        {
        }
    }
    public class ClientGrantTypeRepository
    : AdminConfigurationRepository<ClientGrantType>
    {
        public ClientGrantTypeRepository(AdminConfigurationDbContext dbContext, IMapper mapper, ILogger<ClientGrantTypeRepository> logger)
        : base(dbContext, mapper, logger)
        {
        }
    }
}