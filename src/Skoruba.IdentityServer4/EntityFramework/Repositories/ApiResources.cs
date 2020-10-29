using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.EntityFramework.Entities;
using AutoMapper;

using Skoruba.Repositories;
using Skoruba.IdentityServer4.Models;
using Skoruba.IdentityServer4.EntityFramework.DbContexts;
using ApiResource = IdentityServer4.EntityFramework.Entities.ApiResource;
using Microsoft.Extensions.Logging;

namespace Skoruba.IdentityServer4.EntityFramework.Repositories
{
    public class ApiResourcePropertyRepository
  : Repository<AdminConfigurationDbContext, ApiResourceProperty, ApiResourcePropertyDto, int>
    {
        public ApiResourcePropertyRepository(AdminConfigurationDbContext dbContext, IMapper mapper, ILogger<ApiResourceRepository> logger) 
            : base(dbContext, mapper,logger)
        {
        }
    }
    public class ApiResourceRepository
    : Repository<AdminConfigurationDbContext, ApiResource, ApiResourceDto, int>
    {
        public ApiResourceRepository(AdminConfigurationDbContext dbContext, IMapper mapper, ILogger<ApiResourceRepository> logger) 
        : base(dbContext, mapper,logger)
        {}
        override protected IQueryable<ApiResource> OnSelect(DbSet<ApiResource> set)
        => set.Include(x => x.UserClaims);
    }
        
    public class ApiScopeRepository
    : Repository<AdminConfigurationDbContext, ApiScope, ApiScopeDto, int>
    {
        public ApiScopeRepository(AdminConfigurationDbContext dbContext, IMapper mapper,ILogger<ApiScopeRepository> logger) 
        : base(dbContext, mapper,logger)
        {}
        override protected IQueryable<ApiScope> OnSelect(DbSet<ApiScope> set)
        => set.Include(x => x.UserClaims);
    }

    public class ApiSecretRepository
    : Repository<AdminConfigurationDbContext, ApiSecret, ApiSecretDto, int>
    {
        public ApiSecretRepository(AdminConfigurationDbContext dbContext, IMapper mapper,ILogger<ApiSecretRepository> logger)  
        : base(dbContext, mapper,logger)
        {}
        override protected IQueryable<ApiSecret> OnSelect(DbSet<ApiSecret> set)
        => set;
    }

    public class ApiScopeClaimRepository
    : Repository<AdminConfigurationDbContext, ApiScopeClaim, ApiScopeClaim, int>
    {
        public ApiScopeClaimRepository(AdminConfigurationDbContext dbContext, IMapper mapper,ILogger<ApiScopeClaimRepository> logger) 
         : base(dbContext, mapper,logger)
        {}
        override protected IQueryable<ApiScopeClaim> OnSelect(DbSet<ApiScopeClaim> set)
        => set;
    }  
}