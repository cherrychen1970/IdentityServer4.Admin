using System.ComponentModel.DataAnnotations;
using Skoruba.Admin.BusinessLogic.Identity.Dtos.Identity.Base;
using Skoruba.Admin.BusinessLogic.Identity.Dtos.Identity.Interfaces;

namespace Skoruba.Admin.BusinessLogic.Identity.Dtos.Identity
{
    public class RoleDto<TKey> : BaseRoleDto<TKey>, IRoleDto
    {      
        [Required]
        public string Name { get; set; }
    }
}