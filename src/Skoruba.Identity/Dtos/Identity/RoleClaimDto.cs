using System.ComponentModel.DataAnnotations;
using Skoruba.Admin.BusinessLogic.Identity.Dtos.Identity.Base;
using Skoruba.Admin.BusinessLogic.Identity.Dtos.Identity.Interfaces;

namespace Skoruba.Admin.BusinessLogic.Identity.Dtos.Identity
{
    public class RoleClaimDto<TKey> : BaseRoleClaimDto<TKey>, IRoleClaimDto
    {
        [Required]
        public string ClaimType { get; set; }


        [Required]
        public string ClaimValue { get; set; }
    }
}
