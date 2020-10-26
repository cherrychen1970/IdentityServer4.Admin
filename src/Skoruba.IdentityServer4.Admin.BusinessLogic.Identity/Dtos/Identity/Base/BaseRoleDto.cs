using Skoruba.Admin.BusinessLogic.Identity.Dtos.Identity.Interfaces;
using System.Collections.Generic;

namespace Skoruba.Admin.BusinessLogic.Identity.Dtos.Identity.Base
{
    public class BaseRoleDto<TRoleId> : IBaseRoleDto
    {
        public TRoleId Id { get; set; }

        public bool IsDefaultId() => EqualityComparer<TRoleId>.Default.Equals(Id, default(TRoleId));

        object IBaseRoleDto.Id => Id;
    }
}