using System.Collections.Generic;

namespace Skoruba.AspNetIdentity.Dtos.Interfaces
{
    public interface IRoleClaimsDto : IRoleClaimDto
    {
        string RoleName { get; set; }
        List<IRoleClaimDto> Claims { get; }
        int TotalCount { get; set; }
        int PageSize { get; set; }
    }
}
