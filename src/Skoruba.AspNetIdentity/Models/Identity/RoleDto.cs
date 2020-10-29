using System.ComponentModel.DataAnnotations;
using Skoruba.AspNetIdentity.Models.Base;
using Skoruba.AspNetIdentity.Models.Interfaces;

namespace Skoruba.AspNetIdentity.Models
{
    public class RoleDto<TKey> : BaseRoleDto<TKey>, IRoleDto
    {      
        [Required]
        public string Name { get; set; }
    }
}