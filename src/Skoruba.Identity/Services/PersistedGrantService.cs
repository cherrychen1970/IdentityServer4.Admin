﻿using System;
using System.Threading.Tasks;
using Skoruba.AuditLogging.Services;
using Skoruba.Identity.Dtos.Grant;
using Skoruba.Identity.Mappers;
using Skoruba.Identity.Resources;
using Skoruba.Identity.Services.Interfaces;
using Skoruba.Core.ExceptionHandling;
using Skoruba.Core.Events;
using Skoruba.EntityFramework.Identity.Repositories.Interfaces;

namespace Skoruba.Core.ExceptionHandling
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string message):base(message){}

    }
}

namespace Skoruba.Identity.Services
{


    public class PersistedGrantAspNetIdentityService : IPersistedGrantAspNetIdentityService
    {
        protected readonly IPersistedGrantAspNetIdentityRepository PersistedGrantAspNetIdentityRepository;
        protected readonly IPersistedGrantAspNetIdentityServiceResources PersistedGrantAspNetIdentityServiceResources;
        protected readonly IAuditEventLogger AuditEventLogger;

        public PersistedGrantAspNetIdentityService(IPersistedGrantAspNetIdentityRepository persistedGrantAspNetIdentityRepository,
            IPersistedGrantAspNetIdentityServiceResources persistedGrantAspNetIdentityServiceResources,
            IAuditEventLogger auditEventLogger)
        {
            PersistedGrantAspNetIdentityRepository = persistedGrantAspNetIdentityRepository;
            PersistedGrantAspNetIdentityServiceResources = persistedGrantAspNetIdentityServiceResources;
            AuditEventLogger = auditEventLogger;
        }

        public virtual async Task<PersistedGrantsDto> GetPersistedGrantsByUsersAsync(string search, int page = 1, int pageSize = 10)
        {
            var pagedList = await PersistedGrantAspNetIdentityRepository.GetPersistedGrantsByUsersAsync(search, page, pageSize);
            var persistedGrantsDto = pagedList.ToModel();

            await AuditEventLogger.LogEventAsync(new CommonEvent(persistedGrantsDto));

            return persistedGrantsDto;
        }

        public virtual async Task<PersistedGrantsDto> GetPersistedGrantsByUserAsync(string subjectId, int page = 1, int pageSize = 10)
        {
            var exists = await PersistedGrantAspNetIdentityRepository.ExistsPersistedGrantsAsync(subjectId);
            if (!exists) throw new NotFoundException(subjectId);

            var pagedList = await PersistedGrantAspNetIdentityRepository.GetPersistedGrantsByUserAsync(subjectId, page, pageSize);
            var persistedGrantsDto = pagedList.ToModel();

            await AuditEventLogger.LogEventAsync(new CommonEvent(persistedGrantsDto));

            return persistedGrantsDto;
        }

        public virtual async Task<PersistedGrantDto> GetPersistedGrantAsync(string key)
        {
            var persistedGrant = await PersistedGrantAspNetIdentityRepository.GetPersistedGrantAsync(key);
            if (persistedGrant == null) throw new NotFoundException(key);
            var persistedGrantDto = persistedGrant.ToModel();

            await AuditEventLogger.LogEventAsync(new CommonEvent(persistedGrantDto));

            return persistedGrantDto;
        }

        public virtual async Task<int> DeletePersistedGrantAsync(string key)
        {
            var exists = await PersistedGrantAspNetIdentityRepository.ExistsPersistedGrantAsync(key);
            if (!exists) throw new NotFoundException(key);

            var deleted = await PersistedGrantAspNetIdentityRepository.DeletePersistedGrantAsync(key);

            await AuditEventLogger.LogEventAsync(new CommonEvent(key));

            return deleted;
        }

        public virtual async Task<int> DeletePersistedGrantsAsync(string userId)
        {
            var exists = await PersistedGrantAspNetIdentityRepository.ExistsPersistedGrantsAsync(userId);
            if (!exists) throw new NotFoundException(userId);

            var deleted = await PersistedGrantAspNetIdentityRepository.DeletePersistedGrantsAsync(userId);

            await AuditEventLogger.LogEventAsync(new CommonEvent(userId));

            return deleted;
        }
    }
}
