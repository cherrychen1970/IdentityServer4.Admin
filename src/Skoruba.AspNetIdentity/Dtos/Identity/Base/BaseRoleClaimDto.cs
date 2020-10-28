using Skoruba.AspNetIdentity.Dtos.Interfaces;

namespace Skoruba.AspNetIdentity.Dtos.Base
{
    public class BaseRoleClaimDto<TKey> : IBaseRoleClaimDto
    {
        public int ClaimId { get; set; }

        public TKey RoleId { get; set; }

        object IBaseRoleClaimDto.RoleId => RoleId;
    }
}