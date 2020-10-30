using System;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Skoruba.Repositories;

using Skoruba.AspNetIdentity.EntityFramework.Models;

namespace Skoruba.AspNetIdentity.EntityFramework.Repositories
{
    public class AdminIdentityRepository<TEntity, TModel, TKey> : EntityRepository<AdminIdentityDbContext, TEntity, TKey>
        where TEntity : class
        where TKey : IEquatable<TKey>
    {
        public AdminIdentityRepository(AdminIdentityDbContext context, IMapper mapper, ILogger logger
        //, IAuditEventLogger auditEventLogger
        ) : base(context, logger)
        {
        }
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class UserRepository : AdminIdentityRepository<User, User, string>
    {
        public UserRepository(AdminIdentityDbContext dbContext, IMapper mapper, ILogger<UserRepository> logger)
        : base(dbContext, mapper, logger) { }
    }

    public class UserRoleRepository : AdminIdentityRepository<UserRole, UserRole, int>
    {
        public UserRoleRepository(AdminIdentityDbContext dbContext, IMapper mapper, ILogger<UserRoleRepository> logger)
        : base(dbContext, mapper, logger) { }
    }    
    public class UserClaimRepository : AdminIdentityRepository<UserClaim, UserClaim, int>
    {
        public UserClaimRepository(AdminIdentityDbContext dbContext, IMapper mapper, ILogger<UserClaimRepository> logger)
        : base(dbContext, mapper, logger) { }
    }    
    public class UserLoginRepository : AdminIdentityRepository<UserLogin, UserLogin, string>
    {
        public UserLoginRepository(AdminIdentityDbContext dbContext, IMapper mapper, ILogger<UserLoginRepository> logger)
        : base(dbContext, mapper, logger) { }
    }  
    public class UserTokenRepository : AdminIdentityRepository<UserToken, UserToken, string>
    {
        public UserTokenRepository(AdminIdentityDbContext dbContext, IMapper mapper, ILogger<UserTokenRepository> logger)
        : base(dbContext, mapper, logger) { }
    }            
    public class RoleRepository : AdminIdentityRepository<Role, Role, string>
    {
        public RoleRepository(AdminIdentityDbContext dbContext, IMapper mapper, ILogger<RoleRepository> logger)
        : base(dbContext, mapper, logger) { }
    }

    public class RoleClaimRepository : AdminIdentityRepository<RoleClaim, RoleClaim, int>
    {
        public RoleClaimRepository(AdminIdentityDbContext dbContext, IMapper mapper, ILogger<RoleRepository> logger)
        : base(dbContext, mapper, logger) { }
    }
}