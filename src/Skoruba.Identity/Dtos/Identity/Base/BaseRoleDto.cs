using Skoruba.Identity.Dtos.Identity.Interfaces;
using System.Collections.Generic;

namespace Skoruba.Identity.Dtos.Identity.Base
{
    public class BaseRoleDto<TKey> : IBaseRoleDto
    {
        public TKey Id { get; set; }

        object IBaseRoleDto.Id => Id;
    }
}