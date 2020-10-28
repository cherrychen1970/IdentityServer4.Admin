using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Skoruba.Core.Dtos.Common;
using Skoruba.Core.Helpers;
using Skoruba.EntityFramework.Extensions.Extensions;
using IdentityServer4.EntityFramework.DbContexts;
using Skoruba.EntityFramework.Interfaces;
using Skoruba.EntityFramework.Repositories.Interfaces;
using ApiResource = IdentityServer4.EntityFramework.Entities.ApiResource;
using Skoruba.EntityFramework.Shared.DbContexts;
using Skoruba.Core.Repositories;
using Skoruba.IdentityServer4.Dtos.Configuration;
using AutoMapper;

namespace Skoruba.IdentityServer4.Repositories
{
    public class ApiResourcePropertyRepository
  : Repository<ConfDbContext, ApiResourceProperty, ApiResourcePropertyDto, int>
    {
        public ApiResourcePropertyRepository(ConfDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }
    }
    public class ApiResourceRepository
    : Repository<ConfDbContext, ApiResource, ApiResourceDto, int>
    {
        public ApiResourceRepository(ConfDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {}
        override protected IQueryable<ApiResource> OnSelect(DbSet<ApiResource> set)
        => set.Include(x => x.UserClaims);
    }
        
    public class ApiScopeRepository
    : Repository<ConfDbContext, ApiScope, ApiScopeDto, int>
    {
        public ApiScopeRepository(ConfDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {}
        override protected IQueryable<ApiScope> OnSelect(DbSet<ApiScope> set)
        => set.Include(x => x.UserClaims);
    }

    public class ApiSecretRepository
    : Repository<ConfDbContext, ApiSecret, ApiSecretDto, int>
    {
        public ApiSecretRepository(ConfDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {}
        override protected IQueryable<ApiSecret> OnSelect(DbSet<ApiSecret> set)
        => set;
    }

    public class ApiScopeClaimRepository
    : Repository<ConfDbContext, ApiScopeClaim, ApiScopeClaim, int>
    {
        public ApiScopeClaimRepository(ConfDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {}
        override protected IQueryable<ApiScopeClaim> OnSelect(DbSet<ApiScopeClaim> set)
        => set;
    }  
}