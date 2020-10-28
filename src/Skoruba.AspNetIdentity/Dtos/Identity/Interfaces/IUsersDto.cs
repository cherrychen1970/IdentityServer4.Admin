using System.Collections.Generic;

namespace Skoruba.AspNetIdentity.Dtos.Interfaces
{
    public interface IUsersDto
    {
        int PageSize { get; set; }
        int TotalCount { get; set; }
        List<IUserDto> Users { get; }
    }
}
