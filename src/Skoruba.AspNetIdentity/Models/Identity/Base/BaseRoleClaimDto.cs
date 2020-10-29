using Skoruba.AspNetIdentity.Models.Interfaces;

namespace Skoruba.AspNetIdentity.Models.Base
{
    public class BaseRoleClaimDto<TKey> : IBaseRoleClaimDto
    {
        public int ClaimId { get; set; }

        public TKey RoleId { get; set; }

        object IBaseRoleClaimDto.RoleId => RoleId;
    }
}