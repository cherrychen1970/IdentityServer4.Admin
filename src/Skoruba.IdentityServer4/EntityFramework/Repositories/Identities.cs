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
    : AdminConfigurationRepository<IdentityResource, IdentityResourceDto>
    {
        public IdentityResourceRepository(AdminConfigurationDbContext dbContext, IMapper mapper, ILogger<IdentityResourceRepository> logger)
        : base(dbContext, mapper, logger)
        { }
    }
    public class IdentityPropertyRepository
  : AdminConfigurationRepository<IdentityResourceProperty, IdentityResourcePropertyDto>
    {
        public IdentityPropertyRepository(AdminConfigurationDbContext dbContext, IMapper mapper, ILogger<IdentityPropertyRepository> logger)
            : base(dbContext, mapper, logger)
        {
        }
    }
    public class IdentityClaimRepository
    : AdminConfigurationRepository<IdentityClaim, IdentityClaim>
    {
        public IdentityClaimRepository(AdminConfigurationDbContext dbContext, IMapper mapper, ILogger<IdentityClaimRepository> logger)
         : base(dbContext, mapper, logger)
        { }
    }    
}