using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.EntityFramework.Entities;
using AutoMapper;

using Bluebird.Repositories;
using Skoruba.IdentityServer4.Models;
using Skoruba.IdentityServer4.EntityFramework.DbContexts;
using Microsoft.Extensions.Logging;

namespace Skoruba.IdentityServer4.EntityFramework.Repositories
{
    public class IdentityResourceRepository
    : AdminConfigurationRepository<IdentityResource>
    {
        public IdentityResourceRepository(AdminConfigurationDbContext dbContext, IMapper mapper, ILogger<IdentityResourceRepository> logger)
        : base(dbContext, mapper, logger)
        { }
    }
    public class IdentityPropertyRepository
  : AdminConfigurationRepository<IdentityResourceProperty>
    {
        public IdentityPropertyRepository(AdminConfigurationDbContext dbContext, IMapper mapper, ILogger<IdentityPropertyRepository> logger)
            : base(dbContext, mapper, logger)
        {
        }
    }
    public class IdentityClaimRepository
    : AdminConfigurationRepository<IdentityResourceClaim>
    {
        public IdentityClaimRepository(AdminConfigurationDbContext dbContext, IMapper mapper, ILogger<IdentityClaimRepository> logger)
         : base(dbContext, mapper, logger)
        { }
    }    
}