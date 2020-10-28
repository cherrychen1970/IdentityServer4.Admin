using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Models;
using Microsoft.EntityFrameworkCore;
using Skoruba.IdentityServer4.Dtos;
using Skoruba.Core.Dtos.Common;
using Skoruba.EntityFramework.Extensions.Extensions;
using Skoruba.EntityFramework.Helpers;
using Skoruba.EntityFramework.Interfaces;
using Skoruba.EntityFramework.Repositories.Interfaces;
using Client = IdentityServer4.EntityFramework.Entities.Client;
using Skoruba.EntityFramework.Shared.DbContexts;
using Skoruba.Core.Repositories;
using Skoruba.Core.Helpers;
using Skoruba.IdentityServer4.Dtos.Configuration;
using AutoMapper;


namespace Skoruba.IdentityServer4.Repositories
{
    public class ClientRepository
        : Repository<ConfDbContext, Client, ClientDto, int>
    {
        public ClientRepository(ConfDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
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
    }

    public class ClientSecretRepository
     : Repository<ConfDbContext, ClientSecret, ClientSecretDto, int>
    {
        public ClientSecretRepository(ConfDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }
    }

    public class ClientScopeRepository
     : Repository<ConfDbContext, ClientScope, ClientScope, int>
    {
        public ClientScopeRepository(ConfDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }
    }
    public class ClientPropertyRepository
     : Repository<ConfDbContext, ClientProperty, ClientPropertyDto, int>
    {
        public ClientPropertyRepository(ConfDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }
    }
    public class ClientRepository1 //: IClientRepository    
    {
        protected readonly ConfDbContext DbContext;
        public bool AutoSaveChanges { get; set; } = true;

        public ClientRepository1(ConfDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public virtual async Task<List<string>> GetScopesAsync(string scope, int limit = 0)
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

        public virtual List<string> GetGrantTypes(string grant, int limit = 0)
        {
            var filteredGrants = ClientConsts.GetGrantTypes()
                .WhereIf(!string.IsNullOrWhiteSpace(grant), x => x.Contains(grant))
                .TakeIf(x => x, limit > 0, limit)
                .ToList();

            return filteredGrants;
        }

        public virtual List<SelectItem> GetProtocolTypes()
        {
            return ClientConsts.GetProtocolTypes();
        }

        public virtual List<SelectItem> GetSecretTypes()
        {
            var secrets = new List<SelectItem>();
            secrets.AddRange(ClientConsts.GetSecretTypes().Select(x => new SelectItem(x, x)));

            return secrets;
        }

        public virtual List<string> GetStandardClaims(string claim, int limit = 0)
        {
            var filteredClaims = ClientConsts.GetStandardClaims()
                .WhereIf(!string.IsNullOrWhiteSpace(claim), x => x.Contains(claim))
                .TakeIf(x => x, limit > 0, limit)
                .ToList();

            return filteredClaims;
        }

        public virtual List<SelectItem> GetAccessTokenTypes()
        {
            var accessTokenTypes = EnumHelpers.ToSelectList<AccessTokenType>();
            return accessTokenTypes;
        }

        public virtual List<SelectItem> GetTokenExpirations()
        {
            var tokenExpirations = EnumHelpers.ToSelectList<TokenExpiration>();
            return tokenExpirations;
        }

        public virtual List<SelectItem> GetTokenUsage()
        {
            var tokenUsage = EnumHelpers.ToSelectList<TokenUsage>();
            return tokenUsage;
        }

        public virtual List<SelectItem> GetHashTypes()
        {
            var hashTypes = EnumHelpers.ToSelectList<HashType>();
            return hashTypes;
        }
    }
}