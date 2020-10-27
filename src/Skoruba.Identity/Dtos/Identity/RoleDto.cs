using System.ComponentModel.DataAnnotations;
using Skoruba.Identity.Dtos.Identity.Base;
using Skoruba.Identity.Dtos.Identity.Interfaces;

namespace Skoruba.Identity.Dtos.Identity
{
    public class RoleDto<TKey> : BaseRoleDto<TKey>, IRoleDto
    {      
        [Required]
        public string Name { get; set; }
    }
}