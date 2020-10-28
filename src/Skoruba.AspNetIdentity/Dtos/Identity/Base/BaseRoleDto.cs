using Skoruba.AspNetIdentity.Dtos.Interfaces;
using System.Collections.Generic;

namespace Skoruba.AspNetIdentity.Dtos.Base
{
    public class BaseRoleDto<TKey> : IBaseRoleDto
    {
        public TKey Id { get; set; }

        object IBaseRoleDto.Id => Id;
    }
}