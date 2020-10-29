using Skoruba.AspNetIdentity.Models.Interfaces;
using System.Collections.Generic;

namespace Skoruba.AspNetIdentity.Models.Base
{
    public class BaseRoleDto<TKey> : IBaseRoleDto
    {
        public TKey Id { get; set; }

        object IBaseRoleDto.Id => Id;
    }
}