﻿using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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
}