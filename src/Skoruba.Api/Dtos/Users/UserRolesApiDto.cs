using System.Collections.Generic;

namespace Skoruba.Admin.Api.Dtos.Users
{
    public class UserRolesApiDto<RoleDto<TKey>>
    {
        public UserRolesApiDto()
        {
            Roles = new List<RoleDto<TKey>>();
        }

        public List<RoleDto<TKey>> Roles { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }
    }
}