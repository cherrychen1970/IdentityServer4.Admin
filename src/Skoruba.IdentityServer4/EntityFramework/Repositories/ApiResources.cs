using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.EntityFramework.Entities;
using AutoMapper;

using Bluebird.Repositories;
using Skoruba.IdentityServer4.Models;
using Skoruba.IdentityServer4.EntityFramework.DbContexts;
using ApiResource = IdentityServer4.EntityFramework.Entities.ApiResource;
using Microsoft.Extensions.Logging;

namespace Skoruba.IdentityServer4.EntityFramework.Repositories
{
    public class ApiResourceRepository
    : AdminConfigurationRepository<ApiResource>
    {
        public ApiResourceRepository(AdminConfigurationDbContext dbContext, IMapper mapper, ILogger<ApiResourceRepository> logger)
        : base(dbContext, mapper, logger)
        { }
    }
    public class ApiResourcePropertyRepository
  : AdminConfigurationRepository<ApiResourceProperty>
    {
        public ApiResourcePropertyRepository(AdminConfigurationDbContext dbContext, IMapper mapper, ILogger<ApiResourcePropertyRepository> logger)
            : base(dbContext, mapper, logger)
        {
        }
    }
    public class ApiScopeRepository
    : AdminConfigurationRepository<ApiScope>
    {
        public ApiScopeRepository(AdminConfigurationDbContext dbContext, IMapper mapper, ILogger<ApiScopeRepository> logger)
        : base(dbContext, mapper, logger)
        { }
    }

    public class ApiSecretRepository
    : AdminConfigurationRepository<ApiResourceSecret>
    {
        public ApiSecretRepository(AdminConfigurationDbContext dbContext, IMapper mapper, ILogger<ApiSecretRepository> logger)
        : base(dbContext, mapper, logger)
        { }
    }

    public class ApiScopeClaimRepository
    : AdminConfigurationRepository<ApiScopeClaim>
    {
        public ApiScopeClaimRepository(AdminConfigurationDbContext dbContext, IMapper mapper, ILogger<ApiScopeClaimRepository> logger)
         : base(dbContext, mapper, logger)
        { }

    }

    public class ApiResourceClaimRepository
    : AdminConfigurationRepository<ApiResourceClaim>
    {
        public ApiResourceClaimRepository(AdminConfigurationDbContext dbContext, IMapper mapper, ILogger<ApiResourceClaimRepository> logger)
         : base(dbContext, mapper, logger)
        { }
    }    
}