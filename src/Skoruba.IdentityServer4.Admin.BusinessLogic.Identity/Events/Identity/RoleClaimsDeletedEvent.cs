﻿using Skoruba.AuditLogging.Events;

namespace Skoruba.Admin.BusinessLogic.Identity.Events.Identity
{
    public class RoleClaimsDeletedEvent<TRoleClaimsDto> : AuditEvent
    {
        public TRoleClaimsDto RoleClaim { get; set; }

        public RoleClaimsDeletedEvent(TRoleClaimsDto roleClaim)
        {
            RoleClaim = roleClaim;
        }
    }
}