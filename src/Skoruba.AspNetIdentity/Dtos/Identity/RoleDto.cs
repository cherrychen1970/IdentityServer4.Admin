using System.ComponentModel.DataAnnotations;
using Skoruba.AspNetIdentity.Dtos.Base;
using Skoruba.AspNetIdentity.Dtos.Interfaces;

namespace Skoruba.AspNetIdentity.Dtos
{
    public class RoleDto<TKey> : BaseRoleDto<TKey>, IRoleDto
    {      
        [Required]
        public string Name { get; set; }
    }
}