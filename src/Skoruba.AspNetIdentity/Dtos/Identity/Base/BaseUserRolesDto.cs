using Skoruba.AspNetIdentity.Dtos.Interfaces;

namespace Skoruba.AspNetIdentity.Dtos.Base
{
    public class BaseUserRolesDto<TKey> : IBaseUserRolesDto
    {
        public TKey UserId { get; set; }

        public TKey RoleId { get; set; }

        object IBaseUserRolesDto.UserId => UserId;

        object IBaseUserRolesDto.RoleId => RoleId;
    }
}