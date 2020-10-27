﻿using Skoruba.Admin.BusinessLogic.Identity.Dtos.Identity.Interfaces;

namespace Skoruba.Admin.BusinessLogic.Identity.Dtos.Identity.Base
{
    public class BaseRoleClaimDto<TKey> : IBaseRoleClaimDto
    {
        public int ClaimId { get; set; }

        public TKey RoleId { get; set; }

        object IBaseRoleClaimDto.RoleId => RoleId;
    }
}